using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class MyLanBroadcast : MonoBehaviour
{
    public bool alive = true;
    public string message;

    private void Start()
    {
        new Thread(() =>
        {
            UdpClient Server = new UdpClient(8888);
            byte[] ResponseData = Encoding.ASCII.GetBytes(message);

            IPEndPoint ClientEp = new IPEndPoint(IPAddress.Broadcast, 0);
            Server.Send(ResponseData, ResponseData.Length, ClientEp);

            Thread.Sleep(60000);

            //Server.Close();
        }).Start();
    }
}
