using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAxe : MonoBehaviour
{

    Rigidbody2D rb;
    public float LeftRange;
    public float RightRange;
    public float MaxVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = MaxVelocity * 3;
    }

    // Update is called once per frame
    void Update()
    {
        push();
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
