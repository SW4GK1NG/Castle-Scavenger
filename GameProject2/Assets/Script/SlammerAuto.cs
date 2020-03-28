using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlammerAuto : MonoBehaviour
{
    [HideInInspector]
    Rigidbody2D rb;
    Vector2 startPos;
    bool goingUp;

    [Header("Adjust")]

    public float downSpeed;
    public float DelayUp;
    public float DelayDown;
    public float startDelay;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        Invoke("LoopRepeat", startDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp) {
            if (transform.position.y >= startPos.y) {
                rb.velocity = Vector2.zero;
                goingUp = false;
                StartCoroutine(CooldownSmash());
            }
        }
    }

    void LoopRepeat() {
        StopCoroutine(CooldownSmash());
        rb.velocity = new Vector2(0, -1 * downSpeed);
        StartCoroutine(CooldownUp());
    }

    IEnumerator CooldownUp() {
        yield return new WaitForSecondsRealtime(DelayUp);
        goBack();
        StopCoroutine(CooldownUp());
    }

    IEnumerator CooldownSmash() {
        yield return new WaitForSecondsRealtime(DelayDown);
        LoopRepeat();
    }

    void goBack() {
        StopCoroutine(CooldownUp());
        rb.velocity = new Vector2(0, downSpeed / 7);
        goingUp = true;
    }

    void onCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Ground") {
            StartCoroutine(CooldownUp());
        }
        
    }
}
