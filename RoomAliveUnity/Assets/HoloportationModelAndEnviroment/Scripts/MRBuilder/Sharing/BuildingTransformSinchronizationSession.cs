using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSinchronizationSession : SessionBehaviour
{
    public Transform Target;

    private int _remoteMachineConnectionID = -1;


    private Vector3 _lastPosition;
    private Vector3 _lastScale;
    private Quaternion _lastRotation;

    private TCPsocket _audioClient;


    public override void OnSessionStart()
    {
        base.OnSessionStart();

        _lastPosition = Target.position;
        _lastScale = Target.localScale;
        _lastRotation = Target.rotation;
    }

    public override void OnSessionEnd(byte error)
    {
        AudioStreamer.Instance.StopRecording();
        _remoteMachineConnectionID = -1;
        base.OnSessionEnd(error);
    }

    public override void OnConnect(string connectionIP, int connectionID)
    {
        _remoteMachineConnectionID = connectionID;
        _audioClient = new TCPsocket(new AudioClient(), ETcpSocketType.Socket, 25734, connectionIP);
    }

    public override void OnSessionUpdate()
    {
        if(_remoteMachineConnectionID != -1)
        {
            if (_lastPosition != Target.position || _lastRotation != Target.rotation || _lastScale != Target.localScale)
            {
                Send(SerializeTransform(), _remoteMachineConnectionID, "UnreliableChanel");

                _lastPosition = Target.position;
                _lastScale = Target.localScale;
                _lastRotation = Target.rotation;
            }
        }
    }

    public override void OnDataIncoming(byte[] data, int connectionID, int chanelID)
    {
        if (chanelID == _socketInfo.Chanels["UnreliableChanel"])
        {
            DeserializeTransform(data);

            _lastPosition = Target.position;
            _lastScale = Target.localScale;
            _lastRotation = Target.rotation;
        }
        else if (chanelID == _socketInfo.Chanels["UnreliableFragmented"])
        {
            AudioStreamer.Instance.Receive(data);
        }
        else if(chanelID == _socketInfo.Chanels["ReliableSequencedChanel"])
        {

        }
    }

    public void SendAudio(byte[] clip)
    {
        //Send(clip, _remoteMachineConnectionID, _socketInfo.Chanels["UnreliableFragmented"]);
    }

    private byte[] SerializeTransform()
    {
        List<byte> serializedTransform = new List<byte>();

        Vector3 inversedPosition = Target.position; // WorldToAnchorSpace(Target.position);
        Quaternion inversedRotation = Target.rotation; // WorldToAnchorSpace(Target.rotation);

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
        serializedTransform.AddRange(BitConverter.GetBytes(Target.localScale.x));
        serializedTransform.AddRange(BitConverter.GetBytes(Target.localScale.y));
        serializedTransform.AddRange(BitConverter.GetBytes(Target.localScale.z));

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

            Target.position = new Vector3(x, y, z); // AnchorSpaceToWorld(new Vector3(x, y, z));
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

            Target.rotation = new Quaternion(x, y, z, w); // AnchorSpaceToWorld(new Quaternion(x, y, z, w));
        }

        {
            float x = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float y = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            float z = BitConverter.ToSingle(buffer, offset);
            offset += 4;

            Target.localScale = new Vector3(x, y, z);
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
