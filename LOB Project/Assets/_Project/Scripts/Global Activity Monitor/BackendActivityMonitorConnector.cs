using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BackendActivityMonitorConnector : MonoBehaviour
{
    private string baseUrl = "http://127.0.0.1:8000/api/ActivityMonitor";
    public void SetURL(string url) => baseUrl = url;
    public string GetURL() => baseUrl;

    // --- GET ALL (INDEX) ---
    public async Task<List<ActivityMonitor>> GetIndexAsync()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Wrap the array so JsonUtility can read it
                string json = "{\"items\":" + request.downloadHandler.text + "}";
                ActivityMonitorWrapper wrapper = JsonUtility.FromJson<ActivityMonitorWrapper>(json);
                return new List<ActivityMonitor>(wrapper.items);
            }
            return null;
        }
    }

    // --- GET ONE (SHOW) ---
    public async Task<ActivityMonitor> GetShowAsync(int id)
    {
        Debug.Log($"{baseUrl}/{id}");
        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/{id}"))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string text = request.downloadHandler.text;
                // Valid JSON objects always start with '{'
                if (!string.IsNullOrEmpty(text) && text.StartsWith("{"))
                {
                    return JsonUtility.FromJson<ActivityMonitor>(text);
                }
            }

            Debug.LogError($"Show failed for ID {id}. Error: {request.error}");
            return null;
        }
    }

    // --- SAVE NEW (STORE) ---
    public async Task<bool> StoreObjectAsync(ActivityMonitor data)
    {
        string json = JsonUtility.ToJson(data);
        using (UnityWebRequest request = new UnityWebRequest(baseUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;

                // Log the response so you can see if it's actually JSON or an Error Page
                Debug.Log("Server Response: " + responseText);

                if (responseText.StartsWith("{"))
                {
                    ActivityMonitor savedData = JsonUtility.FromJson<ActivityMonitor>(responseText);
                    data.id = savedData.id;
                    return true;
                }
            }

            // If we reach here, something went wrong
            Debug.LogError($"Store Failed. Code: {request.responseCode} | Error: {request.downloadHandler.text}");
            return false;
        }
    }

    // --- GET ACTIVITY (RECENT) ---
    public async Task<int> GetActivityAsync()
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/recent"))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                ActivityMonitorWrapper wrapper = JsonUtility.FromJson<ActivityMonitorWrapper>(json);

                int total = 0;

                foreach (var item in wrapper.items)
                {
                    total += item.activityScore;
                }
                
                return total;
            }

            Debug.LogError($"Recent Failed" + request.error);
            return 0;
        }
    }
}