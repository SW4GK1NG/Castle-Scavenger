using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slammer : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    [Space]

    public bool onGround;
    public bool Detected;
    public  float downVelocity;

    [Space]

    [Header("Area")]

    public Vector2 collisionSize;
    public Vector2 detectionSize;
    private Color debugCollisionColor = Color.red;

    /*[Header("Offset(Not used yet)")]

    public Vector2 detectionOffset;*/

    [HideInInspector]

    Rigidbody2D rb;
    Vector2 startPoint;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPoint = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {  

        onGround = Physics2D.OverlapBox((Vector2)transform.position, collisionSize, groundLayer);

        Detected = Physics2D.OverlapBox((Vector2)transform.position, detectionSize, playerLayer);

        if (Detected && !onGround) {
            //StartCoroutine(Slamdown());
        }

    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position, collisionSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position, detectionSize);
    }

    void onCollisionEnter2D (Collision2D other) {
        if (other.gameObject.tag == "Ground") {
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator Slamdown () {
        yield return new WaitForSecondsRealtime(1f);
        rb.velocity = new Vector2(0, -1 * downVelocity);
        StopAllCoroutines();
    }

}