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
    public ParticleSystem trail;
    public ParticleSystem Dead;

    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        transform.position = location;
        if (goLeft) {
            rb.velocity = new Vector2(Velocity * -1, 0);
            transform.localScale = 
                new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            var TrailShape = trail.shape;
            TrailShape.rotation = new Vector3(TrailShape.rotation.x, TrailShape.rotation.y * -1, TrailShape.rotation.z);
            TrailShape.position = new Vector3(TrailShape.position.x + 0.4f, TrailShape.position.y, TrailShape.position.z);
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
            Instantiate(Dead, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
