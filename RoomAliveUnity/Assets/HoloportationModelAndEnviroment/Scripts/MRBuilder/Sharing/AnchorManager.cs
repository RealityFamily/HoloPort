using HoloGroup.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

public class AnchorManager : MonoSingleton<AnchorManager>
{
    public bool AnchorEstablished { get; private set; }
    public Transform Anchor { get; private set; }
    public byte[] AnchorAsBytes { get; private set; }
    public event Action OnAnchorEstablished = () => { };
    private List<byte> _buffer = new List<byte>();


    private void Awake()
    {
        AnchorEstablished = false;
    }

    public void CreateAnchor()
    {
        _buffer.Clear();

        GameObject go = new GameObject("Anchor");
        go.transform.position = Camera.main.transform.position;
        var anchor = go.AddComponent<UnityEngine.XR.WSA.WorldAnchor>();
        anchor.OnTrackingChanged += Anchor_OnTrackingChangedAfterCreate;
        Anchor_OnTrackingChangedAfterCreate(anchor, anchor.isLocated);
    }

    public void DeserializeAnchor(byte[] buffer)
    {


        UnityEngine.XR.WSA.Sharing.WorldAnchorTransferBatch.ImportAsync(buffer, onComplete);
    }

    private void onComplete(UnityEngine.XR.WSA.Sharing.SerializationCompletionReason completionReason, UnityEngine.XR.WSA.Sharing.WorldAnchorTransferBatch deserializedTransferBatch)
    {
        if(completionReason == UnityEngine.XR.WSA.Sharing.SerializationCompletionReason.Succeeded)
        {

            GameObject go = new GameObject("Anchor");

            var anchor = deserializedTransferBatch.LockObject("Anchor", go);
            anchor.OnTrackingChanged += Anchor_OnTrackingChangedAfterReceive;
            Anchor_OnTrackingChangedAfterReceive(anchor, anchor.isLocated);
        }
    }

    private void Anchor_OnTrackingChangedAfterCreate(UnityEngine.XR.WSA.WorldAnchor self, bool located)
    {
        if(located)
        {
            Anchor = self.transform;
            AnchorEstablished = true;
            self.OnTrackingChanged -= Anchor_OnTrackingChangedAfterCreate;

            OnAnchorEstablished();

            UnityEngine.XR.WSA.Sharing.WorldAnchorTransferBatch batch = new UnityEngine.XR.WSA.Sharing.WorldAnchorTransferBatch();
            batch.AddWorldAnchor("Anchor", self);
            UnityEngine.XR.WSA.Sharing.WorldAnchorTransferBatch.ExportAsync(batch, onDataAvailable, onCompleted);
        }
    }

    private void Anchor_OnTrackingChangedAfterReceive(UnityEngine.XR.WSA.WorldAnchor self, bool located)
    {
        if (located)
        {
            Anchor = self.transform;
            AnchorEstablished = true;
            self.OnTrackingChanged -= Anchor_OnTrackingChangedAfterReceive;
            OnAnchorEstablished();
        }
    }

    private void onCompleted(UnityEngine.XR.WSA.Sharing.SerializationCompletionReason completionReason)
    {
        if(completionReason == UnityEngine.XR.WSA.Sharing.SerializationCompletionReason.Succeeded)
        {
            if (_buffer.Count > 5 * 1024 * 1024)
            {
                AnchorAsBytes = _buffer.ToArray();
            }
            else
            {
                StartCoroutine(RecreateAnchorProcess());
            }
        }
    }

    IEnumerator RecreateAnchorProcess()
    {
        yield return new WaitForSeconds(0.3f);
        CreateAnchor();
    }

    private void onDataAvailable(byte[] data)
    {
        _buffer.AddRange(data);
    }
}
