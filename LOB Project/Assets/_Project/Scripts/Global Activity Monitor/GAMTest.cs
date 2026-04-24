using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAMTest : MonoBehaviour
{
    public BackendActivityMonitorConnector service;

    public ActivityMonitor am;
    

    private async void Start()
    {
        am.userName = "Test1";
        am.activityScore = 10;
        bool test = await service.StoreObjectAsync(am);
        int result = await service.GetActivityAsync();
        Debug.Log(result);
    }
}
