using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;



public class ModelDataTCPhost : ICustomTCP
{
    private Configurator _config;

    public ModelDataTCPhost(Configurator conf)
    {
        _config = conf;
    }

    public void OnSocketStartingError(Exception e) { }
    public void OnSocketListenerStartingError(Exception e) { }
    public void OnListenerOpened(bool successfully) { }


    public bool ListenerTransferingProcess(NetworkStream networkStream)
    {
        while (true)
        {
            if (_config.ServerQueue.Count == 0)
            {
                Thread.Sleep(50);
            }
            else
            {
                while (_config.ServerQueue.Count > 0)
                {
                    var buffer = _config.ServerQueue.Dequeue();
                    networkStream.Write(buffer, 0, buffer.Length);
                    networkStream.FlushAsync();
                }
            }
        }


        return true;
    }

    public void SocketTransferingProcess(bool connectSuccessful, NetworkStream networkStream = null)
    {
        if (!connectSuccessful)
        {
            Debug.Log("TCP NO CONNECT");
            return;
        }
        else
        {
            Debug.Log("TCP CONNECTED");
        }

        while (true)
        {
            byte[] packet = new byte[40];
            var countReal = networkStream.Read(packet, 0, 40);
            if (countReal < 40) throw new Exception("Incorrect Size!!!!");
            else
                _config.ClientQueue.Enqueue(packet);

            Thread.Sleep(50);
        }
    }
}

