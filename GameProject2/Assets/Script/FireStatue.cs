using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStatue : MonoBehaviour
{
    [HideInInspector]
    GameObject FirePoint;
    bool faceLeft;
    AudioSource sound;

    [Header("Config")]
    public GameObject Fireball;
    public float FireSpeed;
    public float StartDelay;
    public float FireRate;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        FirePoint = this.transform.Find("FirePoint").gameObject;

        if (transform.localScale.x >= 0) {
            faceLeft = false;
        } else {
            faceLeft = true;
        }

        InvokeRepeating("Shoot", StartDelay, FireRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot() {
        sound.Play();
        GameObject ShotFire = Instantiate(Fireball);
        Fire FireScript = ShotFire.GetComponent<Fire>();
        FireScript.location = FirePoint.transform.position;
        FireScript.Velocity = FireSpeed;
        FireScript.goLeft = faceLeft;
    }
}
