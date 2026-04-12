using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostRecord
{
    public int id;
    public string username;
    public SnapshotLog actions;
    public float duration;

    public string ToJson() => JsonUtility.ToJson(this);
}

[System.Serializable]
public class GhostRecordWrapper
{
    public GhostRecord[] items;
}