using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
    [SerializeField] private string sceneName = "Main";
    public void LoadMainScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
