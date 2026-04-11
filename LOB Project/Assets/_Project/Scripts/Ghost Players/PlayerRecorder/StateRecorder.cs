using System.Collections;
using UnityEngine;

public class StateRecorder : MonoBehaviour
{
    [Range(0.001f, 5f)][SerializeField] private float recordingHeatbeatInverval;

    private SnapshotLog log;
    private float logDuration;
    BackendGhostPlayerConnector connector;

    private bool isRecording = false;

    public void StartRecording(Transform target, float duration)
    {
        if (isRecording)
        {
            Debug.Log("Already Recording. Aborting.");
            return;
        }

        log.startPosition = target.position;
        logDuration = duration;
        isRecording = true;
        StartCoroutine(RecordState(target, duration));
    }

    public async void SaveRecordingToDatabase()
    {
        GhostRecord data = new GhostRecord()
        {
            username = "TempUsername", // Method call to get Username
            actions = log,
            duration = logDuration,
        };

        bool success = await connector.StoreObjectAsync(data);

        if (success)
            Debug.Log("Saved to Laravel successfully!");
        else
            Debug.LogError("Save failed.");
    }

    private IEnumerator RecordState(Transform target, float recordingDuration)
    {
        float recordTime = 0f;
        float interval = 0f;

        while (recordTime < recordingDuration)
        {
            recordTime += Time.deltaTime;

            if (recordTime >= interval)
            {
                TransformSnapshot snapshot = new TransformSnapshot()
                {
                    target = target,
                    position = target.position,
                    rotation = target.rotation,
                    timeStamp = recordTime,
                };

                log.recordedSnapshots.Enqueue(snapshot);
                Debug.Log("Recorded Time: " + snapshot.timeStamp);

                interval += recordingHeatbeatInverval;
            }

            yield return new WaitForFixedUpdate();
        }

        isRecording = false;
    }
}