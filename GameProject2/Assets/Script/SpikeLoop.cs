﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLoop : MonoBehaviour
{

    [HideInInspector]
    BoxCollider2D coll;
    Animator anim;
    Vector2 activeOffset;
    Vector2 passiveOffset;
    AudioSource sound;

    [Header("Adjust")]
    public float StartDelay;
    public float Delay;



    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        Invoke("StabLoop", StartDelay);

        activeOffset = coll.offset;
        passiveOffset = coll.offset + new Vector2(0, -0.4f);

        //coll.offset = passiveOffset;
        coll.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StabLoop() {
        StopAllCoroutines();
        StartCoroutine(Stab());
    }

    IEnumerator Stab() {
        anim.SetTrigger("stab");
        yield return new WaitForSecondsRealtime(0.35f);
        coll.enabled = true;
        sound.Play();
        yield return new WaitForSecondsRealtime(1.1f);
        coll.enabled = false;
        yield return new WaitForSecondsRealtime(Delay);
        StabLoop();
        //StopAllCoroutines();
    }
}
