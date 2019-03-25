using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

public class SessionsManager : MonoBehaviour
{
    public static SessionsManager Instance;

    private int _messageBufferSize = 256;

    // <socketID, group>
    private static Dictionary<int, SessionGroup> _activeGroups = new Dictionary<int, SessionGroup>();

    public void ActivateSession(SessionBehaviour sessionBehaviour, int port)
    {
        // Если на порту уже есть группа сессий, добавляем наше поведение к группе
        foreach(var group in _activeGroups.Values.ToList())
        {
            if(group.Socket.SocketPort == port)
            {
                group.AddSession(sessionBehaviour, SessionGroup_OnGroupClosing);
                return;
            }
        }

        // Если нет, то создаём новую группу и сокет на указанном порту
        // и кладём туда нашу сессию
        UDPsocket socket = new UDPsocket(port, sessionBehaviour.UdpSocketConfig);
        var newGroup = new SessionGroup(socket);
        _activeGroups.Add(socket.SocketID, newGroup);
        newGroup.AddSession(sessionBehaviour, SessionGroup_OnGroupClosing);
    }

    private void SessionGroup_OnGroupClosing(SessionGroup closingGroup)
    {
        _activeGroups.Remove(closingGroup.Socket.SocketID);
        if(_activeGroups.Count < 1)
        {
            NetworkTransport.Shutdown();
        }
    }

    private void OnReceiveGlobalError()
    {
        _activeGroups.Clear();
        NetworkTransport.Shutdown();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (!NetworkTransport.IsStarted || _activeGroups.Count < 1)
            return;

        int socketID;
        int connectionID;
        int chanelID;
        byte[] buffer = new byte[_messageBufferSize];
        int receivedSize;
        byte error;
        NetworkEventType eventType = NetworkTransport.Receive(out socketID, out connectionID, out chanelID, buffer, _messageBufferSize, out receivedSize, out error);

        if (error != 0)
        {
            var groupList = _activeGroups.Values.ToList();
            OnReceiveGlobalError();
            foreach (var group in groupList)
            {
                group.OnSessionEnd(error);
            }
            return;
        }

        SessionGroup currentMessageHandler;
        if (_activeGroups.TryGetValue(socketID, out currentMessageHandler))
        {
            switch (eventType)
            {
                case NetworkEventType.DataEvent:
                    currentMessageHandler.OnDataIncoming(TakeRealData(buffer, receivedSize), connectionID, chanelID);
                    break;
                case NetworkEventType.BroadcastEvent:
                    NetworkTransport.GetBroadcastConnectionMessage(socketID, buffer, buffer.Length, out receivedSize, out error);  // ToDo: handle error
                    currentMessageHandler.OnBroadcastDataIncoming(TakeRealData(buffer, receivedSize));
                    break;
                case NetworkEventType.ConnectEvent:
                    string connectionIP;
                    int connectionPort;
                    NetworkID networkID;
                    NodeID nodeID;
                    byte getConnectionInfoError;

                    NetworkTransport.GetConnectionInfo(socketID, connectionID, out connectionIP, out connectionPort, out networkID, out nodeID, out getConnectionInfoError); // ToDo: handle error
                    connectionIP = connectionIP.Substring(7);
                    currentMessageHandler.OnConnect(connectionIP, connectionID);
                    break;
                case NetworkEventType.DisconnectEvent:
                    currentMessageHandler.OnDisconnect(connectionID);
                    break;
                default:
                    break;
            }
        }
        else
        {
            throw new System.Exception("Did not find handler for current message");
        }

        foreach (var group in _activeGroups.Values.ToList())
        {
            group.OnSessionUpdate();
        }
    }

    private byte[] TakeRealData(byte[] buffer, int realDataLenght)
    {
        byte[] realData = new byte[realDataLenght];
        System.Buffer.BlockCopy(buffer, 0, realData, 0, realDataLenght);
        return realData;
    }
}
