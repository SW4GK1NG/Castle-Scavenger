using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    CollisionCheck coll;
    AnimScript anim;

    [Header("Player Value")]
    public float speed;
    public float jumpForce;
    public float dashSpeed;
    public float wallSlideSpeed;
    public float wallJumpLerp;
    public int timeJumped = 0;
    public int maxJump = 2;
    float isMoving;

    [Space]

    [Header("Player Status")]
    public int side = 1;
    public bool isDead = false;
    public bool inGoal = false;
    public bool facingRight = true;
    public bool canMove = true;
    public bool touchWall;
    public bool onWall;
    public bool isDashing;
    public bool wallJumped;

    [Space]

    bool groundTouch;
    bool hasDashed;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CollisionCheck>();
        anim = GetComponentInChildren<AnimScript>();
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        anim.SetHorizontalMovement(x, y, rb.velocity.y);
        
        if (coll.onGround && !isDashing)
            {
                wallJumped = false;
                GetComponent<Jump>().enabled = true;
            }

        if (onWall) {
            if (rb.velocity.y < -wallSlideSpeed) {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("jump");

            if (coll.onGround || timeJumped < maxJump) {
                //Debug.Log("Jump");
                Jump(Vector2.up, false);
            }
            if (coll.onWall && !coll.onGround) {
                //Debug.Log("Wall Jump");
                WallJump();
            }

        }

        if(coll.onWall && !coll.onGround)
        {
            if (x != 0)
            {
                onWall = true;
                WallSlide();
            }
        }

        if (!coll.onWall || coll.onGround) {
            onWall = false;
        }

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if(!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if (Input.GetButtonDown("Dash") && !hasDashed)
        {
            Dash(side);
        }
        
        if(x > 0)
        {
            side = 1;
            anim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            anim.Flip(side);
        }
    }

    void OnCollisionEnter2D (Collision2D other) {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Wall") {
            timeJumped = 0;
        }

        if (other.gameObject.tag == "Trap") {
            Debug.Log("Ded");
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag == "Trap") {
            Debug.Log("Ded");
        }
    }

    void OnCollisionExit2D (Collision2D other) {

    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;

        //jumpParticle.Play();
    }

    void Walk(Vector2 dir)
    {

        if (!canMove) {
            return;
        }

        if (!wallJumped) {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        } else {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }

    }

    void Jump (Vector2 dir, bool wall) {
        if (timeJumped < maxJump) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += dir * jumpForce;
            timeJumped++;
        }
    }

    void WallSlide()
    {
        if(coll.wallSide != side) {
            anim.Flip(side * -1);
        }

        if (!canMove) {
            return;
        }

        bool pushingWall = false;
        if((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall)) {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;
        wallJumped = true;
        rb.velocity = new Vector2(push, -wallSlideSpeed);
    }

    void WallJump()
    {
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        wallJumped = true;
        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;
        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);
        
    }

    void Dash(float side)
    {
        hasDashed = true;
        anim.SetTrigger("dash");
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(side, 0);
        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());
        rb.gravityScale = 0;
        GetComponent<Jump>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        rb.gravityScale = 2.5f;
        GetComponent<Jump>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.25f);
        if (coll.onGround)
            hasDashed = false;
    }

    IEnumerator DisableMovement(float time)
    {
        //Debug.Log("Delay");
        canMove = false;
        //Debug.Log("False");
        yield return new WaitForSeconds(time);
        //Debug.Log("True");
        canMove = true;
    }
    	
}
