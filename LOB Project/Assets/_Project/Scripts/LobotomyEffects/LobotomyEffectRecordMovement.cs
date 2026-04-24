using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobotomyEffectRecordMovement : MonoBehaviour, ILobotomyEffect
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float recordingDuration;
    [Range(0.1f, 100)][SerializeField] private float _selectionWeight;

    public float GetEffectSelectionPriority()
    {
        return _selectionWeight;
    }

    public void StartEffect(Transform selection)
    {
        // start recording
        StateRecorder.Instance.StartRecording(playerTransform, recordingDuration);

        // save recording to database after complete
        Invoke(nameof(StateRecorder.Instance.SaveRecordingToDatabase), recordingDuration + 1);
    }

    public void StopEffect(Transform selection) { }
}
