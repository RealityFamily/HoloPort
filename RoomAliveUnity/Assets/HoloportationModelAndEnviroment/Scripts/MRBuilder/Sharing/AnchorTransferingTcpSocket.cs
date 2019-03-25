using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

#if NETFX_CORE
using Windows.Storage.Streams;
using System.Threading.Tasks;
#endif

public class AnchorTransferingTcpSocket : ICustomTCP
{
    public void OnSocketStartingError(Exception e) { }
    public void OnSocketListenerStartingError(Exception e) { }
    public void OnListenerOpened(bool successfully)
    {

    }

#if NETFX_CORE
    public async Task<bool> ListenerTransferingProcess(DataWriter networkWriter, DataReader networkReader)
    {
        UnityEngine.WSA.Application.InvokeOnAppThread(() => 
        {
        }, true);

        await networkReader.LoadAsync(4);
        int size = networkReader.ReadInt32();

        byte[] buffer = new byte[size];
        await networkReader.LoadAsync((uint)size);
        networkReader.ReadBytes(buffer);

        UnityEngine.WSA.Application.InvokeOnAppThread(() => 
        {
            AnchorManager.Instance.DeserializeAnchor(buffer);
        }, true);

        return true;
    }
    public async Task SocketTransferingProcess(bool connectSuccessful, DataWriter networkWriter = null, DataReader networkReader = null)
    {
        UnityEngine.WSA.Application.InvokeOnAppThread(() => 
        {
        }, true);

        if (connectSuccessful)
        {
            int size = AnchorManager.Instance.AnchorAsBytes.Length;

            networkWriter.WriteInt32(size);
            networkWriter.WriteBytes(AnchorManager.Instance.AnchorAsBytes);

            await networkWriter.StoreAsync();
            await networkWriter.FlushAsync();
        }
        else
        {
            UnityEngine.WSA.Application.InvokeOnAppThread(() => 
            {

            }, true);
        }
    }
#else
    public bool ListenerTransferingProcess(NetworkStream networkStream) { return false; }
    public void SocketTransferingProcess(bool connectSuccessful, NetworkStream networkStream = null) { }
#endif
}
