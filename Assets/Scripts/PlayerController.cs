using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputScript playerActionsControls;

    GameManager gameManager;

    public float movementSpeed;
    //private bool touchingGround;
    //public float gravityModifier;
    public float playerRadius;
    //public int levelUnitWidth;
    public GameObject particles;
    [Header("Teleport")]
    public float teleportCooldown;
    private float teleportCounter;
    bool canTeleport;

    [Header("Dash")]
    public float dashDuration;
    public float dashCooldown;
    public float dashModifier;
    private float dashCounter;
    bool canDash;

    float rotationSpeed;

    private void Awake()
    {
        playerActionsControls = new PlayerInputScript();
    }
    private void OnEnable()
    {
        playerActionsControls.Enable();
    }

    private void OnDisable()
    {
        playerActionsControls.Disable();
    }

    void Start()
    {
        gameManager = GameManager.instance;

        canDash = true;
        canTeleport = true;
        transform.parent.Find("Crosshair").gameObject.SetActive(false);
        rotationSpeed = movementSpeed * 90 /Mathf.PI*2;
    }

    void Update()
    {
        if (!canDash)
        {
            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0.0f)
            {
                dashCounter = 0.0f;
                canDash = true;
            }
        }
        if (!canTeleport)
        {
            teleportCounter -= Time.deltaTime;
            if (teleportCounter <= 0.0f)
            {
                teleportCounter = 0.0f;
                canTeleport = true;
            }
        }

        if(!gameManager.paused)
            PerformMovement();

        int swapInput = (int)playerActionsControls.Ground.SwapLevel.ReadValue<float>();

        if (!gameManager.paused && canTeleport && swapInput == 1)
            SwapLevel();
        /*
        if (swapInput == -1)
            SwapLevel(gameManager.levelLeft);
        else if (swapInput == 1)
            SwapLevel(gameManager.levelRight);
        */
    }

    public PlayerInputScript GetInputScript()
    {
        return playerActionsControls;
    }

    void SwapLevel(Transform level = null)
    {
        if (!level)
            level = transform.parent == gameManager.levelLeft ? gameManager.levelRight : gameManager.levelLeft;
        else if (transform.parent == level)
            return;
        transform.parent.Find("Crosshair").gameObject.SetActive(true);
        level.Find("Crosshair").gameObject.SetActive(false);
        Vector2 savedPosition = GetArrayIntPosition();
        if (particles)
            GameObject.Instantiate(particles, transform.position, transform.rotation);
        transform.parent = level;
        SetLocalPosition(savedPosition);
        if (particles)
            GameObject.Instantiate(particles, transform.position, transform.rotation);
        canTeleport = false;
        teleportCounter = teleportCooldown;
        gameManager.ResetTimer();
    }

    void PerformMovement()
    {
        float movementSideways = playerActionsControls.Ground.MoveHorizontally.ReadValue<float>();
        float movementUpwards = playerActionsControls.Ground.MoveVertically.ReadValue<float>();

        Vector3 currentPosition = transform.localPosition;
        Vector3 moveVector = Vector3.right * movementSideways + Vector3.forward * movementUpwards;

        if (moveVector != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVector);

            bool dashPressed = playerActionsControls.Ground.Dash.ReadValue<float>() == 1.0f;
            if (canDash && dashPressed)
            {
                canDash = false;
                dashCounter = dashCooldown;
                moveVector *= dashModifier;
            }
            else if (dashCounter >= dashCooldown - dashDuration)
            {
                moveVector *= dashModifier;
            }

            transform.GetChild(0).Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
        }

        moveVector *= Time.deltaTime * movementSpeed;

        float clampValue = gameManager.levelRadius - playerRadius;
        currentPosition.x = Mathf.Clamp(currentPosition.x + moveVector.x, -clampValue, clampValue);
        currentPosition.z = Mathf.Clamp(currentPosition.z + moveVector.z, -clampValue, clampValue);

        transform.localPosition = currentPosition;
    }

    Vector2 GetArrayPosition()
    {
        Vector2 position = new Vector2(transform.localPosition.x, transform.localPosition.z);
        position += Vector2.one * (gameManager.levelRadius - playerRadius);
        return position;
    }

    public Vector2 GetArrayIntPosition()
    {
        Vector2 floatPosition = GetArrayPosition();
        return new Vector2(Mathf.RoundToInt(floatPosition.x), Mathf.RoundToInt(floatPosition.y));
    }

    void SetLocalPosition(Vector2 position)
    {
        position -= Vector2.one * (gameManager.levelRadius - playerRadius);

        transform.localPosition = new Vector3(position.x, transform.localPosition.y, position.y);
    }
}
