using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    GameManager gameManager;
    Transform vertical;
    Transform horizontal;
    void Start()
    {
        gameManager = GameManager.instance;
        transform.localPosition = Vector3.zero;
        vertical = transform.Find("Vertical");
        horizontal = transform.Find("Horizontal");
    }
    void Update()
    {
        Vector2 pos = gameManager.playerInstance.GetArrayIntPosition();
        pos -= Vector2.one * (gameManager.levelRadius - gameManager.playerInstance.playerRadius);
        horizontal.localPosition = Vector3.forward * pos.y;
        vertical.localPosition = Vector3.right * pos.x;
    }
}
