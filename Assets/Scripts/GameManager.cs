using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int levelRadius;
    public static GameManager instance;

    public PlayerController playerInstance;
    public float timeSurvived;

    public Transform levelLeft;
    public Transform levelRight;

    public MissileSpawner[] missileSpawners;

    void Awake()
    {
        if (!instance)
            instance = this;

        playerInstance = GameObject.Find("Player").GetComponent<PlayerController>();
        if (!playerInstance)
            Debug.LogError("ERROR! No player object found!!");

        timeSurvived = 0.0f;
    }

    void Update()
    {
        timeSurvived += Time.deltaTime;
    }
}
