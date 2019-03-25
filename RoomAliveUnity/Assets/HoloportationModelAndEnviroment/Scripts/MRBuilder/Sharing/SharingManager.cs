using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloGroup.Patterns;
using MRBuilder.UserInteraction.Input.KeyboardInput;
using System;
using MRBuilder.Architecture.Layouts;
using HoloGroup.UserInteraction.Input.KeyboardInput;

#if NETFX_CORE
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
#endif


public class SharingManager : MonoSingleton<SharingManager>
{
    public int UDP_Port_Application = 14329;
    public int TCP_Port = 14329;

    public bool IsMaster { get; private set; }

    public ServerSession ServerSession { get; private set; }
    private ClientSession _clientSession;

    private string _selfIP = "";

    private void Awake()
    {
//#if NETFX_CORE
//            foreach (HostName localHostName in NetworkInformation.GetHostNames())
//            {
//                if (localHostName.IPInformation != null)
//                {
//                    if (localHostName.Type == HostNameType.Ipv4)
//                    {
//                        _selfIP = localHostName.ToString();
//                        break;
//                    }
//                }
//            }
//#else
//        _selfIP = Network.player.ipAddress;
//#endif
    }

    public void StartHost()
    {
        IsMaster = true;

        AnchorManager.Instance.CreateAnchor();
        ServerSession = new ServerSession();
        ServerSession.Start(UDP_Port_Application, 0);

    }

    private string _ip = "192.168.121.229";

    public void StartClient()
    {
        IsMaster = false;

        KeyboardManager.Instance.InvokeKeyboard(Keyboard.Layout.Qwerty, GameObject.Find("MainMenuWindow").GetComponent<CanvasGroup>(), _ip, onChangedCallback, onCanceledCallback, onCompletedCallback);
    }

    private void onChangedCallback(string newIP)
    {
        _ip = newIP;
    }

    private void onCompletedCallback()
    {

        _clientSession = new ClientSession(_ip);
        _clientSession.Start(UDP_Port_Application, 0);

    }

    private void onCanceledCallback()
    {
        _ip = "";
    }

}

