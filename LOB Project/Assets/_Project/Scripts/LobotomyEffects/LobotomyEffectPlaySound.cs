using UnityEngine;

public class LobotomyEffectPlaySound : MonoBehaviour, ILobotomyEffect
{
    [SerializeField] private int LobotomyEscalationValue = 0;
    [SerializeField] private AudioSource _audio;
    [Range(1, 100)][SerializeField] private float _selectionWeight;

    public float GetEffectSelectionPriority()
    {
        return _selectionWeight;
    }

    public void StartEffect(Transform selection)
    {
        if (LobotomyEscalationValue > LobotomySelectionResponse.selectionOcurrenceCounter) return;

        if (!_audio.isPlaying)
            _audio.Play();
    }

    public void StopEffect(Transform selection)
    {
        return;
    }
}
