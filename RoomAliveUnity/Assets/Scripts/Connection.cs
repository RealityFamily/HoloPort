using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;


public class Connection : MonoBehaviour
{
    private string ConnPass = "";
    bool startserver = false;
    public bool StartServer {set {
            startserver = value;
    }}

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            ConnPass += Random.Range(0, 9);
        }

        print(ConnPass);

        new Thread(() =>
        {

            TcpListener server = new TcpListener(GetLocalIPAddress(), 7777);
            print(GetLocalIPAddress().ToString());
            server.Start();
            try
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                byte[] data = new byte[256];
                StringBuilder response = new StringBuilder();

                int bytes = stream.Read(data, 0, data.Length);
                response.Append(Encoding.UTF8.GetString(data, 0, bytes));

                if (response.ToString() == ConnPass)
                {
                    data = Encoding.Unicode.GetBytes("Подключен");
                    stream.Write(data, 0, data.Length);

                    GameObject parent = transform.parent.gameObject;
                    parent.transform.Find("MyRoom").gameObject.SetActive(true);

                    MyLanBroadcast lanBroadcast = gameObject.GetComponent<MyLanBroadcast>();
                    lanBroadcast.alive = false;
                }
                else
                {
                    data = Encoding.Unicode.GetBytes("Неверный пароль");
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                server.Stop();
                print(GetLocalIPAddress().ToString());
                print("Kinect не запущен");
                print(ex.Message);
            }
        }).Start();
    }

    public static IPAddress GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip;
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
