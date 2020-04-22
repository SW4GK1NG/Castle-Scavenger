using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public string songName;
    AudioManager sound;

    // Start is called before the first frame update
    void Start()
    {
        sound = FindObjectOfType<AudioManager>();
        sound.Play(songName);
        //Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
