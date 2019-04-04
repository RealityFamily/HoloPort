using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;


#if NETFX_CORE
using Windows.Storage.Streams;
#endif

public class AudioClient : ICustomTCP
{
    public void OnSocketStartingError(Exception e) { }
    public void OnSocketListenerStartingError(Exception e) { }
    public void OnListenerOpened(bool successfully) { }

#if NETFX_CORE
    public async Task<bool> ListenerTransferingProcess(DataWriter networkWriter, DataReader networkReader)
    {
        //await ProjectSynchronizationManager.Instance.NetworkProvider.ReceivingProjectProcess(networkReader, networkWriter);
        return true;
    }
    public async Task SocketTransferingProcess(bool connectSuccessful, DataWriter networkWriter = null, DataReader networkReader = null)
    {
        if (!connectSuccessful) throw new ApplicationException("Can not connect by TCP");
        while (true)
        {
            uint lenght = await networkReader.LoadAsync(60000);
            byte[] buffer = new byte[lenght];
            networkReader.ReadBytes(buffer);

            AudioStreamer.Instance.Receive(buffer);

            await Task.Delay(100);

        }

        //await ProjectSynchronizationManager.Instance.NetworkProvider.SendingProjectProcess(networkReader, networkWriter);
    }
#else
    public bool ListenerTransferingProcess(NetworkStream networkStream)
    {
        return true;
    }

    public void SocketTransferingProcess(bool connectSuccessful, NetworkStream networkStream = null)
    {
        if (!connectSuccessful) throw new ApplicationException("Can not connect by TCP");
        while (true)
        {
            byte[] buffer = new byte[60000];
            networkStream.Read(buffer, 0, 60000);

            AudioStreamer.Instance.Receive(buffer);

            Thread.Sleep(50);

        }
    }
#endif
}
