using UnityEngine;

public class RecordsHandler
{
    private BackendActivityMonitorConnector activityConnector;
    private BackendPlayerRecordConnector playerRecordConnector;
    private float elapsedTime = 0f;
    
    public RecordsHandler(BackendActivityMonitorConnector activity, BackendPlayerRecordConnector player)
    {
        activityConnector = activity;
        playerRecordConnector = player;
    }
    
    public async void UploadActivityRecord()
    {
        ActivityMonitor activityRecord = new ActivityMonitor()
        {
            userName = AuthManager.GetUsername(),
            activityScore = 10,
        };

        bool test = await activityConnector.StoreObjectAsync(activityRecord);

        LogSuccess(test);

        int result = await activityConnector.GetActivityAsync();
        Debug.Log("Activity score total is: " + result);
    }

    public async void UploadPlayerRecord(float elapsedTime)
    {
        PlayerRecord record = new PlayerRecord()
        {
            username = AuthManager.GetUsername(),
            score = Mathf.Round(elapsedTime * 100) / 100,
        };

        bool success = await playerRecordConnector.StoreObjectAsync(record);

        LogSuccess(success);
    }

    private void LogSuccess(bool success)
    {
        if (success)
            Debug.Log("Activity Record Upload Successful");
        else
            Debug.Log("Activity Record Upload Failed");
    }
}
