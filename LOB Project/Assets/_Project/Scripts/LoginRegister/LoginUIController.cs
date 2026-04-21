using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginUIController : MonoBehaviour
{
    private string username;
    private string password;

    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private float ErrorDisplayTime = 30f;

    private void Awake()
    {
        CheckIfLoggedIn();
    }

    public void UpdatePassword(string newPass) => password = newPass;

    public void UpdateUsername(string newName) => username = newName;

    public async void OnLogin()
    {
        LoginResponse response = await AuthConnector.Instance.Login(username, password);

        HandleResponse(response);

    }

    public async void OnRegister()
    {
        LoginResponse response = await AuthConnector.Instance.Register(username, password);

        HandleResponse(response);
    }

    private void HandleResponse(LoginResponse responseData)
    {
        if (responseData.response == null)
        {
            StartCoroutine(DisplayError("No Response From Server."));
            return;
        }

        if (responseData.success)
        {
            Debug.Log("Successfully Registered new user");
            LoadGameScene();
        }
        else
        {
            Debug.Log("Failed to register new user");
            Debug.Log("Response Message is: " + responseData.response.message);
            StartCoroutine(DisplayError(responseData.response.message));
        }
    }

    private IEnumerator DisplayError(string message)
    {
        errorText.text = message;
        yield return new WaitForSeconds(ErrorDisplayTime);
        errorText.text = "";
    }

    private async void CheckIfLoggedIn()
    {
        if (AuthManager.IsLoggedIn)
        {
            bool authenticated = await AuthConnector.Instance.IsTokenValidAsync();

            if (authenticated)
                LoadGameScene();
            else
                Debug.Log("Player is not logged in");
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Main");
    }
}
