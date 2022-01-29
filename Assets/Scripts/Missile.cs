using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;


    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.up*speed*Time.deltaTime);
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
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
        Destroy(this.gameObject);
    }
}
