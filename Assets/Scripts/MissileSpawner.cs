using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum directions
{
    upDown,
    leftRight
};

public class MissileSpawner : MonoBehaviour
{

    GameManager gameManager;
    public GameObject missile;
    public GameObject homing;
    public float distanceFromCenter;
    private Vector3[] sides = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

    public directions spawnMode = directions.upDown;

    public float timeBetweenMissiles;
    public float timeBetweenHomings;
    public float maxDistanceToDestroy;
    private float missileCounter;
    private float homingCounter;

    void Awake()
    {
    }

    void Start()
    {
        missileCounter = 0.0f;
        homingCounter = 0.0f;
        gameManager = GameManager.instance;
    }

    void SpawnProjectile(GameObject projectile)
    {
        int side = Random.Range(0,4);   //minInclusive, but maxExclusive
        side = spawnMode == directions.upDown ? side & 2 : side | 1;  // now only forward or back
        int row = Random.Range(0, 2*gameManager.levelRadius);

        GameObject newMissile = Instantiate(projectile, transform.position+sides[side] * distanceFromCenter, Quaternion.LookRotation(-sides[side]), transform) as GameObject;

        newMissile.transform.position += sides[(side+1)&3] * (gameManager.levelRadius - gameManager.playerInstance.playerRadius - row);
        newMissile.GetComponent<Missile>().maxDistance = maxDistanceToDestroy;
    }

    void Update()
    {
        if(missileCounter <= 0.0f)
        {
            SpawnProjectile(missile);
            missileCounter = timeBetweenMissiles;
        }
        if(homingCounter <= 0.0f)
        {
            SpawnProjectile(homing);
            homingCounter = timeBetweenHomings;
        }
        missileCounter -= Time.deltaTime;
        homingCounter -= Time.deltaTime;
    }
}
