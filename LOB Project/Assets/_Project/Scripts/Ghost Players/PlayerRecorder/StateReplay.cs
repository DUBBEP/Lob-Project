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

    public void StartSmoothReplay (ReplayData replay)
    {
        Queue<TransformSnapshot> queue = new Queue<TransformSnapshot>(replay.log.recordedSnapshots);

        activeReplays.Add(replay);
        StartCoroutine(RunReplaySmooth(replay));
    }

    private IEnumerator RunReplay(ReplayData replay)
    {
        Queue<TransformSnapshot> queue = new Queue<TransformSnapshot>(replay.log.recordedSnapshots);
        float replayTime = 0;

        while (replay.log.recordedSnapshots.Count > 0)
        {
            replayTime += Time.deltaTime;
            TransformSnapshot snapshot = queue.Peek();

            Debug.Log($"Snapshot Time Stamp: {snapshot.timeStamp} VS Replay Time: {replayTime}");
            
            if (replayTime >= snapshot.timeStamp)
            {
                queue.Dequeue();
                snapshot.target = replay.target;
                snapshot.UpdateTargetTransform();
            }

            yield return new WaitForFixedUpdate();
        }

        activeReplays.Remove(replay);
    }

    private IEnumerator RunReplaySmooth(ReplayData replay)
    {
        Queue<TransformSnapshot> queue = new Queue<TransformSnapshot>(replay.log.recordedSnapshots);
        float replayTime = 0;

        Vector3 lastPosition = replay.target.position;
        Quaternion lastRotation = replay.target.rotation;
        float lastTimestamp = 0;

        while (replay.log.recordedSnapshots.Count > 0)
        {
            replayTime += Time.deltaTime;
            TransformSnapshot nextSnapshot = queue.Peek();

            float timeSegment = nextSnapshot.timeStamp - lastTimestamp;
            float progress = (replayTime - lastTimestamp) / timeSegment;

            replay.target.position = Vector3.Lerp(lastPosition, nextSnapshot.position, progress);
            replay.target.rotation = Quaternion.Slerp(lastRotation, nextSnapshot.rotation, progress);

            Debug.Log($"Snapshot Time Stamp: {nextSnapshot.timeStamp} VS Replay Time: {replayTime}");

            if (replayTime >= nextSnapshot.timeStamp)
            {
                queue.Dequeue();

                lastPosition = nextSnapshot.position;
                lastRotation = nextSnapshot.rotation;
                lastTimestamp = nextSnapshot.timeStamp;
            }

            yield return null;
        }

        activeReplays.Remove(replay);
    }
}
