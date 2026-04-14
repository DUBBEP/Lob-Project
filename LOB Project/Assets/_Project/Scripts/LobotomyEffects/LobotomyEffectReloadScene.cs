using UnityEngine;

public class LobotomyEffectReloadScene : MonoBehaviour, ILobotomyEffect
{
    [SerializeField] private float resetTimer;
    [SerializeField] private GameObject resetSequence;

    bool countdown;

    [Range(1, 100)][SerializeField] private float _selectionWeight;

    public float GetEffectSelectionPriority()
    {
        return _selectionWeight;
    }

    public void StartEffect(Transform selection)
    {
        countdown = true;
    }

    private void FixedUpdate()
    {
        if (countdown)
        {
            resetTimer -= Time.deltaTime;
        }

        if (resetTimer < 0)
        {
            resetSequence.SetActive(true);
        }
    }

    public void StopEffect(Transform selection)
    {

    }
}
