using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyLanBroadcast : NetworkDiscovery
{
    public bool alive = true;

    public void StartBroadcast()
    {
        StopBroadcast();
        base.Initialize();
        base.StartAsServer();
    }
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
    }

    private void Update()
    {
        if (!alive)
        {
            StopBroadcast();
        }
    }
}
