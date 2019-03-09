using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA;

public class HoloConnection : MonoBehaviour
{
    public UnityEngine.UI.InputField adress;
    private void Start()
    {
        StartCoroutine(LoadingWindowsMrWrapper());
    }

    private IEnumerator LoadingWindowsMrWrapper()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(LoadDevice("WindowsMR"));
    }

    private static IEnumerator LoadDevice(string newDevice)
    {
        XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        XRSettings.enabled = true;
    }

    public void ConnectToHoloB()
    {   if(adress.text!=null)
        HolographicRemoting.Connect(adress.text);
    }

    public void DisconnectFromHoloB()
    {
        HolographicRemoting.Disconnect();
    }

}
