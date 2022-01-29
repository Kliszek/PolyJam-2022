using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputScript playerActionsControls;
    public float movementSpeed;
    //private bool touchingGround;
    //public float gravityModifier;
    public float levelRadius;
    public float playerRadius;
    //public int levelUnitWidth;
    public Transform levelLeft;
    public Transform levelRight;
    public GameObject particles;

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
        transform.parent.Find("Crosshair").gameObject.SetActive(false);
        rotationSpeed = movementSpeed * 90 /Mathf.PI*2;
    }

    void Update()
    {
        PerformMovement();

        int swapInput = (int)playerActionsControls.Ground.SwapLevel.ReadValue<float>();

        if (swapInput == -1)
            SwapLevel(levelLeft);
        else if (swapInput == 1)
            SwapLevel(levelRight);
    }

    void SwapLevel(Transform level)
    {
        if (transform.parent == level)
            return;
        transform.parent.Find("Crosshair").gameObject.SetActive(true);
        level.Find("Crosshair").gameObject.SetActive(false);
        Vector2 savedPosition = GetArrayIntPosition();
        if (particles)
            GameObject.Instantiate(particles, transform.position, transform.rotation);
        transform.parent = level;
        SetLocalPosition(savedPosition);
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
            transform.GetChild(0).Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
        }

        moveVector *= Time.deltaTime * movementSpeed;

        currentPosition.x = Mathf.Clamp(currentPosition.x + moveVector.x, playerRadius - levelRadius, levelRadius - playerRadius);
        currentPosition.z = Mathf.Clamp(currentPosition.z + moveVector.z, playerRadius - levelRadius, levelRadius - playerRadius);

        transform.localPosition = currentPosition;
    }

    Vector2 GetArrayPosition()
    {
        Vector2 position = new Vector2(transform.localPosition.x, transform.localPosition.z);
        position += Vector2.one * (levelRadius - playerRadius);
        return position;
    }

    public Vector2 GetArrayIntPosition()
    {
        Vector2 floatPosition = GetArrayPosition();
        return new Vector2(Mathf.RoundToInt(floatPosition.x), Mathf.RoundToInt(floatPosition.y));
    }

    void SetLocalPosition(Vector2 position)
    {
        position -= Vector2.one * (levelRadius - playerRadius);

        transform.localPosition = new Vector3(position.x, transform.localPosition.y, position.y);
    }
}
