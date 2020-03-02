using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCount : MonoBehaviour
{
    public Text DeathAmount;
    public int DeathPull;

    // Start is called before the first frame update
    void Start()
    {
        DeathAmount = gameObject.GetComponent<Text>();
        DeathPull = MasterControl.Instance.Deaths;
        DeathAmount.text = ("Deaths: " + DeathPull.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
