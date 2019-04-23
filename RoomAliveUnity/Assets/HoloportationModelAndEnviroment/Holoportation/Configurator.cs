using HoloGroup.Patterns;
using MRBuilder.Architecture;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configurator : MonoSingleton<Configurator>
{
    public Transform TestModelTransform;
    public AudioStreamer AudioStreamer;
    public bool IsFirstPC;

    private string RemouteMachineIP;

    public EnviromentSinchronizationSession SyncEnviromentSession;

    /// <summary>
    /// 
    /// </summary>
    private TCPsocket _modelDataServer;
    private TCPsocket _modelDataClient;

    private Transform _target;
    private Vector3 _lastPosition;
    private Vector3 _lastScale;
    private Quaternion _lastRotation;


    public Queue<byte[]> ServerQueue = new Queue<byte[]>();
    public Queue<byte[]> ClientQueue = new Queue<byte[]>();


    private void Start()
    {
        CreateModel();

        _target = BuildingManager.Instance.Building.transform;
        _target.gameObject.SetActive(false);


        _lastPosition = _target.position;
        _lastScale = _target.localScale;
        _lastRotation = _target.rotation;

        if(IsFirstPC)
        {
            _modelDataServer = new TCPsocket(new ModelDataTCPhost(this), ETcpSocketType.ListenerSocket, 55558);
        }
        else
        {
            _modelDataServer = new TCPsocket(new ModelDataTCPhost(this), ETcpSocketType.ListenerSocket, 55568);
        }

        StartCoroutine(StartClientRoutine());
    }

    private void CreateModel()
    {
        Model.Factory factory = new Model.Factory();
        BuildingManager.Instance.MakeBuilding(factory.MakeModel(TestModelTransform, "TestModel", true));
    }

    public UnityEngine.UI.InputField adress;
    public GameObject HoloModel;
    private int kinectClickCounter = 0;

    public void GetIpAdress()
    {
        if (kinectClickCounter < 1)
        {
            if (adress.text != "")
            {
                RemouteMachineIP = adress.text;
                HoloModel.active = true;
            }
        }
        kinectClickCounter++;
    }

    private void Update()
    {
        if (_lastPosition != _target.position || _lastRotation != _target.rotation || _lastScale != _target.localScale)
        {
            ServerQueue.Enqueue(SerializeTransform());
            Debug.Log("SEND");

            _lastPosition = _target.position;
            _lastScale = _target.localScale;
            _lastRotation = _target.rotation;
        }


        while(ClientQueue.Count > 0)
        {
            Debug.Log("RECEIVE");

            DeserializeTransform(ClientQueue.Dequeue());

            _lastPosition = _target.position;
            _lastScale = _target.localScale;
            _lastRotation = _target.rotation;
        }
    }

    private void OnDestroy()
    {
        _modelDataServer.Dispose();
        _modelDataClient.Dispose();
    }


    private byte[] SerializeTransform()
    {
        List<byte> serializedTransform = new List<byte>();

        Vector3 inversedPosition = _target.position;
        Quaternion inversedRotation = _target.rotation;

        // position
        serializedTransform.AddRange(BitConverter.GetBytes(inversedPosition.x));
        serializedTransform.AddRange(BitConverter.GetBytes(inversedPosition.y));
        serializedTransform.AddRange(BitConverter.GetBytes(inversedPosition.z));

        // rotation
        serializedTransform.AddRange(BitConverter.GetBytes(inversedRotation.x));
        serializedTransform.AddRange(BitConverter.GetBytes(inversedRotation.y));
        serializedTransform.AddRange(BitConverter.GetBytes(inversedRotation.z));
        serializedTransform.AddRange(BitConverter.GetBytes(inversedRotation.w));

        // scale
        serializedTransform.AddRange(BitConverter.GetBytes(_target.localScale.x));
        serializedTransform.AddRange(BitConverter.GetBytes(_target.localScale.y));
        serializedTransform.AddRange(BitConverter.GetBytes(_target.localScale.z));

        return serializedTransform.ToArray();
    }

    private void DeserializeTransform(byte[] buffer)
    {
        int offset = 0;

        {
            float x = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float y = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float z = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            _target.position = new Vector3(x, y, z); // AnchorSpaceToWorld(new Vector3(x, y, z));
        }

        {
            float x = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float y = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float z = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float w = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            _target.rotation = new Quaternion(x, y, z, w); // AnchorSpaceToWorld(new Quaternion(x, y, z, w));
        }

        {
            float x = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float y = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float z = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            _target.localScale = new Vector3(x, y, z);
        }
    }


    private IEnumerator StartClientRoutine()
    {
        yield return new WaitForSeconds(10);

        if (IsFirstPC)
        {
            _modelDataClient = new TCPsocket(new ModelDataTCPhost(this), ETcpSocketType.Socket, 55568, RemouteMachineIP);
        }
        else
        {
            _modelDataClient = new TCPsocket(new ModelDataTCPhost(this), ETcpSocketType.Socket, 55558, RemouteMachineIP);
        }

        yield return new WaitForSeconds(5);

        _target.gameObject.SetActive(true);
    }
}
