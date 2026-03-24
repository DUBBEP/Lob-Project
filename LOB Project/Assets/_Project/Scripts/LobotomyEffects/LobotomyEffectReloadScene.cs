using UnityEngine;

public class LobotomyEffectReloadScene : MonoBehaviour, ILobotomyEffect
{
    [SerializeField]
    private float resetTimer;
    [SerializeField]
    private GameObject resetSequence;

    bool countdown;
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
