using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using FlightSimulator.ViewModels;

namespace FlightSimulator.Model
{
    /*the client to send commands through*/
    class CommandClient
    {
        /*the wrapper to the TCPClient, in order to run it on a seperate thread easily*/
        class PrivateClient
        {
            private TcpClient server;
            private DataQueue commands;
            /*initialize infomation on the client*/
            public PrivateClient(string ip, int port, DataQueue commandsQue)
            {
                commands = commandsQue;
                //create the TCP client
                this.server = new TcpClient();
                System.Diagnostics.Debug.WriteLine("Client connecting on ip = {0} and port = {1}", ip, port);
                //connect the TCP client to the server
                server.Connect(ip, port);
            }
            /*send all of the commands that there are to send to the flight simulator*/
            public void SendCommands()
            {
                //basic reader params
                using (NetworkStream ns = server.GetStream())
                using (BinaryWriter writer = new BinaryWriter(ns))
                {
                    //run always on the background
                    while (true)
                    {
                        //run only while the server is connected
                        while (server.Connected)
                        {
                            //clear the command queue if it's not empty
                            while (!commands.IsEmpty())
                            {
                                //get the command and parse it to definitly end with \r\n
                                string commandToSend = Regex.Replace(commands.RemoveElement(), @"\n|\r", "") + "\r\n";
                                System.Diagnostics.Debug.WriteLine("sending = " + commandToSend);
                                //send the given command, converting it to a char array due to how BinaryWriter.Write works
                                writer.Write(commandToSend.ToCharArray());
                                
                            }
                        }
                    }
                }
            }
        }

        //make the class a singleton since we only need one server
        #region Singleton
        private static CommandClient _Instance = null;
        public static CommandClient Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CommandClient();
                }
                return _Instance;
            }
        }
        #endregion

        private Thread serverThread;
        private DataQueue commands;
        /*set up the command client*/
        private CommandClient()
        {
            serverThread = null;
            commands = new DataQueue();
            //make it so that the client attempts to connect only when the flight simulator connects to us, to make sure the server exists
            InfoServer.Instance.AddToConnectorEvent(this.Connect);
        }

        /*connects the client to the flight simulator*/
        public void Connect(string ip, int port)
        {
            //if there is already a thread, make sure to interrupt it
            if (serverThread != null) serverThread.Interrupt();
            //set up the wrapper to the client and start it
            PrivateClient server = new PrivateClient(ip, port, commands);
            serverThread = new Thread(new ThreadStart(server.SendCommands))
            {
                IsBackground = true
            };
            System.Diagnostics.Debug.WriteLine("Client connection astablished on ip = {0} and port = {1}", ip, port);
            serverThread.Start();
        }
        /*send a command for the client to send to the flight simulator*/
        public void SendData(string command)
        {
            //if the client is actually connected add the command to the queue
            if (serverThread != null) commands.AddElement(command);
        }
    }

    /*the server to get info from the flight simulator*/
    public class InfoServer : BaseNotify
    {
        //delegate for the commands client to use
        public delegate void connectDelegate(string ip, int port);

        /*wrapper class for TCPListener for ease of use in thread*/
        class PrivateServer
        {
            //event to execute for the connection of the command client
            public event connectDelegate connectEvent;
            private TcpListener server;
            private DataQueue commands;
            /*set up the server*/
            public PrivateServer(DataQueue commandsQue)
            {
                commands = commandsQue;
            }
            //get all of the commands and push them to the queue
            public void GetCommands(string ip, int port, int commandPort)
            {
                //create the TCPListener and start it
                server = new TcpListener(IPAddress.Parse(ip), port);
                System.Diagnostics.Debug.WriteLine("Server connecting on ip = {0} and port = {1}", ip, port);
                server.Start();
                //run until you get a connection
                while (true)
                {
                    //accept the connection
                    TcpClient client = server.AcceptTcpClient();
                    System.Diagnostics.Debug.WriteLine("Server accepted client");
                    //invoke the event for the command client to connect to the flight simulator
                    connectEvent?.Invoke(ip, commandPort);
                    //get the network stream from the client
                    NetworkStream ns = client.GetStream();
                    //run while there is a connection
                    while (client.Connected)
                    {
                        //read information
                        StreamReader reader = new StreamReader(ns);
                        string command = reader.ReadLine();
                        //push the information to the queue
                        commands.AddElement(command);
                    }
                }
            }
        }

        //make the class a singleton since we only need one server
        #region Singleton
        private static InfoServer _Instance = null;
        public static InfoServer Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new InfoServer();
                }
                return _Instance;
            }
        }
        #endregion

        private Thread serverThread;
        private PrivateServer server;
        private DataQueue commands;
        /*set up the server*/
        private InfoServer()
        {
            serverThread = null;
            commands = new DataQueue();
            //subscribe to the queue event with the class notification event
            commands.PropertyChanged += Vm_PropertyChanged;
            server = new PrivateServer(commands);
        }

        /*open the server for connections*/
        public void Connect(string ip, int port, int commandport)
        {
            if (serverThread != null) serverThread.Interrupt();
            serverThread = new Thread(()=>server.GetCommands(ip, port, commandport))
            {
                IsBackground = true
            };
            serverThread.Start();
        }

        /*notified whoever is subscribed to the class of the changes to the queue*/
        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //notify the subscribed delegates of the change to the queue
            NotifyPropertyChanged(e.PropertyName);
        }

        /*get a data point from the queue*/
        public string[] getData()
        {
            //get the raw data
            string dataBeforeConversion = commands.RemoveElement();
            //split the data up and return it
            string[] splitData = dataBeforeConversion.Split(',');
            //System.Diagnostics.Debug.WriteLine("Lon = " + splitData[0] + " Lat = " + splitData[1]);
            return splitData;
        }

        /*add the delegate of command client connection to the private class event*/
        public void AddToConnectorEvent(connectDelegate connector)
        {
            //add the delegate to the ConnectEvent in the TCPListener wrapper
            this.server.connectEvent += connector;
        }
    }

    /*thread safe queue to store all of the data (sent / received) from the server/client*/
    class DataQueue : BaseNotify
    {
        //used for the lock statement
        private readonly Object Lock = new Object();
        //the actual data queue to lock
        private Queue<String> Data = new Queue<string>();

        /*checks if the queue is empty*/
        public bool IsEmpty()
        {
            lock (Lock)
            {
                return Data.Count == 0;
            }
        }

        /*adds a given element to the queue*/
        public void AddElement(string add)
        {
            lock (Lock)
            {
                Data.Enqueue(add);
            }
            NotifyPropertyChanged("Added");
        }

        /*removes a given element from the queue*/
        public string RemoveElement()
        {
            string ret;
            lock (Lock)
            {
                ret = Data.Dequeue();
            }
            NotifyPropertyChanged("Removed");
            return ret;
        }
    }
}