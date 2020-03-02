using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterControl : MonoBehaviour
{
    public static MasterControl Instance;
    public int Deaths = 0;
    void Awake ()   
       {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
