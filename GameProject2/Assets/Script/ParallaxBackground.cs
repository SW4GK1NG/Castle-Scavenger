using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Vector2 parallaxEffectMultiplier;
    public GameObject Camera;
    float textureUnitSizeX;
    Vector3 lastCamPosition;
    Transform camTransform;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.transform;
        lastCamPosition = camTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = camTransform.position - lastCamPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCamPosition = camTransform.position;

        if (Mathf.Abs(camTransform.position.x - transform.position.x) >= textureUnitSizeX) {
            float offsetPositionX = (camTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(camTransform.position.x, transform.position.y);
        }

    }
}
