using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

#if NETFX_CORE
using Windows.Storage.Streams;
using System.Threading.Tasks;
#endif

public interface ICustomTCP
{
    void OnSocketStartingError(Exception e);
    void OnSocketListenerStartingError(Exception e);
    void OnListenerOpened(bool successfully);

#if NETFX_CORE
    Task<bool> ListenerTransferingProcess(DataWriter networkWriter, DataReader networkReader); // async 
    Task SocketTransferingProcess(bool connectSuccessful, DataWriter networkWriter = null, DataReader networkReader = null);
#else
    bool ListenerTransferingProcess(NetworkStream networkStream);
    void SocketTransferingProcess(bool connectSuccessful, NetworkStream networkStream = null);
#endif
}
