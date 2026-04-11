using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReplay : MonoBehaviour
{
    private List<ReplayData> activeReplays = new List<ReplayData>();

    public static StateReplay Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void StartReplay(ReplayData replay)
    {
        activeReplays.Add(replay);
        StartCoroutine(RunReplay(replay));
    }

    private IEnumerator RunReplay(ReplayData replay)
    {
        float replayTime = 0;
        while (replay.log.recordedSnapshots.Count > 0)
        {
            replayTime += Time.deltaTime;
            TransformSnapshot snapshot = replay.log.recordedSnapshots.Peek();

            Debug.Log($"Snapshot Time Stamp: {snapshot.timeStamp} VS Replay Time: {replayTime}");
            
            if (replayTime >= snapshot.timeStamp)
            {
                replay.log.recordedSnapshots.Dequeue();
                snapshot.target = replay.target;
                snapshot.UpdateTargetTransform();
            }

            yield return new WaitForFixedUpdate();
        }

        activeReplays.Remove(replay);
    }
}
