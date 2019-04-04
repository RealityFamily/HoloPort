using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioStreamer : MonoBehaviour
{
    public static AudioStreamer Instance;

    private AudioSource _audio;
    int _lastSample;
    AudioClip _clip;
    int FREQUENCY = 22050;

    private float _timer = 0;
    private bool _recordNow = false;


    private Queue<byte[]> _audioQueue = new Queue<byte[]>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(this);
    }

    private void Start()
    {
        Init();
    }

    private void OLDUpdate()
    {
        if (!_recordNow)
            return;
        if (_timer < 0.1f)
        {
            _timer += Time.deltaTime;
            return;
        }
        _timer = 0;

        int pos = Microphone.GetPosition(null);
        int diff = pos - _lastSample;
        if (diff > 0)
        {
            float[] samples = new float[diff * _clip.channels];
            _clip.GetData(samples, _lastSample);
            byte[] byteClip = ToByteArray(samples);


            if (byteClip.Length > 65000)
            {
                byte[] buf = new byte[65000];
                System.Buffer.BlockCopy(byteClip, 0, buf, 0, 65000);
                Configurator.Instance.SyncEnviromentSession.SendAudio(buf);

            }
            else
            {
                Configurator.Instance.SyncEnviromentSession.SendAudio(byteClip);
            }

            //SessionManager.SendMessageAudio(byteClip); //QosType.UnreliableFragmented
        }
        _lastSample = pos;
    }

    private void Update()
    {

        while (_audioQueue.Count > 0)
        {
            float[] f = ToFloatArray(_audioQueue.Dequeue());
            _audio.clip = AudioClip.Create("clip", f.Length, 1, FREQUENCY, false);
            _audio.clip.SetData(f, 0);
            if (!_audio.isPlaying) _audio.Play();
        }
    }

    public void Init()
    {
        _audio = gameObject.AddComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.loop = false;
    }

    public void StartRecording()
    {
        if (Microphone.devices.Length <= 0)
        {
            Debug.Log("No Mic");
            return;
        }

        _clip = Microphone.Start(null, true, 100, FREQUENCY);
        while (Microphone.GetPosition(null) < 0) { }
        _recordNow = true;
        Debug.Log("Recording started");
    }

    public void StopRecording()
    {
        _recordNow = false;
    }

    public void Receive(byte[] byteClip)
    {
        Debug.Log("Audio incoming");
        _audioQueue.Enqueue(byteClip);

        //float[] f = ToFloatArray(byteClip);
        //_audio.clip = AudioClip.Create("clip", f.Length, 1, FREQUENCY, false);
        //_audio.clip.SetData(f, 0);
        //if (!_audio.isPlaying) _audio.Play();
    }

    public byte[] ToByteArray(float[] floatArray)
    {
        int len = floatArray.Length * 4;
        byte[] byteArray = new byte[len];
        int pos = 0;
        foreach (float f in floatArray)
        {
            byte[] data = System.BitConverter.GetBytes(f);
            System.Array.Copy(data, 0, byteArray, pos, 4);
            pos += 4;
        }
        return byteArray;
    }

    public float[] ToFloatArray(byte[] byteArray)
    {
        int len = byteArray.Length / 4;
        float[] floatArray = new float[len];
        for (int i = 0; i < byteArray.Length; i += 4)
        {
            floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
        }
        return floatArray;
    }
}
