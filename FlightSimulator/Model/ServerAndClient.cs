using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows;


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
                server.Connect(ip, port);
            }
            public void sendCommands()
            {
                byte[] toSend = new byte[2048];
                NetworkStream ns = server.GetStream();
                while (true)
                {
                    while (server.Connected)
                    {
                        while (!commands.isEmpty())
                        {
                            toSend = Encoding.Default.GetBytes(commands.RemoveMember());
                            ns.Write(toSend, 0, toSend.Length);
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
        public void connect(string ip, int port)
        {
            if (serverThread != null) serverThread.Interrupt();
            PrivateClient server = new PrivateClient(ip, port, commands);
            serverThread = new Thread(new ThreadStart(server.sendCommands));
            serverThread.IsBackground = true;
            serverThread.Start();
        }
        public void sendData(string command)
        {
            if(serverThread != null) commands.AddMember(command);
        }
    }

    public class InfoServer
    {
        class PrivateServer
        {
            private TcpListener server;
            private DataQueue commands;
            public PrivateServer(string ip, int port, DataQueue commandsQue)
            {
                commands = commandsQue;
                this.server = new TcpListener(new IPAddress(Encoding.Default.GetBytes(ip)), port);
            }
            public void getCommands()
            {
                byte[] toGet = new byte[2048];
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream ns = client.GetStream();
                    while (client.Connected)
                    {
                        ns.Read(toGet, 0, toGet.Length);
                        commands.AddMember(Encoding.Default.GetString(toGet).Trim());
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

        private Thread serverThread;
        private DataQueue commands;
        private InfoServer()
        {
            serverThread = null;
            commands = new DataQueue();
        }
        public void connect(string ip, int port)
        {
            if (serverThread != null) serverThread.Interrupt();
            PrivateServer server = new PrivateServer(ip, port, commands);
            serverThread = new Thread(new ThreadStart(server.getCommands));
            serverThread.IsBackground = true;
            serverThread.Start();
        }
        public double[] getData()
        {
            if (commands.isEmpty()) return null;
            string toSplit = commands.RemoveMember();
            string[] splitData = toSplit.Split(',');
            double[] ret = { Double.Parse(splitData[0]), Double.Parse(splitData[1]) };
            return ret;
        }
    }

    class DataQueue
    {
        private readonly Object Lock = new Object();
        private Queue<String> Data = new Queue<string>();

        public bool isEmpty()
        {
            lock (Lock)
            {
                return Data.Count == 0;
            }
        }

        public void AddMember(string add)
        {
            lock (Lock)
            {
                Data.Enqueue(add);
            }
        }
        public string RemoveMember()
        {
            lock (Lock)
            {
                return Data.Dequeue();
            }
        }
    }
}
