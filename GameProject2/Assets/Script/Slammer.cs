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

    [Header("Detection Area")]

    public Vector2 collisionSize;
    public Vector2 detectionSize;
    private Color debugCollisionColor = Color.red;

    [Header("Offset")]

    public Vector2 detectionOffset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        detectionOffset = new Vector2(0, detectionSize.y);

        onGround = Physics2D.OverlapBox((Vector2)transform.position, collisionSize, groundLayer);
        Detected = Physics2D.OverlapBox((Vector2)transform.position - detectionOffset, detectionSize, playerLayer);
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position, collisionSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position - detectionOffset, detectionSize);
    }
}