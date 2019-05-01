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
                byte[] toSend = new byte[2048];
                NetworkStream ns = server.GetStream();
                while (true)
                {
                    while (server.Connected)
                    {
                        while (!commands.IsEmpty())
                        {
                            string commandToSend = commands.RemoveElement();
                            System.Diagnostics.Debug.WriteLine("Trying to send = " + commandToSend);
                            toSend = Encoding.ASCII.GetBytes(commandToSend);
                            ns.Write(toSend, 0, toSend.Length);
                            System.Diagnostics.Debug.WriteLine("Sent = {0}", toSend);
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
        class PrivateServer
        {
            private TcpListener server;
            private DataQueue commands;
            public PrivateServer(string ip, int port, DataQueue commandsQue)
            {
                commands = commandsQue;
                System.Diagnostics.Debug.WriteLine("Server connecting on ip = {0} and port = {1}", ip, port);
                this.server = new TcpListener(IPAddress.Parse(ip), port);
            }
            public void GetCommands()
            {
                byte[] toGet = new byte[4096];
                server.Start();
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    System.Diagnostics.Debug.WriteLine("Server accepted client");
                    NetworkStream ns = client.GetStream();
                    while (client.Connected)
                    {
                        ns.Read(toGet, 0, toGet.Length);
                        string command = Encoding.Default.GetString(toGet).Trim();
                        commands.AddElement(command);

                        System.Diagnostics.Debug.WriteLine("Server adding to queue = {0}", command);
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
        private DataQueue commands;
        private InfoServer()
        {
            serverThread = null;
            commands = new DataQueue();
            commands.PropertyChanged += Vm_PropertyChanged;
        }
        public void Connect(string ip, int port)
        {
            if (serverThread != null) serverThread.Interrupt();
            PrivateServer server = new PrivateServer(ip, port, commands);
            serverThread = new Thread(new ThreadStart(server.GetCommands))
            {
                IsBackground = true
            };
            System.Diagnostics.Debug.WriteLine("Server connection astablished on ip = {0} and port = {1}", ip, port);
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
            return splitData;
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