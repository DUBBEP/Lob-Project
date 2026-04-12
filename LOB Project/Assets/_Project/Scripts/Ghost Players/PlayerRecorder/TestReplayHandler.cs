using UnityEngine;

public class TestReplayHandler : MonoBehaviour
{
    [SerializeField] private float recordingLength;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform replayTargetTransform;

    private BackendGhostPlayerConnector connector;

    private ReplayData replayData = new ReplayData();

    private void Start()
    {
        connector = FindFirstObjectByType<BackendGhostPlayerConnector>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StateRecorder.Instance.StartRecording(playerTransform, recordingLength);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            MakeReplayData();
            StateRecorder.Instance.SaveRecordingToDatabase();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GetFirstInDatabase();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (replayData == null)
            {
                Debug.Log("Replay Data is null");
                return;
            }

            if (replayData.log.recordedSnapshots.Count <= 0)
            {
                Debug.Log("Replay data has no recorded snapshots");
                return;
            }

            StateReplay.Instance.StartSmoothReplay(replayData);
        }
    }

    private void MakeReplayData()
    {
        if (StateRecorder.Instance.log == null)
        {
            Debug.Log("There is no log in the state recorder");
            return;
        }

        replayData.target = replayTargetTransform;
        replayData.log = StateRecorder.Instance.log;
    }

    public async void GetFirstInDatabase()
    {
        GhostRecord record = await connector.GetShowAsync(3);

        if (record == null)
        {
            Debug.Log("Record Retrieved from database was null");
            return;
        }

        replayData.target = replayTargetTransform;
        replayData.log = record.actions;
    }
}
