using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [HideInInspector]
    Rigidbody2D rb;
    public float Velocity;
    public Vector2 location;
    public bool goLeft;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = location;
        if (goLeft) {
            rb.velocity = new Vector2(Velocity * -1, 0);
            transform.localScale = 
                new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        } else {
            rb.velocity = new Vector2(Velocity, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Ground") {
            Destroy(gameObject);
        }
    }
}
