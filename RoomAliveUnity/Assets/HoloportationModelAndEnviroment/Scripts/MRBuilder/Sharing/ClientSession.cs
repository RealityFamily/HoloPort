using MRBuilder.Architecture;
using MRBuilder.Architecture.Layouts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSession : SessionBehaviour
{
    private string _hostIp;
    private int _hostConnectID = -1;
    private TCPsocket _socket;

    public ClientSession(string hostIp)
    {
        _hostIp = hostIp;
    }

    public override void OnSessionStart()
    {
        base.OnSessionStart();
        _hostConnectID = ConnectTo(_hostIp, SharingManager.Instance.UDP_Port_Application);


    }

    public override void OnSessionEnd(byte error)
    {
        base.OnSessionEnd(error);
        _hostConnectID = -1;
        _socket.Dispose();
        Restart();
    }

    public override void OnConnect(string connectionIP, int connectionID)
    {

        base.OnConnect(connectionIP, connectionID);

        if(connectionID == _hostConnectID && !SharingManager.Instance.IsMaster)
        {
            if (!AnchorManager.Instance.AnchorEstablished)
            {
                _socket = new TCPsocket(new AnchorTransferingTcpSocket(), ETcpSocketType.ListenerSocket, SharingManager.Instance.TCP_Port);
                AnchorManager.Instance.OnAnchorEstablished += OnAnchorEstablished;
                Send(BitConverter.GetBytes(10), connectionID, "ReliableSequencedChanel"); // command 10 - request anchor from master(server)
            }
            else
            {
                OnAnchorEstablished();
            }
        }
    }


    public override void OnDataIncoming(byte[] data, int connectionID, int chanelID)
    {
        if (chanelID == _socketInfo.Chanels["UnreliableChanel"])
        {
            DeserializeTransform(data);
        }
        else
        {
            int offset = 0;
            int token = BitConverter.ToInt32(data, offset);
            offset += 4;

            if (token == 0)
            {
                LayoutKind layoutKind = (LayoutKind)BitConverter.ToInt32(data, offset);
                offset += 4;

                string layoutName = System.Text.Encoding.UTF8.GetString(data, offset, data.Length - offset);

                LayoutManager.Instance.SwitchLayoutKind(layoutKind, layoutName);
            }
            else if(token == 1)
            {
                LayoutKind layoutKind = (LayoutKind)BitConverter.ToInt32(data, offset);
                LayoutManager.Instance.SwitchAllLayoutsKind(layoutKind);
            }
        }
    }



    private void OnAnchorEstablished()
    {
        Send(BitConverter.GetBytes(11), _hostConnectID, "ReliableSequencedChanel"); // command 11 - say master that anchor established
        AnchorManager.Instance.OnAnchorEstablished -= OnAnchorEstablished;
    }


    private byte[] SerializeTransform()
    {
        List<byte> serializedTransform = new List<byte>();
        Vector3 inversedPosition = WorldToAnchorSpace(BuildingManager.Instance.Building.transform.position);
        Quaternion inversedRotation = WorldToAnchorSpace(BuildingManager.Instance.Building.transform.rotation);

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
        serializedTransform.AddRange(BitConverter.GetBytes(BuildingManager.Instance.Building.transform.localScale.x));
        serializedTransform.AddRange(BitConverter.GetBytes(BuildingManager.Instance.Building.transform.localScale.y));
        serializedTransform.AddRange(BitConverter.GetBytes(BuildingManager.Instance.Building.transform.localScale.z));

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

            BuildingManager.Instance.Building.transform.position = AnchorSpaceToWorld(new Vector3(x, y, z));
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

            BuildingManager.Instance.Building.transform.rotation = AnchorSpaceToWorld(new Quaternion(x, y, z, w));
        }

        {
            float x = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float y = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float z = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            BuildingManager.Instance.Building.transform.localScale = new Vector3(x, y, z);
        }
    }

    #region BEETWEEN_SPACE_TRANSFORMATION_CONVERT_METHODS

    // position
    public Vector3 WorldToAnchorSpace(Vector3 worldPosition)
    {

        return AnchorManager.Instance.Anchor.InverseTransformPoint(worldPosition);
    }

    public Vector3 AnchorSpaceToWorld(Vector3 hybridSpacePosition)
    {

        return AnchorManager.Instance.Anchor.TransformPoint(hybridSpacePosition);
    }

    // rotation
    public Quaternion WorldToAnchorSpace(Quaternion worldRotation)
    {

        return worldRotation * Quaternion.Inverse(AnchorManager.Instance.Anchor.rotation);
    }

    public Quaternion AnchorSpaceToWorld(Quaternion hybridSpaceRotation)
    {
        return hybridSpaceRotation * AnchorManager.Instance.Anchor.rotation;
    }

    #endregion BEETWEEN_SPACE_TRANSFORMATION_CONVERT_METHODS
}
