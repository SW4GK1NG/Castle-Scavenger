using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject CPT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && MasterControl.Instance.checkpointed == false) {
            MasterControl.Instance.checkpointed = true;
            GameObject TextCP = Instantiate(CPT);
            CheckpointText CPTScript = TextCP.GetComponent<CheckpointText>();
            CPTScript.stayPos = this.transform.position;
        }
    }
}
