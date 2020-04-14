using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    void Update()
    {
        
    }

    public void LoadNextLevel(string SceneName) {
        StartCoroutine(LoadingTime(SceneName));
    }

    IEnumerator LoadingTime(string Scene) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(Scene);
    }
}
