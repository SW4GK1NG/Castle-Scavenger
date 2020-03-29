using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAxe : MonoBehaviour
{

    Rigidbody2D rb;
    AudioManager sound;
    public float LeftRange;
    public float RightRange;
    public float MaxVelocity;
    bool goingRight = true;
    public bool needBoots;
    float boots;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sound = FindObjectOfType<AudioManager>();
        if (needBoots) {
            boots = 250;
        } else {
            boots = 50;
        }
        rb.angularVelocity = MaxVelocity * boots;
    }

    // Update is called once per frame
    void Update()
    {
        push();
        /*if (rb.angularVelocity == 0) {
            sound.Play("Swing");
        }*/
    }

    void push () {
        if (transform.rotation.z > 0 && transform.rotation.z < RightRange && rb.angularVelocity > 0 && rb.angularVelocity < MaxVelocity) {
            rb.angularVelocity = MaxVelocity;
            //Debug.Log("Right");
        } else if (transform.rotation.z < 0 && transform.rotation.z > LeftRange && rb.angularVelocity < 0 && rb.angularVelocity > MaxVelocity * -1) {
            rb.angularVelocity = MaxVelocity * -1;
            //Debug.Log("Left");
        }
    }

}
