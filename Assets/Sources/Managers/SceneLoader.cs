using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    public void LoadNextSceneAsync()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation asyncUnload = SceneManager.LoadSceneAsync(sceneIndex);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex +1);
    }
    public void LoadPreviousSceneAsync()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation asyncUnload = SceneManager.LoadSceneAsync(sceneIndex);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex - 1);
    }
    public void CloseApplication()
    {
        Application.Quit();
    }
}
