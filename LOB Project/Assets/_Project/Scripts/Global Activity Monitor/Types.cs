using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActivityMonitor
{
    public int id;
    public string username;
    public int activity_score;

    public string ToJson() => JsonUtility.ToJson(this);
}

[System.Serializable]
public class  ActivityMonitorWrapper
{
    public ActivityMonitor[] items;    
}

