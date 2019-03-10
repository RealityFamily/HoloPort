//#define VERBOSE

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoomAliveToolkit
{
    /// <summary>
    /// The object that encapsulates the functionality for reading client data asynchronously 
    /// </summary>
    public class ClientState
    {
        private static int counter = 1;

        public ClientState()
        {
            ID = counter;
            counter++;
            receivingMessageBuffer = new byte[MessageSize];
        }

        public UdpClient client = new UdpClient();
        public IPEndPoint remoteIP = null;

        public int ID = 0; //counter of clients (each is unique)
        public int MessageSize = 10000000;
        public bool readyToSend = true;

        public bool active = false;

        // Receiving buffers:
        public byte[] receivingMessageBuffer;
        public const int maximumMessageQueueLength = 50;
        public Queue<byte[]> sendingMessageQueue = new Queue<byte[]>(maximumMessageQueueLength);

        public int BytesReceived = 0;
        public int PacketCounter = 0;

        public void ResetCounters()
        {
            BytesReceived = 0;
            PacketCounter = 0;
        }
    }

    public interface INetworkCallback
    {
        void OnPrint(string message);
        void OnError(string message);
    }

    public class ReceivedMessageEventArgs : EventArgs
    {
        public ReceivedMessageEventArgs(byte[] _data)
        {
            this.data = _data;

        }

        public byte[] data;
    }

    /// <summary>
    /// A helper class that handles TCP based network streaming (both server and client side). 
    /// </summary>
    public class TCPNetworkStreamer
    {
        private int server_port = 11000;
        private const int bufferSize = 10240000;
        public int tmpBufferSize = 640000;
        public int Port { get { return server_port; } }

        public INetworkCallback callback = null;

        //public bool CompressStream = false;
        private bool isServer = false;

        public bool IsServer { get { return isServer; } }

        public bool ReadyToSend
        {
            get
            {
                bool ready = (clients.Count > 0) ? true : false;
                foreach (ClientState client in clients)
                {
                    if (!client.readyToSend) ready = false;
                }
                return ready;
            }
        }

        private TcpListener server;
        private List<ClientState> clients = new List<ClientState>();

        public bool runningServer = false;

        public bool Connected { get { return clients.Count > 0; } }



        // Sending queue:
        public delegate void ReceivedMessageEventHandler(object sender, ReceivedMessageEventArgs e);
        public ReceivedMessageEventHandler ReceivedMessage;

        public string name = "";



        public void Close()
        {
            runningServer = false;
            foreach (ClientState cs in clients)
            {
                cs.active = false;
            }
        }

        protected void Print(string message)
        {
            if (callback != null)
            {
                callback.OnPrint(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        protected void PrintError(string message)
        {
            if (callback != null)
            {
                callback.OnError(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }



        #region CLIENT SPECIFIC

        //public void ConnectToServerUDP(string host, int port)
        //{
        //    Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(host), port);
        //    byte[] data = Encoding.UTF8.GetBytes("Hello");
        //    udpSocket.SendTo(data, SocketFlags.None, endPoint);
        //}


        public void ConnectToSever(string host, int port)
        {
            if (!IsServer)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(host), port);
                UdpClient client = new UdpClient();
                client.Connect(host, port);
                byte[] data = Encoding.UTF8.GetBytes("Hello");

                try
                {
                    client.Send(data, data.Length);
                    Print("Establishing Connection to " + host + " - " + endPoint + ":" + port);
                    ClientHandler(endPoint);
                }
                catch (Exception e)
                {
                    PrintError("Connecting to server failed: " + e.Message);
                }
            }
            else
            {
                PrintError("ConnectToServer Error! This TCPNetworkStreamer is configured to run as a server!");
            }
        }

        private void ClientHandler(IPEndPoint remoteIP)
        {
            try
            {
                ClientState clientState = new ClientState();
                clientState.remoteIP = remoteIP;

                clients.Add(clientState);

                Print("Connection established!");

                new Thread(RunClient).Start(clientState);
            }
            catch (Exception e)
            {
                PrintError("Connecting to server failed: " + e.Message);
            }
        }

        /// <summary>
        /// ClientThread (same for server and client)
        /// </summary>
        /// <param name="state"></param>
        private void RunClient(object clientState)
        {
            var state = clientState as ClientState;
            state.active = true;

            byte[] bufferTmp = new byte[tmpBufferSize];

            // Loop to receive all the data sent by the client.
            try
            {
                while (state.active)
                {
                    //both reading and writing to a network stream can happen concurrently, so we will do this through asynchronous calls
                    //send to them whatever you need to send
                    if (state.readyToSend && state.sendingMessageQueue.Count > 0)
                    {
                        byte[] sendBuffer;
                        lock (state.sendingMessageQueue)
                        {
                            sendBuffer = state.sendingMessageQueue.Dequeue();

                            state.readyToSend = false;
                        }
                        // Send a message:
                        state.client.Send(sendBuffer, sendBuffer.Length, state.remoteIP);
                    }


                    state.client.Connect(state.remoteIP);
                    IPEndPoint IPremote = null;
                    while (state.client.Available > 0)
                    {
                        bufferTmp = state.client.Receive(ref IPremote);
                        ProcessReceivedBuffer(bufferTmp, bufferTmp.Length, state);
                    }



                }
            }
            catch (Exception e)
            {
                PrintError("Client " + state.ID + " Exception: Unable to write to socket (beginAsync): " + e);
            }

            Print("Client " + state.ID + " disconnected.");

            //should probably check if this one is in the list
            clients.Remove(state);
        }

        //there is no guarrantee that the message will be transferred in one packet, so we need to assemble a buffer and keep track on where it is.
        private void ProcessReceivedBuffer(byte[] buffer, int bytesReceived, ClientState clientState)
        {

#if VERBOSE
            Console.WriteLine("Processing! This message: " + bytesReceived + " Received so far: " + clientState.BytesReceived);
#endif
            if (clientState.BytesReceived == 0 && bytesReceived > 3) //new message
            {
                int newMessageSize = BitConverter.ToInt32(buffer, 0) + sizeof(int); //prefix is one int
                if (newMessageSize != clientState.MessageSize)
                {
#if VERBOSE
                    Console.WriteLine("Resizing receiving buffer to: " + newMessageSize + " Old size: " + clientState.MessageSize + " Bytes received so far: " + clientState.BytesReceived);
#endif
                    clientState.MessageSize = newMessageSize;
                    byte[] newBuffer = new byte[newMessageSize];
                    clientState.receivingMessageBuffer = newBuffer;
                }
            }

            int availableLength = clientState.MessageSize - clientState.BytesReceived;
            int copyLen = Math.Min(availableLength, bytesReceived);
            clientState.PacketCounter++;
            Array.Copy(buffer, 0, clientState.receivingMessageBuffer, clientState.BytesReceived, copyLen);

            clientState.BytesReceived += copyLen;

            if (clientState.BytesReceived == clientState.MessageSize)
            {
#if VERBOSE
                Console.WriteLine("Received Message of {0} bytes from client #{1} in {2} messages", clientState.BytesReceived, clientState.ID, clientState.PacketCounter);
#endif
                clientState.ResetCounters();
                //let folks know that they have the full message
                if (ReceivedMessage != null)
                {
                    ReceivedMessage(clientState, new ReceivedMessageEventArgs(UnwrapMessage(clientState.receivingMessageBuffer)));
                }
            }

            if (copyLen != bytesReceived) // process the remainder of the message
            {
#if VERBOSE
                Console.WriteLine("Packet was longer than the expected. Received {0}  Copied {1}", bytesReceived, copyLen);
#endif
                byte[] newBuffer = new byte[bytesReceived - copyLen];

                Array.Copy(buffer, copyLen, newBuffer, 0, bytesReceived - copyLen);

                ProcessReceivedBuffer(newBuffer, bytesReceived - copyLen, clientState);
            }
        }
        #endregion CLIENT SPECIFIC

        public void CloseClient(int clientID)
        {
            foreach (ClientState state in clients)
            {
                if (state.ID == clientID)
                {
                    Print("Closing client " + clientID + "...");
                    state.active = false;

                }
            }
        }
        public void CloseAllClients()
        {
            foreach (ClientState state in clients)
            {
                Print(this.name + " Closing client " + state.ID + "...");
                state.active = false;
            }
            clients.Clear();
        }

        public void SendMessageToAllClients(byte[] data)
        {
            byte[] dataWrapped = WrapMessage(data);
            foreach (ClientState state in clients)
            {
                lock (state.sendingMessageQueue)
                {
                    if (state.sendingMessageQueue.Count < ClientState.maximumMessageQueueLength)
                    {
                        state.sendingMessageQueue.Enqueue(dataWrapped);
                    }
                }
            }
        }

        public void SendMessageToClient(byte[] data, int clientID)
        {
            byte[] dataWrapped = WrapMessage(data);
            foreach (ClientState state in clients)
            {
                if (state.ID == clientID)
                {
                    lock (state.sendingMessageQueue)
                    {
                        if (state.sendingMessageQueue.Count < ClientState.maximumMessageQueueLength)
                        {
                            state.sendingMessageQueue.Enqueue(dataWrapped);
                        }
                    }
                }
            }
        }

        public int GetClientCount() { return clients.Count; }

        /// <summary>
        /// Wraps a message. The wrapped message is ready to send to a stream.
        /// </summary>
        /// <remarks>
        /// <para>Generates a length prefix for the message and returns the combined length prefix and message.</para>
        /// </remarks>
        /// <param name="message">The message to send.</param>
        public byte[] WrapMessage(byte[] message)
        {
            //if (CompressStream)
            //{
            //    MemoryStream stream1 = new MemoryStream();
            //    System.IO.Compression.DeflateStream defStream = new System.IO.Compression.DeflateStream(stream1, System.IO.Compression.CompressionMode.Compress);

            //    defStream.Write(message, 0, message.Length);
            //    message = stream1.ToArray();
            //    stream1.Close();
            //}

            // Get the length prefix for the message
            byte[] lengthPrefix = BitConverter.GetBytes(message.Length);
            // Concatenate the length prefix and the message
            byte[] ret = new byte[lengthPrefix.Length + message.Length];
            lengthPrefix.CopyTo(ret, 0);
            message.CopyTo(ret, lengthPrefix.Length);
            return ret;
        }

        public byte[] UnwrapMessage(byte[] message)
        {
            byte[] data = new byte[message.Length - sizeof(int)];

            //skip the prefix
            Array.Copy(message, sizeof(int), data, 0, data.Length);

            //if (CompressStream)
            //{
            //    MemoryStream original = new MemoryStream(data);
            //    System.IO.Compression.DeflateStream defStream = new System.IO.Compression.DeflateStream(original, System.IO.Compression.CompressionMode.Decompress);
            //    MemoryStream memStream = new MemoryStream();
            //    defStream.CopyTo(memStream);// Read(tmp, 0, tmp.Length);
            //    original.Close();
            //    data = memStream.ToArray();
            //    memStream.Close();
            //}
            return data;
        }
    }
}
