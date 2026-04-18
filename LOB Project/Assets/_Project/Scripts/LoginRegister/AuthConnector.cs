using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text;

public class AuthConnector : MonoBehaviour
{
    private string baseUrl = "http://127.0.0.1:8000/api";

    public async Task<bool> Login(string username, string password)
    {
        LoginFields fields = new LoginFields { name = username, password = password };
        string json = JsonUtility.ToJson(fields);

        using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                AuthResponse response = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);

                // Save token for all future requests
                AuthManager.Token = response.token;

                // Optional: Store username locally so StateRecorder can access it
                PlayerPrefs.SetString("CurrentUsername", response.user.name);

                Debug.Log("Login Successful! Token stored.");
                return true;
            }

            Debug.LogError("Login Failed: " + request.downloadHandler.text);
            return false;
        }
    }

    public async Task<bool> Register(string username, string password)
    {
        // Note: Laravel 'confirmed' rule looks for 'password_confirmation'
        string json = "{\"name\":\"" + username + "\", \"password\":\"" + password + "\", \"password_confirmation\":\"" + password + "\"}";

        using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/register", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Usually, Laravel returns the token immediately after registration
                AuthResponse response = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
                AuthManager.Token = response.token;
                PlayerPrefs.SetString("CurrentUsername", response.user.name);
                return true;
            }

            Debug.LogError("Registration Failed: " + request.downloadHandler.text);
            return false;
        }
    }

    public async Task<bool> Logout()
    {
        if (!AuthManager.IsLoggedIn) return true;

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm($"{baseUrl}/logout", ""))
        {
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);

            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Logged out from server.");
            }
            else
            {
                Debug.LogWarning("Server logout failed, but clearing local token anyway.");
            }

            // Always clear local data regardless of server response
            AuthManager.Token = null;
            PlayerPrefs.DeleteKey("CurrentUsername");
            return true;
        }
    }
}