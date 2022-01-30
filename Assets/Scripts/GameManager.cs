using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private Text timeSurvivedUI;
    public GameObject loseScreen;
    public GameObject pauseScreen;
    public GameObject mainScreen;
    [HideInInspector]
    public bool paused;


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
        loseScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    void Start()
    {
        teleportationSlider = GameObject.Find("TeleportationBar").GetComponent<Slider>();
        timeSurvivedUI = GameObject.Find("TimeSurvived").transform.GetChild(0).GetComponent<Text>();
        paused = true;
        Time.timeScale = 0.0f;
    }

    void Update()
    {
        bool pausePressed = playerInstance.GetInputScript().Ground.Pause.WasReleasedThisFrame();
        if (!paused && pausePressed)
            Pause();
        else if (paused && pausePressed)
            Resume();


        timeSurvived += Time.deltaTime;
        counter -= Time.deltaTime;

        timeSurvivedUI.text = $"for {timeSurvived.ToString("#00.00")} seconds";

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
            PlayerLost(true);
            Debug.Log("YOU LOST!");
        }
    }

    public void ResetTimer()
    {
        counter = maxTimeNoTeleportation;
        teleportationSlider.normalizedValue = counter / maxTimeNoTeleportation;
    }

    public void PlayerLost(bool timeRunOut = false)
    {
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        paused = true;
        loseScreen.SetActive(true);
        loseScreen.transform.Find("Score").GetComponent<Text>().text = $"You survived for:\n{timeSurvived.ToString("#00.00")} seconds";
        if(timeRunOut)
        {
            loseScreen.transform.Find("YouLost").GetComponent<Text>().text = $"You haven't teleported for 10 seconds.\n   You lost.";
        }
    }

    public void Restart()
    {
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        pauseScreen.SetActive(false);
        paused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        pauseScreen.SetActive(true);
        paused = true;
    }

    public void StartGame()
    {
        paused = false;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        mainScreen.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
