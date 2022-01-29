using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public GameObject explosion;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        transform.Translate(Vector3.up*speed*Time.deltaTime);
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > 2*gameManager.levelRadius || Mathf.Abs(transform.position.z) > 2 * gameManager.levelRadius)
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
