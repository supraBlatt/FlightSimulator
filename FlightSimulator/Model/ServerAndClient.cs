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
    class CommandClient
    {
        class PrivateClient
        {
            private TcpClient server;
            private DataQueue commands;
            public PrivateClient(string ip, int port, DataQueue commandsQue)
            {
                commands = commandsQue;
                this.server = new TcpClient();
                System.Diagnostics.Debug.WriteLine("Client connecting on ip = {0} and port = {1}", ip, port);
                server.Connect(ip, port);
            }
            public void SendCommands()
            {

                using (NetworkStream ns = server.GetStream())
                using (BinaryWriter writer = new BinaryWriter(ns))
                {
                    while (true)
                    {
                        while (server.Connected)
                        {
                            while (!commands.IsEmpty())
                            {
                                string commandToSend = Regex.Replace(commands.RemoveElement(), @"\n|\r", "") + "\r\n";
                                System.Diagnostics.Debug.WriteLine("sending = " + commandToSend);
                                writer.Write(commandToSend.ToCharArray());
                                
                            }
                        }
                    }
                }
            }
        }

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
        private CommandClient()
        {
            serverThread = null;
            commands = new DataQueue();
            InfoServer.Instance.AddToConnectorEvent(this.Connect);
        }
        public void Connect(string ip, int port)
        {
            if (serverThread != null) serverThread.Interrupt();
            PrivateClient server = new PrivateClient(ip, port, commands);
            serverThread = new Thread(new ThreadStart(server.SendCommands))
            {
                IsBackground = true
            };
            System.Diagnostics.Debug.WriteLine("Client connection astablished on ip = {0} and port = {1}", ip, port);
            serverThread.Start();
        }
        public void SendData(string command)
        {
            if (serverThread != null) commands.AddElement(command);
        }
    }

    public class InfoServer : BaseNotify
    {
        public delegate void connectDelegate(string ip, int port);
        class PrivateServer
        {
            public event connectDelegate connectEvent;
            private TcpListener server;
            private DataQueue commands;
            public PrivateServer(DataQueue commandsQue)
            {
                commands = commandsQue;
            }
            public void GetCommands(string ip, int port, int commandPort)
            {
                server = new TcpListener(IPAddress.Parse(ip), port);
                System.Diagnostics.Debug.WriteLine("Server connecting on ip = {0} and port = {1}", ip, port);
                server.Start();
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    System.Diagnostics.Debug.WriteLine("Server accepted client");
                    connectEvent?.Invoke(ip, commandPort);
                    NetworkStream ns = client.GetStream();
                    while (client.Connected)
                    {
                        StreamReader reader = new StreamReader(ns);
                        string command = reader.ReadLine();
                        commands.AddElement(command);
                    }
                }
            }
        }

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


        // event for registering to dataqueue changes


        private Thread serverThread;
        private PrivateServer server;
        private DataQueue commands;
        private InfoServer()
        {
            serverThread = null;
            commands = new DataQueue();
            commands.PropertyChanged += Vm_PropertyChanged;
            server = new PrivateServer(commands);
        }
        public void Connect(string ip, int port, int commandport)
        {
            if (serverThread != null) serverThread.Interrupt();
            serverThread = new Thread(()=>server.GetCommands(ip, port, commandport))
            {
                IsBackground = true
            };
            System.Diagnostics.Debug.WriteLine("Server connection astablished on ip =" + ip + "and port =" + port);
            serverThread.Start();
        }

        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        public string[] getData()
        {
            string dataBeforeConversion = commands.RemoveElement();
            string[] splitData = dataBeforeConversion.Split(',');
            //System.Diagnostics.Debug.WriteLine("Lon = " + splitData[0] + " Lat = " + splitData[1]);
            return splitData;
        }

        public void AddToConnectorEvent(connectDelegate connector)
        {
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