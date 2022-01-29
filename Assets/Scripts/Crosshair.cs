using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public PlayerController player;
    Transform vertical;
    Transform horizontal;
    void Start()
    {
        transform.localPosition = Vector3.zero;
        vertical = transform.Find("Vertical");
        horizontal = transform.Find("Horizontal");
    }
    void Update()
    {
        Vector2 pos = player.GetArrayIntPosition();
        pos -= Vector2.one * (player.levelRadius-player.playerRadius);
        horizontal.localPosition = Vector3.forward * pos.y;
        vertical.localPosition = Vector3.right * pos.x;
    }
}
