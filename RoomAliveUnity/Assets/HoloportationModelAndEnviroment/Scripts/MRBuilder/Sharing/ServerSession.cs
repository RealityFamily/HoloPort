using MRBuilder.Architecture;
using MRBuilder.Architecture.Layouts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSession : SessionBehaviour
{
    TCPsocket _socket;
    private Dictionary<int, string> _connectedClients = new Dictionary<int, string>();
    private List<int> _acceptedClients = new List<int>();


    public override void OnSessionStart()
    {
        base.OnSessionStart();

    }

    public override void OnConnect(string connectionIP, int connectionID)
    {

        base.OnConnect(connectionIP, connectionID);
        _connectedClients.Add(connectionID, connectionIP);
    }

    public override void OnDisconnect(int connectionID)
    {

        base.OnDisconnect(connectionID);
        _connectedClients.Remove(connectionID);
        _acceptedClients.Remove(connectionID);
    }

    public override void OnSessionEnd(byte error)
    {
        base.OnSessionEnd(error);
        _connectedClients.Clear();
        _acceptedClients.Clear();
        _socket.Dispose();
        Restart();
    }

    public override void OnDataIncoming(byte[] data, int connectionID, int chanelID)
    {
        if (chanelID == _socketInfo.Chanels["UnreliableChanel"])
        {

        }
        else
        {
            int commandNumber = BitConverter.ToInt32(data, 0);


            if (commandNumber == 10)
            {
                _socket = new TCPsocket(new AnchorTransferingTcpSocket(), ETcpSocketType.Socket, SharingManager.Instance.TCP_Port, _connectedClients[connectionID]);
            }
            else if (commandNumber == 11)
            {
                if(!_acceptedClients.Contains(connectionID))
                    _acceptedClients.Add(connectionID);
            }
        }
    }

    public override void OnSessionUpdate()
    {
        if (_acceptedClients.Count <= 0)
            return;

        foreach (var conID in _acceptedClients)
        {
            Send(SerializeTransform(), conID, "UnreliableChanel");
        }
    }


    public void SendLayoutChanging(LayoutKind layoutKind, string layoutName)
    {
        if (_acceptedClients.Count <= 0)
            return;

        List<byte> buffer = new List<byte>();

        buffer.AddRange(BitConverter.GetBytes(0)); // token for one layouts
        buffer.AddRange(BitConverter.GetBytes((int)layoutKind));
        buffer.AddRange(System.Text.Encoding.UTF8.GetBytes(layoutName));

        foreach (var conID in _acceptedClients)
        {
            Send(buffer.ToArray(), conID, "ReliableSequencedChanel");
        }
    }

    public void SendAllLayoutChanging(LayoutKind layoutKinde)
    {
        if (_acceptedClients.Count <= 0)
            return;

        List<byte> buffer = new List<byte>();

        buffer.AddRange(BitConverter.GetBytes(1)); // token for all layouts
        buffer.AddRange(BitConverter.GetBytes((int)layoutKinde));


        foreach (var conID in _acceptedClients)
        {
            Send(buffer.ToArray(), conID, "ReliableSequencedChanel");
        }
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
