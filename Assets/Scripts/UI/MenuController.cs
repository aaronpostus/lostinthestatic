using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum SceneName {
    MainMenu,
    Level,
    EndScene
}

public class MenuController : MonoBehaviour
{
    public void LoadScene(SceneNameWrapper sceneName) {
        SceneManager.LoadScene(sceneName.Value.ToString());
    }

    public void LoadScene(SceneName sceneName) {
        SceneManager.LoadScene(sceneName.ToString());
    }


    public void Quit() {
        Application.Quit();
    }
}
