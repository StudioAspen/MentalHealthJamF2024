using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] string sceneName;

    public void QuickChangeScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void ChangeNamedScene() {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrentScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
