using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityMetricModel : MonoBehaviour
{
    private async void Start()
    {
        int result = await BackendActivityMonitorConnector.Instance.GetActivityAsync();
        Debug.Log(result);

        if (result <= 0)
        {
            Debug.Log("ACTIVITY SCORE LESS OR EQUAL TO ZERO");
        }
        else if (result <= 100)
        {
            //Make model 1 active
            Debug.Log("MODEL 1");
        }
        else if (result <= 1000)
        {
            //Make model 2 active
            Debug.Log("MODEL 2");
        }
        else if (result <= 2000)
        {
            //Make model 3 active
            Debug.Log("MODEL 3");
        }
        else
        {
            //Make model 4 active
            Debug.Log("MODEL 4");
        }
    }


}
