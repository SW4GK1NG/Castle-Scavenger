using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointText : MonoBehaviour
{

    Text cpText;

    // Start is called before the first frame update
    void Start()
    {
        cpText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MasterControl.Instance.checkpointed == true) {
            cpText.text = "Checkpoint Passed";
        }
    }
}
