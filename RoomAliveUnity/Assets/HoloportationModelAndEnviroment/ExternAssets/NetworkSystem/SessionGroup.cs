using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionGroup
{
    public UDPsocket Socket { get; private set; }
    private List<SessionBehaviour> _groupedSession = new List<SessionBehaviour>();

    private event Action<SessionGroup> OnGroupClosing;

    // .ctor
    private SessionGroup() { }
    public SessionGroup(UDPsocket socket)
    {
        Socket = socket;
    }

    // api
    public void AddSession(SessionBehaviour behaviour, Action<SessionGroup> groupClosingHandler)
    {
        _groupedSession.Add(behaviour);
        if (OnGroupClosing == null)
            OnGroupClosing += groupClosingHandler;
        behaviour.SetSocketInfo(new UdpSocketInfo() { SocketID = Socket.SocketID, Chanels = Socket.Chanels });
        behaviour.OnSessionStopedByUser += SessionBehaviour_OnSessionStopedByUser;
        behaviour.OnSessionStart();
    }

    // recall
    public void OnConnect(string connectionIP, int connectionID)
    {
        foreach (var session in _groupedSession)
        {
            session.OnConnect(connectionIP, connectionID);
        }
    }
    public void OnDisconnect(int connectionID)
    {
        foreach (var session in _groupedSession)
        {
            session.OnDisconnect(connectionID);
        }
    }
    public void OnDataIncoming(byte[] data, int connectionID, int chanelID)
    {
        byte[] forSessionData;
        int id = GetIdAndRewriteBuffer(data, out forSessionData);
        GetSessionByID(id).OnDataIncoming(forSessionData, connectionID, chanelID);  // may be NUll reference
    }
    public void OnBroadcastDataIncoming(byte[] broadcastData)
    {
        foreach (var session in _groupedSession)
        {
            session.OnBroadcastDataIncoming(broadcastData);
        }
    }

    public void OnSessionEnd(byte error)
    {
        foreach (var session in _groupedSession)
        {
            session.OnSessionEnd(error);
        }
    }
    public void OnSessionUpdate()
    {
        foreach (var session in _groupedSession)
        {
            session.OnSessionUpdate();
        }
    }

    // Internal methods
    private SessionBehaviour GetSessionByID(int id)
    {
        foreach (var session in _groupedSession)
        {
            if (session.SessionID == id)
                return session;
        }

        return null;
    }
    private int GetIdAndRewriteBuffer(byte[] oldbuffer, out byte[] newBuffer)
    {
        byte[] idAsBytes = new byte[4];
        System.Buffer.BlockCopy(oldbuffer, 0, idAsBytes, 0, 4);
        int id = BitConverter.ToInt32(idAsBytes, 0);
        newBuffer = new byte[oldbuffer.Length - 4];
        System.Buffer.BlockCopy(oldbuffer, 4, newBuffer, 0, newBuffer.Length);
        return id;
    }

    private void SessionBehaviour_OnSessionStopedByUser(SessionBehaviour behaviour)
    {
        _groupedSession.Remove(behaviour);
        if(_groupedSession.Count < 1)
        {
            if (OnGroupClosing != null)
                OnGroupClosing(this);
        }
    }
}
