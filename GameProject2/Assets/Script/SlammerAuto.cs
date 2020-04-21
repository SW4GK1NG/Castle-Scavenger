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
    public float maxTravel;
    public bool reverse;
    public ParticleSystem Dust;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        Dust = GetComponentInChildren<ParticleSystem>();
        if (reverse) {
            downSpeed = downSpeed * -1;
        }
        Invoke("Ready", startDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp) {
            if (!reverse && transform.position.y >= startPos.y) {
                Dust.Stop();
                rb.velocity = Vector2.zero;
                goingUp = false;
                StartCoroutine(CooldownSmash());
            }

            if (reverse && transform.position.y <= startPos.y) {
                Dust.Stop();
                rb.velocity = Vector2.zero;
                goingUp = false;
                StartCoroutine(CooldownSmash());
            }
        }

        if (!reverse) {
            if (transform.position.y <= startPos.y - maxTravel) {
                rb.velocity = Vector2.zero;
                StartCoroutine(CooldownUp());
            }
        } else {
            if (transform.position.y >= startPos.y + maxTravel) {
                rb.velocity = Vector2.zero;
                StartCoroutine(CooldownUp());
            }
        }
    }

    void GoDown() {
        StopCoroutine(Pullback());
        rb.velocity = new Vector2(0, -1 * downSpeed);
        StartCoroutine(CooldownUp());
    }

    void Ready() {
        StartCoroutine(Pullback());
    }

    IEnumerator Pullback() {
        StopCoroutine(CooldownSmash());
        rb.velocity = new Vector2(0, downSpeed / 14);
        yield return new WaitForSeconds(0.4f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.6f);
        GoDown();
    }

    IEnumerator CooldownUp() {
        Dust.Play();
        yield return new WaitForSecondsRealtime(DelayUp);
        goBack();
        StopCoroutine(CooldownUp());
    }

    IEnumerator CooldownSmash() {
        yield return new WaitForSecondsRealtime(DelayDown);
        StartCoroutine(Pullback());
        StopCoroutine(CooldownSmash());
    }

    void goBack() {
        StopCoroutine(CooldownUp());
        rb.velocity = new Vector2(0, downSpeed / 7);
        goingUp = true;
    }

    void onCollisionEnter2D(Collision2D other) {
        Dust.Play();
        if (other.gameObject.tag == "Ground") {
            Dust.Play();
            StartCoroutine(CooldownUp());
        }
        
    }

}
