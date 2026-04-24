using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BackendPlayerRecordConnector : MonoBehaviour
{
    public static BackendPlayerRecordConnector Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private string baseUrl = "http://127.0.0.1:8000/api/PlayerRecords";
    public void SetURL(string url) => baseUrl = url;
    public string GetURL() => baseUrl;

    // --- GET ALL (INDEX) ---
    public async Task<List<PlayerRecord>> GetIndexAsync()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Wrap the array so JsonUtility can read it
                string rawText = request.downloadHandler.text;
                string json = "{\"items\":" + request.downloadHandler.text + "}";
                PlayerRecordListWrapper wrapper = JsonUtility.FromJson<PlayerRecordListWrapper>(json);
                
                if (wrapper != null && wrapper.items != null)
                {
                    return new List<PlayerRecord>(wrapper.items);
                }
                else
                {
                    Debug.LogError($"Failed to parse JSON. Raw API response: {rawText}");
                    return null;
                }
            }
            return null;
        }
    }

    // --- GET ONE (SHOW) ---
    public async Task<PlayerRecord> GetShowAsync(int id)
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
                    return JsonUtility.FromJson<PlayerRecord>(text);
                }
            }

            Debug.LogError($"Show failed for ID {id}. Error: {request.error}");
            return null;
        }
    }

    // --- SAVE NEW (STORE) ---
    public async Task<bool> StoreObjectAsync(PlayerRecord data)
    {
        if (!AuthManager.IsLoggedIn)
        {
            Debug.LogError("Cannot save: User not logged in.");
            return false;
        }

        string json = JsonUtility.ToJson(data);
        using (UnityWebRequest request = new UnityWebRequest(baseUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);

            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;

                // Log the response so you can see if it's actually JSON or an Error Page
                Debug.Log("Server Response: " + responseText);

                if (responseText.StartsWith("{"))
                {
                    PlayerRecord savedData = JsonUtility.FromJson<PlayerRecord>(responseText);
                    data.id = savedData.id;
                    return true;
                }
            }

            if (request.responseCode == 401)
            {
                AuthManager.Token = null;
                // Trigger UI to show "Session Expired, please login again."
            }

            // If we reach here, something went wrong
            Debug.LogError($"Store Failed. Code: {request.responseCode} | Error: {request.downloadHandler.text}");
            return false;
        }
    }
}
