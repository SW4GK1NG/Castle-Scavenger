using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    //public Vector2 parallaxEffectMultiplier;
    //public GameObject Camera;
    //bool infiniteHorizontal;
    //bool infiniteVertical;
    //float textureUnitSizeX;
    //float textureUnitSizeY;
    //Vector3 lastCamPosition;
    //Transform camTransform;
    //Vector2 startpost;
    //Vector2 endpost;
    //Vector2 size;

    public GameObject player;
    public GameObject camera;

    [Range(0f, 1f)]
    public float moveSpeed;

    

    // Start is called before the first frame update
    void Start()
    {
        /*camTransform = Camera.transform;
        lastCamPosition = camTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;*/

        /*size = transform.localScale;
        transform.position = Camera.transform.position;*/


    }

    void Update() {
        transform.position = new Vector2(player.transform.position.x * moveSpeed, camera.transform.position.y);
    }

    /*void LateUpdate()
    {
        Vector3 deltaMovement = camTransform.position - lastCamPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCamPosition = camTransform.position;

        if (infiniteHorizontal) {
            if (Mathf.Abs(camTransform.position.x - transform.position.x) >= textureUnitSizeX) {
                float offsetPositionX = (camTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(camTransform.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (infiniteVertical) {
            if (Mathf.Abs(camTransform.position.y - transform.position.y) >= textureUnitSizeY) {
                float offsetPositionY = (camTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, camTransform.position.y + offsetPositionY);
            }
        }

    }*/
}
