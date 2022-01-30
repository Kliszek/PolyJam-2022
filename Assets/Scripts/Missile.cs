using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public GameObject explosion;
    bool followPlayer = false;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        if(followPlayer)
            transform.LookAt(gameManager.playerInstance.transform);

        if (Mathf.Abs(transform.localPosition.x) > 2*gameManager.levelRadius || Mathf.Abs(transform.localPosition.z) > 2 * gameManager.levelRadius)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Explode();
        }
        else if(collision.gameObject.tag == "Player")
        {
            Debug.Log("YOU LOSE");
        }
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
