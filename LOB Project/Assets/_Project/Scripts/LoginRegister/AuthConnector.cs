using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class AuthConnector : MonoBehaviour
{
    private string baseUrl = "http://127.0.0.1:8000/api";

    public static AuthConnector Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public async Task<LoginResponse> Login(string username, string password)
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

                return MakeResponse(true, request);
            }

            Debug.LogError("Login Failed: " + request.downloadHandler.text);

            return MakeResponse(false, request);
        }
    }

    public async Task<LoginResponse> Register(string username, string password)
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
                return MakeResponse(true, request);
            }

            Debug.LogError("Registration Failed: " + request.downloadHandler.text);
            return MakeResponse(false, request);
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

    public async Task<bool> IsTokenValidAsync()
    {
        string token = PlayerPrefs.GetString("AuthToken", "");

        if (string.IsNullOrEmpty(token))
        {
            Debug.Log("No token found in local storage.");
            return false;
        }

        using (UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8000/api/user"))
        {
            // Standard Laravel/Sanctum Headers
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("Accept", "application/json");

            // We "await" the request until it finishes
            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Welcome back! Server confirmed: {request.downloadHandler.text}");
                return true;
            }
            else
            {
                Debug.LogWarning($"Session invalid (Code: {request.responseCode}). Clearing token.");
                PlayerPrefs.DeleteKey("AuthToken");
                return false;
            }
        }
    }

    private LoginResponse MakeResponse(bool success, UnityWebRequest request)
    {
        return new LoginResponse()
        {
            success = success,
            response = JsonUtility.FromJson<LaravelErrorResponse>(request.downloadHandler.text),
        };
    }
}