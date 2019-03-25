using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class UDPsocket
{
    public int SocketID { get; private set; }
    public int SocketPort { get; private set; }
    public Dictionary<string, int> Chanels { get { return _chanels; } }

    private Dictionary<string,int> _chanels = new Dictionary<string, int>();

    private UDPsocket() { }
    public UDPsocket(int port, ICustomUDP socketConfig = null)
    {
        if (!NetworkTransport.IsStarted) 
            NetworkTransport.Init();
        if (!Application.runInBackground)
            Application.runInBackground = true;

        if (socketConfig == null)
            socketConfig = new DefaultUDP();

        ConnectionConfig connectionConfig = new ConnectionConfig();
        foreach(var chanel in socketConfig.Chanels)
        {
            int chanelNum = connectionConfig.AddChannel(chanel.Value);
            _chanels.Add(chanel.Key, chanelNum);
        }

        HostTopology socketTopology = new HostTopology(connectionConfig, socketConfig.MaxConnections);
        SocketID = NetworkTransport.AddHost(socketTopology, port);
        SocketPort = port;
    }
}

public struct UdpSocketInfo
{
    public int SocketID;
    public Dictionary<string, int> Chanels;
}
