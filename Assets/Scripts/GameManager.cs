using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public int levelRadius;
    public static GameManager instance;

    public PlayerController playerInstance;
    public GameObject hud;
    Slider teleportationSlider;

    public float timeSurvived;

    public Transform levelLeft;
    public Transform levelRight;

    public float maxTimeNoTeleportation;
    private float counter;


    public MissileSpawner[] missileSpawners;

    void Awake()
    {
        if (!instance)
            instance = this;

        playerInstance = GameObject.Find("Player").GetComponent<PlayerController>();
        if (!playerInstance)
            Debug.LogError("ERROR! No player object found!!");

        timeSurvived = 0.0f;
        counter = maxTimeNoTeleportation;
    }

    void Start()
    {
        teleportationSlider = GameObject.Find("TeleportationBar").GetComponent<Slider>();

    }

    void Update()
    {
        timeSurvived += Time.deltaTime;
        counter -= Time.deltaTime;

        foreach(MissileSpawner ms in missileSpawners)
        {
            ms.homingSpeed = Mathf.Clamp(4 + timeSurvived / 15, 0, 10);
            ms.missileSpeed = Mathf.Clamp(6 + timeSurvived / 12, 0, 15);
            ms.timeBetweenMissiles = Mathf.Clamp(1 - timeSurvived / 25, 0.2f, 1);
            ms.timeBetweenHomings = Mathf.Clamp(5 - timeSurvived / 30, 2, 5);
        }

        teleportationSlider.normalizedValue = counter / maxTimeNoTeleportation;

        if (counter <= 0.0f)
        {
            PlayerLost();
            Debug.Log("YOU LOST!");
        }
    }

    public void ResetTimer()
    {
        counter = maxTimeNoTeleportation;
        teleportationSlider.normalizedValue = counter / maxTimeNoTeleportation;
    }

    public void PlayerLost()
    {
        Time.timeScale = 0.0f;

    }
}
