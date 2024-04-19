using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // loads a scene
        SceneManager.LoadScene(sceneName);
    }
}