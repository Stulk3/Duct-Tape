using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public void CloseGame()
    {
        SceneLoader.instance.CloseApplication();
    }
    public void Retry()
    {

    }
}
