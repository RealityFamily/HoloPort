using HoloGroup.Patterns;
using MRBuilder.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configurator : MonoSingleton<Configurator>
{
    public Transform TestModelTransform;
    public AudioStreamer AudioStreamer;

    private string RemouteMachineIP;

    public EnviromentSinchronizationSession SyncEnviromentSession;


    private void Start()
    {
        CreateModel();


        SyncEnviromentSession = new EnviromentSinchronizationSession();
        SyncEnviromentSession.Target = BuildingManager.Instance.Building.transform;
        SyncEnviromentSession.Start(55538, 0);

        SyncEnviromentSession.ConnectTo(RemouteMachineIP, 55538);


        //AudioStreamer.Init();
        //AudioStreamer.StartRecording();
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

}
