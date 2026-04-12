using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReplayHandler : MonoBehaviour
{
    [SerializeField] private float recordingLength;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform replayTargetTransform;

    private ReplayData replayData;

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

            StateReplay.Instance.StartReplay(replayData);
        }
    }

    private void MakeReplayData()
    {
        if (StateRecorder.Instance.log == null)
        {
            Debug.Log("There is no log in the state recorder");
            return;
        }

        replayData = new ReplayData()
        {
            target = replayTargetTransform,
            log = StateRecorder.Instance.log,
        };
    }
}
