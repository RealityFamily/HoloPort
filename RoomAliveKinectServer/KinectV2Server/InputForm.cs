using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KinectV2Server
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }

        //<summary>
        //in this region will be written module? which will create TCP connection with UnityClient
        private IPAddress UnityClientIP;

        private void BroadcastMessage()
        {
            UdpClient Client = new UdpClient();
            byte[] RequestData = Encoding.ASCII.GetBytes("");
            IPEndPoint ServerEp = new IPEndPoint(IPAddress.Any, 0);

            Client.EnableBroadcast = true;
            Client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

            byte[] ServerResponseData = Client.Receive(ref ServerEp);
            string ServerResponse = Encoding.ASCII.GetString(ServerResponseData);
            if (ServerResponse == "I'M HOLOLENS")
            {
                UnityClientIP = ServerEp.Address;
                Client.Close();
                TCPConnection();
            }
        }

        private void TCPConnection()
        {
            bool isAvailable = false;
            while (!isAvailable) {
                TcpConnectionInformation[] tcpConnInfoArray = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();
                foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
                {
                    if (tcpi.LocalEndPoint.Port == 7777)
                    {
                        isAvailable = true;
                        break;
                    }
                }
            }

            TcpClient client = new TcpClient();
            client.Connect(UnityClientIP, 7777);

            byte[] data = new byte[256];
            StringBuilder response = new StringBuilder();
            NetworkStream stream = client.GetStream();

            int bytes = stream.Read(data, 0, data.Length);
            response.Append(Encoding.UTF8.GetString(data, 0, bytes));

            if (response.ToString() == "Подключен")
            {
                    MainForm.instance.ConnectStatus = true;
            }

            new Thread(() =>
            {
                while (true)
                {
                    if (!client.Connected)
                    {
                        MainForm.instance.ConnectStatus = false;
                        break;
                    }
                }
            }).Start();

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox.Text.Length == 4 && Int32.TryParse(textBox.Text, out int number)) {
                BroadcastMessage();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
