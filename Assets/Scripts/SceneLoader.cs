using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Loading scene: " + sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting");
    }
}
