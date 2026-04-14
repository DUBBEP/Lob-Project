using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobotomyEffectPlaySound : MonoBehaviour, ILobotomyEffect
{
    [SerializeField] private AudioSource _audio;
    [Range(1, 100)][SerializeField] private float _selectionWeight;

    public float GetEffectSelectionPriority()
    {
        return _selectionWeight;
    }

    public void StartEffect(Transform selection)
    {
        if (!_audio.isPlaying)
            _audio.Play();
    }

    public void StopEffect(Transform selection)
    {
        return;
    }
}
