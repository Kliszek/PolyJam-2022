using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{

    GameManager gameManager;
    public GameObject missile;
    public float distanceFromCenter;
    private Vector3[] sides = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

    public float timeBetweenMissiles;
    private float counter;

    void Awake()
    {
    }

    void Start()
    {
        counter = 0.0f;
        gameManager = GameManager.instance;
    }

    void SpawnMissile()
    {
        int side = Random.Range(0,4);   //minInclusive, but maxExclusive
        int row = Random.Range(0, 2*gameManager.levelRadius);

        GameObject newMissile = Instantiate(missile, transform.position+sides[side] * distanceFromCenter, Quaternion.LookRotation(-sides[side]), transform) as GameObject;
        Debug.Log(newMissile.transform.position);
        Debug.Log(gameManager.levelRadius);
        //(side+1)&3 = (side+1)%4
        newMissile.transform.position += sides[(side+1)&3] * (gameManager.levelRadius - gameManager.playerInstance.playerRadius - row);
    }

    void Update()
    {
        if(counter <= 0.0f)
        {
            SpawnMissile();
            counter = timeBetweenMissiles;
        }
        counter -= Time.deltaTime;
    }
}
