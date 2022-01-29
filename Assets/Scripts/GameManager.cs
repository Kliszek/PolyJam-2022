using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public float levelRadius;
    public static GameManager instance;

    public PlayerController playerInstance;
    public float score;

    public Transform levelLeft;
    public Transform levelRight;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
            instance = this;

        playerInstance = GameObject.Find("Player").GetComponent<PlayerController>();
        if (!playerInstance)
            Debug.LogError("ERROR! No player object found!!");

        score = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
