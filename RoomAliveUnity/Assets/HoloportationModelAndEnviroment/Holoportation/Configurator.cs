using HoloGroup.Patterns;
using MRBuilder.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Configurator : MonoSingleton<Configurator>
{
    public Transform TestModelTransform;
    public AudioStreamer AudioStreamer;

    private string RemouteMachineIP;
    public InputField RemouteMachineField;

    public EnviromentSinchronizationSession SyncEnviromentSession;


    private void Start()
    {
        CreateModel();


        SyncEnviromentSession = new EnviromentSinchronizationSession();
        SyncEnviromentSession.Target = BuildingManager.Instance.Building.transform;
        SyncEnviromentSession.Start(55538, 0);

        SyncEnviromentSession.ConnectTo(RemouteMachineIP, 55538);


        AudioStreamer.Init();
        AudioStreamer.StartRecording();
    }

    private void CreateModel()
    {
        Model.Factory factory = new Model.Factory();
        BuildingManager.Instance.MakeBuilding(factory.MakeModel(TestModelTransform, "TestModel", true));
    }

    public void GetIPAdress()
    {
        if (RemouteMachineField.text != "")
        {
            RemouteMachineIP = RemouteMachineField.text;
            gameObject.SetActive(true);
        }
    }

}
