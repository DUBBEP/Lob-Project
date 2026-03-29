using System.Collections.Generic;
using UnityEngine;

public class GhostRecord
{
    public int id;
    public string username;
    public List<Command> actions;
    public float duration;

    public string ToJson() => JsonUtility.ToJson(this);
}

public class GhostRecordWrapper
{
    public GhostRecord[] records;
}