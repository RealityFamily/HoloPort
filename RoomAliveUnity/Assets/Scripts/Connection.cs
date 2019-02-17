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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            ConnPass += Random.Range(0, 9);
        }

        Thread thread = new Thread(new ThreadStart(AnswerWaiting));
    }

    private void AnswerWaiting()
    {
        IPEndPoint ippoint = new IPEndPoint(IPAddress.Any, 65000);
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Bind(ippoint);
            socket.Listen(1);

            print("tcpServer запущен");

            bool answer = false;
            while (!answer)
            {
                Socket handler = socket.Accept();
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                byte[] data = new byte[256];

                bytes = handler.Receive(data);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));

                if (builder.ToString() == ConnPass)
                {
                    data = Encoding.Unicode.GetBytes("Подключен");
                    handler.Send(data);

                    GameObject parent = transform.parent.gameObject;
                    parent.transform.Find("MyRoom").gameObject.SetActive(true);

                    MyLanBroadcast lanBroadcast = gameObject.GetComponent<MyLanBroadcast>();
                    lanBroadcast.alive = false;
                }
                else
                {
                    data = Encoding.Unicode.GetBytes("Неверный пароль");
                    handler.Send(data);
                }
            }
        }
        catch (Exception ex)
        {
            print(ippoint.Address.ToString());
            print("Kinect не запущен");
            print(ex.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
