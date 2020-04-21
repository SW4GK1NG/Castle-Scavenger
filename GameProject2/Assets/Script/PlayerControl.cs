using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    CollisionCheck coll;
    Collider2D coll2D;
    AnimScript anim;
    AudioManager sound;

    [Header("Player Config")]
    public float speed;
    public float jumpForce;
    public float dashSpeed;
    public float wallSlideSpeed;
    public float wallJumpLerp;
    public int maxJump = 2;
    public GameObject respawnPoint;
    public string nextStage;
    public LevelLoader LevelLoaderObject;
    public GameObject EndCanvas;
    public GameObject BloodSpill;
    public ParticleSystem Dust;
    public ParticleSystem DustRun;

    [Space]

    [Header("Player Status")]
    public int side = 1;
    public int timeJumped = 0;
    public bool isDead = false;
    public bool inGoal = false;
    public bool facingRight = true;
    public bool canMove = true;
    public bool canSlide = true;
    public bool onWall;
    public bool onWallPlayed;
    public bool isDashing;
    public bool wallJumped;
    public bool intoWall;
    public bool gameEnd = false;
    public bool gotGem = false;
    float isMoving;
    float x;
    float y;

    [Space]

    bool groundTouch;
    bool hasDashed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CollisionCheck>();
        anim = GetComponentInChildren<AnimScript>();
        coll2D = GetComponent<BoxCollider2D>();
        sound = FindObjectOfType<AudioManager>();
        
        if (MasterControl.Instance.checkpointed == true) {
            CheckpointRespawn();
        }
    }
    void Update()
    {
        if (!gameEnd || !gotGem) {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
        }

        Vector2 dir = new Vector2(x, y);

        Walk(dir);

        if(x == 0 || !canMove || !gameEnd || !groundTouch) {
            DustRun.Stop();
        }

        if (canMove) {
            anim.SetHorizontalMovement(x, y, rb.velocity.y);
        } else if (!canMove && gotGem) {
            anim.SetHorizontalMovement(0, 0, rb.velocity.y);
        } else if (!canMove && !gotGem) {
            anim.SetHorizontalMovement(x, y, rb.velocity.y);
        }

        if (coll.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<Jump>().enabled = true;
        }

        if (onWall)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }

        if (Input.GetButtonDown("Jump") && canMove && !isDashing)
        {

            if (coll.onWall && coll.onGround && x != 0) {
                StartCoroutine(SlideDisable());
            }

            if (coll.onGround || timeJumped < maxJump)
            {
                sound.Play("Bruh");
                anim.SetTrigger("jump");
                Jump(Vector2.up, false);
            }
            if (coll.onWall && !coll.onGround)
            {
                sound.Play("Bruh");
                anim.SetTrigger("jump");
                WallJump();
            }

        }

        if (coll.onWall && !coll.onGround && canSlide)
        {
            if (x != 0)
            {
                onWall = true;
                WallSlide();
                Dust.Play();
            }
        }

        if (!coll.onWall || coll.onGround)
        {
            onWall = false;
            onWallPlayed = false;
        }

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if (Input.GetButtonDown("Dash") && !hasDashed && canMove)
        {
            Dash(side);
        }

        if (x > 0 && canMove)
        {
            side = 1;
            anim.Flip(side);
        }
        if (x < 0 && canMove)
        {
            side = -1;
            anim.Flip(side);
        }

        if (gameEnd) {
            ForceWalk(new Vector2(1, 0));
            anim.SetHorizontalMovement(1, y, rb.velocity.y);
        }

        if (gotGem) {
            x = 0;
            y = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Wall")
        {
            timeJumped = 0;
            Dust.Play();
        }

        if (canMove && other.gameObject.tag == "Trap")
        {
            PlayerDead();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (canMove && other.gameObject.tag == "Trap")
        {
            PlayerDead();
        }

        if (other.gameObject.tag == "Finish") {
            StartCoroutine(EndStage());
        }

        if (other.gameObject.tag == "Gem") {
            StartCoroutine(GetGem(other.gameObject));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bound") {
            //PlayerDead();
        }
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;
    }

    void Walk(Vector2 dir)
    {

        if (!canMove && !gameEnd)
        {
            return;
        }

        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
            DustRun.Play();
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }

    }

    void ForceWalk(Vector2 dir) {
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    void Jump(Vector2 dir, bool wall)
    {
        if (timeJumped < maxJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += dir * jumpForce;
            timeJumped++;
        }
    }

    void WallSlide()
    {
        if (onWall && !onWallPlayed)
        {
            onWallPlayed = true;
            anim.SetTrigger("slide");
        }

        if (coll.wallSide != side)
        {
            anim.Flip(side * -1);
        }

        if (!canMove)
        {
            return;
        }

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
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
        Jump((Vector2.up + wallDir / 1.5f), true);

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

    void CheckpointRespawn() {
        transform.position = respawnPoint.transform.position;
    }

    void PlayerDead()
    {
        StartCoroutine(resetScreen());
        MasterControl.Instance.Deaths++;
        Instantiate(BloodSpill, transform.position, Quaternion.identity);
        canMove = false;
        rb.isKinematic = true;
        coll2D.isTrigger = true;
        rb.velocity = Vector2.zero;
        sound.Play("Dead");
        anim.SetTrigger("die");
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

    IEnumerator SlideDisable() {
        canSlide = false;
        yield return new WaitForSeconds(0.5f);
        canSlide = true;
        StopCoroutine(SlideDisable());
    }

    IEnumerator GroundDash()
    {
        Dust.Play();
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

    IEnumerator EndStage() {
        canMove = false;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        gameEnd = true;
        x = 1;
        Debug.Log("Attemp Claer CP");
        MasterControl.Instance.checkpointed = false;
        Debug.Log("Claer CP");
        yield return new WaitForSeconds(2f);
        x = 0;
        LevelLoaderObject.LoadNextLevel(nextStage);
    }

    IEnumerator GetGem(GameObject GemHit) {
        canMove = false;
        gotGem = true;
        rb.velocity = new Vector2(rb.velocity.x / 3, rb.velocity.y);
        anim.Stop();
        yield return new WaitForSeconds(1.25f);
        Destroy(GemHit);
        anim.SetTrigger("gem");
        yield return new WaitForSeconds(3.25f);
        Instantiate(EndCanvas);
    }

    IEnumerator resetScreen()
    {
        yield return new WaitForSecondsRealtime(3f);
        Scene scene = SceneManager.GetActiveScene();
		LevelLoaderObject.LoadNextLevel(scene.name);
    }

}