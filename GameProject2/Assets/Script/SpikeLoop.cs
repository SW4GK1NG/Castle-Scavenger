using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLoop : MonoBehaviour
{

    [HideInInspector]
    BoxCollider2D coll;
    Animator anim;
    Vector2 activeOffset;
    Vector2 passiveOffset;

    [Header("Adjust")]
    public float StartDelay;
    public float Delay;



    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        Invoke("StabLoop", StartDelay);

        activeOffset = coll.offset;
        passiveOffset = coll.offset + new Vector2(0, -0.66f);

        coll.offset = passiveOffset;
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
        coll.offset = activeOffset;
        yield return new WaitForSecondsRealtime(1.3f);
        coll.offset = passiveOffset;
        yield return new WaitForSecondsRealtime(Delay);
        StabLoop();
        //StopAllCoroutines();
    }
}
