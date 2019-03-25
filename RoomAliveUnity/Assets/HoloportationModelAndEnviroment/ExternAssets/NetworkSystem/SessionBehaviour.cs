using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SessionBehaviour
{
    public event Action<SessionBehaviour> OnSessionStopedByUser = (b) => { };

    public int SessionID { get; private set; }
    public int MaxMessageSize { get; private set; }
    public ICustomUDP UdpSocketConfig { get; private set; }
    protected UdpSocketInfo _socketInfo;
    private bool _activated = false;

    // data for restart;
    private int _lastPort;
    private int _lastID;

    public void Start(int port, int id, int maxMessageSize = 256, ICustomUDP socketConfig = null)
    {
        if (SessionsManager.Instance == null)
            throw new NullReferenceException("Did not find alive SessionsManager on scene. Can not to activate session.");

        SessionID = _lastID = id;
        _lastPort = port;
        MaxMessageSize = maxMessageSize;
        UdpSocketConfig = socketConfig;

        SessionsManager.Instance.ActivateSession(this, port);
    }
    public void Restart()
    {
        if (_activated)
            return;

        Start(_lastPort, _lastID, MaxMessageSize, UdpSocketConfig);
    }
    public void Stop()
    {
        _activated = false;
        OnSessionStopedByUser(this);
    }
    public void SetSocketInfo(UdpSocketInfo info)
    {
        _socketInfo = info;
    }

    public int ConnectTo(string ip, int port)
    {
        byte error;
        int conID = NetworkTransport.Connect(_socketInfo.SocketID, ip, port, 0, out error);

        return conID;
    }

    public void Send(byte[] data, int connectionID, string chanelName)
    {
        if (!_activated || !NetworkTransport.IsStarted)
            return;

        byte[] buffer = new byte[data.Length + 4];
        byte[] id = BitConverter.GetBytes(SessionID);
        System.Buffer.BlockCopy(id, 0, buffer, 0, 4);
        System.Buffer.BlockCopy(data, 0, buffer, 4, data.Length);

        int chanelID;
        if (_socketInfo.Chanels.TryGetValue(chanelName, out chanelID))
        {
            byte error;
            NetworkTransport.Send(_socketInfo.SocketID, connectionID, chanelID, buffer, buffer.Length, out error);  // handle error

        }
        else
        {
            throw new Exception(@"The socket has not chanel whis name: " + chanelName);
        }

    }

    public void Send(byte[] data, int connectionID, int chanelID)
    {
        if (!_activated || !NetworkTransport.IsStarted)
            return;

        byte[] buffer = new byte[data.Length + 4];
        byte[] id = BitConverter.GetBytes(SessionID);
        System.Buffer.BlockCopy(id, 0, buffer, 0, 4);
        System.Buffer.BlockCopy(data, 0, buffer, 4, data.Length);


        byte error;
        NetworkTransport.Send(_socketInfo.SocketID, connectionID, chanelID, buffer, buffer.Length, out error);  // handle error
    }

    // UDP callbacks methods
    public virtual void OnConnect(string connectionIP, int connectionID) { }
    public virtual void OnDisconnect(int connectionID) { }
    public virtual void OnDataIncoming(byte[] data, int connectionID, int chanelID) { }
    public virtual void OnBroadcastDataIncoming(byte[] broadcastData) { }

    // Session Lifetime methods
    public virtual void OnSessionStart() { _activated = true; }
    public virtual void OnSessionEnd(byte error) { _activated = false; }
    public virtual void OnSessionUpdate() { }
}
