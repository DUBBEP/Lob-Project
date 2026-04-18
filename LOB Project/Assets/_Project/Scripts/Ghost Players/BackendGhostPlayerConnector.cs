using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BackendGhostPlayerConnector : MonoBehaviour
{
    [SerializeField]
    private string baseUrl = "http://127.0.0.1:8000/api/GhostRecords";
    public void SetURL(string url) => baseUrl = url;
    public string GetURL() => baseUrl;

    // --- GET ALL (INDEX) ---
    public async Task<List<GhostRecord>> GetIndexAsync()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Wrap the array so JsonUtility can read it
                string json = "{\"items\":" + request.downloadHandler.text + "}";
                GhostRecordWrapper wrapper = JsonUtility.FromJson<GhostRecordWrapper>(json);
                return new List<GhostRecord>(wrapper.items);
            }
            return null;
        }
    }

    // --- GET ONE (SHOW) ---
    public async Task<GhostRecord> GetShowAsync(int id)
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
                    return JsonUtility.FromJson<GhostRecord>(text);
                }
            }

            Debug.LogError($"Show failed for ID {id}. Error: {request.error}");
            return null;
        }
    }

    // --- SAVE NEW (STORE) ---
    public async Task<bool> StoreObjectAsync(GhostRecord data)
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
            request.SetRequestHeader("Accept", "application/json");
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
                    GhostRecord savedData = JsonUtility.FromJson<GhostRecord>(responseText);
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

    public async Task<List<GhostRecord>> GetRandomRecordsAsync()
    {
        // Note the /random at the end of the URL
        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/random"))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Re-using the same wrapper logic you have for Index
                string json = "{\"items\":" + request.downloadHandler.text + "}";
                GhostRecordWrapper wrapper = JsonUtility.FromJson<GhostRecordWrapper>(json);
                return new List<GhostRecord>(wrapper.items);
            }

            Debug.LogError($"Failed to fetch random records: {request.error}");
            return null;
        }
    }
}