using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointText : MonoBehaviour
{
    public Vector2 stayPos;
    Text cpText;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Checkpointed");
        cpText = GetComponent<Text>();
        transform.position = stayPos;
        Invoke("Killme", 2);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (MasterControl.Instance.checkpointed == true) {
            cpText.text = "Checkpoint Passed";
        }*/
    }

    void Killme() {
        Destroy(gameObject);
    }
}
