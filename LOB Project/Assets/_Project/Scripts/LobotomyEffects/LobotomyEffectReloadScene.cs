using UnityEngine;

public class LobotomyEffectReloadScene : MonoBehaviour, ILobotomyEffect
{
    [Range(10, 30)][SerializeField] private float minTime;
    [Range(40, 60)][SerializeField] private float MaxTime;
    [SerializeField] private GameObject resetSequence;

    private Timer scoreTracker;
    private float resetTimer;
    bool countdown;
    private bool recordsSent;

    [Range(1, 100)][SerializeField] private float _selectionWeight;

    private void Awake()
    {
        scoreTracker = gameObject.AddComponent<Timer>();
    }

    public float GetEffectSelectionPriority()
    {
        return _selectionWeight;
    }

    public void StartEffect(Transform selection)
    {
        if (countdown) return;
        countdown = true;

        resetTimer = Random.Range(minTime, MaxTime);
    }

    private void FixedUpdate()
    {
        if (countdown)
            resetTimer -= Time.deltaTime;

        if (resetTimer < 0)
        {
            if (recordsSent == false)
            {
                RecordsHandler recordHandler = new RecordsHandler(
                    BackendActivityMonitorConnector.Instance,
                    BackendPlayerRecordConnector.Instance
                    );

                recordHandler.UploadActivityRecord();
                recordHandler.UploadPlayerRecord(scoreTracker.elapsedTime);
                Debug.Log("Uploading Player record");

                recordsSent = true;
            }

            resetSequence.SetActive(true);

        }
    }

    public void StopEffect(Transform selection) { }

    public async void SaveActivityScore(int aS, string uN)
    {
        ActivityMonitor activityScoreToSend = new ActivityMonitor()
        {
            activity_score = aS,
            username = uN
        };

        bool success = await BackendActivityMonitorConnector.Instance.StoreObjectAsync(activityScoreToSend);
    }
}
