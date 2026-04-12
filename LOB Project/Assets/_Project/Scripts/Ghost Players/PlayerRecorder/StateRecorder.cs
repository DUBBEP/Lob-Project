using System.Collections;
using UnityEngine;

public class StateRecorder : MonoBehaviour
{
    [Range(0.001f, 5f)][SerializeField] private float recordingHeatbeatInverval;

    public SnapshotLog log { get; private set; } = new SnapshotLog();
    public bool isRecording { get; private set; } = false;
    private float logDuration;
    private BackendGhostPlayerConnector connector;

    public static StateRecorder Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        connector = FindFirstObjectByType<BackendGhostPlayerConnector>();
    }

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
        Debug.Log($"Log being sent is {log}");
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

                log.recordedSnapshots.Add(snapshot);
                Debug.Log("Recorded Time: " + snapshot.timeStamp);

                interval += recordingHeatbeatInverval;
            }

            yield return new WaitForFixedUpdate();
        }

        isRecording = false;
    }
}