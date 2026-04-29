using System.Collections.Generic;
using UnityEngine;

public class ActivityMetricModel : MonoBehaviour
{
    [SerializeField] private List<GameObject> moonStages = new List<GameObject>();

    private async void Start()
    {
        int result = await BackendActivityMonitorConnector.Instance.GetActivityAsync();
        Debug.Log("Activity Score is " + result);

        if (result <= 0)
        {
            SetModel(0);
            Debug.Log("ACTIVITY SCORE LESS OR EQUAL TO ZERO");
        }
        else if (result <= 100)
        {
            //Make model 1 active
            Debug.Log("MODEL 1");
            SetModel(0);
        }
        else if (result <= 300)
        {
            //Make model 2 active
            Debug.Log("MODEL 2");
            SetModel(1);
        }
        else if (result <= 900)
        {
            //Make model 3 active
            Debug.Log("MODEL 3");
            SetModel(2);
        }
        else
        {
            //Make model 4 active
            Debug.Log("MODEL 4");
            SetModel(2);
        }
    }

    private void SetModel(int stage)
    {
        if (moonStages == null || moonStages.Count == 0) return;

        foreach (GameObject moon in moonStages)
            moon.SetActive(false);

        moonStages[stage].SetActive(true);
    }
}
