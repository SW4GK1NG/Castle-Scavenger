using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;

    //Player Value
    bool facingRight = true;
    float maximumSpeed = 4f;
    float jumpForce = 300f;
    float isMoving;
    bool isDead = false;
    bool inGoal = false;
    bool canMove;
    bool grounded;
    bool walled;
    int timeJumped = 0;

    //Ground Check
    public LayerMask whatIsGround;
    public Transform groundCheck;
    float groundRad = 0.1f;

    //Wall Check
    public LayerMask whatIsWall;
    public Transform wallCheck;
    float wallRad = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        canMove = true;
        grounded = Physics2D.OverlapCircle (groundCheck.position, groundRad, whatIsGround);
        walled = Physics2D.OverlapCircle (wallCheck.position, wallRad, whatIsWall);

        if (!isDead && !inGoal) {
            isMoving = Input.GetAxis ("Horizontal");
        }

        //Walking Mechanic
        if (canMove && !walled) {
            if (Input.GetKey(KeyCode.RightArrow)) {
			    rb.velocity = new Vector2(maximumSpeed ,rb.velocity.y);
		    } else if (Input.GetKey(KeyCode.LeftArrow)) {
			    rb.velocity = new Vector2(maximumSpeed * -1 ,rb.velocity.y);
            } else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.RightArrow)) {
				rb.velocity = new Vector2(0,rb.velocity.y);
			}
        }

        if (isMoving > 0 && !facingRight) {
			Flipper();
		} else if (isMoving < 0 && facingRight) {
			Flipper();
		}
    }

    void Update () {
        if (canMove && !isDead) {
            jump();
        }
    }

    void OnCollisionEnter2D (Collision2D other) {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Wall") {
            timeJumped = 0;
        }
    }

    void Flipper (){
	    facingRight = !facingRight;
		Vector3 TheScale = transform.localScale;
		TheScale.x *= -1;
		transform.localScale = TheScale;
	}

    void jump () {
        if (Input.GetKeyDown(KeyCode.Space) && !isDead && canMove && (grounded || timeJumped < 2)) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce));
            timeJumped++;
        }
    }
    	
}
