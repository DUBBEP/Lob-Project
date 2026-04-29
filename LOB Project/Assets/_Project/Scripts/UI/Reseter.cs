using UnityEngine;
using UnityEngine.SceneManagement;

public class Reseter : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLeaderboard()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
