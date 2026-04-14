using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformSnapshot
{
    public Vector3 position;
    public Quaternion rotation;
    public float timeStamp;

    public void UpdateTargetTransform(Transform target)
    {
        target.position = position;
        target.rotation = rotation;
    }
}

[System.Serializable]
public class SnapshotLog
{
    public Vector3 startPosition;
    public List<TransformSnapshot> recordedSnapshots = new List<TransformSnapshot>();
}

[System.Serializable]
public class ReplayData
{
    public Transform target;
    public SnapshotLog log;
}