using UnityEngine;
using UnityEngine.SceneManagement;

public class Reseter : MonoBehaviour
{
    [SerializeField] private string leaderboardSceneName = "Leaderboard";
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLeaderboard()
    {
        SceneManager.LoadScene(leaderboardSceneName);
    }
}
