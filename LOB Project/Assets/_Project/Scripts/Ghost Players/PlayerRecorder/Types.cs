using System.Collections.Generic;
using UnityEngine;

public class TransformSnapshot
{
    public Transform target;
    public Vector3 position;
    public Quaternion rotation;
    public float timeStamp;

    public void UpdateTargetTransform()
    {
        target.position = position;
        target.rotation = rotation;
    }
}

public class SnapshotLog
{
    public Vector3 startPosition;
    public Queue<TransformSnapshot> recordedSnapshots = new Queue<TransformSnapshot>();
}

public class ReplayData
{
    public Transform target;
    public SnapshotLog log;
}