using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public GameObject explosion;
    public bool followPlayer = false;
    GameManager gameManager;
    [HideInInspector]
    public float maxDistance = 13.0f;
    public bool collideWithEachOther = true;
    public float lifespan = 20.0f;
    public bool quietTimeoutDestroy = true;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0.0f)
            Explode(quietTimeoutDestroy);

        transform.Translate(Vector3.forward*speed*Time.deltaTime);
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        if(followPlayer)
            transform.LookAt(gameManager.playerInstance.transform);

        if (Mathf.Abs(transform.localPosition.x) > maxDistance || Mathf.Abs(transform.localPosition.z) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || (collision.gameObject.tag == "Projectile" && collideWithEachOther))
        {
            Explode();
        }
        else if(collision.gameObject.tag == "Player")
        {
            Explode();
            gameManager.PlayerLost();
            Debug.Log("You died. I survived max 40 seconds, yeah.");
        }
    }

    void Explode(bool quiet = false)
    {
        if(!quiet)
            Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
