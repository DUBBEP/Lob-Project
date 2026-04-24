using UnityEngine;

public class LobotomyEffectReloadScene : MonoBehaviour, ILobotomyEffect
{
    [Range(10, 30)][SerializeField] private float minTime;
    [Range(40, 60)][SerializeField] private float MaxTime;
    [SerializeField] private GameObject resetSequence;

    private float resetTimer;
    bool countdown;

    [Range(1, 100)][SerializeField] private float _selectionWeight;

    public float GetEffectSelectionPriority()
    {
        return _selectionWeight;
    }

    public void StartEffect(Transform selection)
    {
        if (countdown == false)
            resetTimer = Random.Range(minTime, MaxTime);
        
        countdown = true;
        //SaveActivityScore(activityScore, username);
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

    public async void SaveActivityScore(int aS, string uN)
    {
        ActivityMonitor activityScoreToSend = new ActivityMonitor()
        {
            activityScore = aS,
            userName = uN
        };

        bool success = await BackendActivityMonitorConnector.Instance.StoreObjectAsync(activityScoreToSend);
    }
}
