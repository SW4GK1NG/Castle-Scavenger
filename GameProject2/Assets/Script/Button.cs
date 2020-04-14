using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{

    public LevelLoader LevelLoaderObject;
    public string NextLevel;

    // Start is called before the first frame update
    void Start()
    {
        LevelLoaderObject = FindObjectOfType<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressStart() {
        //Something here
        LevelLoaderObject.LoadNextLevel(NextLevel);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
