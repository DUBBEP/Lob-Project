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
        StateRecorder.Instance.StartRecording(playerTransform, recordingDuration, true);
    }

    public void StopEffect(Transform selection) { }
}
