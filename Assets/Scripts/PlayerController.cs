using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputScript playerActionsControls;
    public float movementSpeed;
    private bool touchingGround;
    public float gravityModifier;

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
        
    }

    void Update()
    {
        float movementSideways = playerActionsControls.Ground.MoveHorizontally.ReadValue<float>();
        float movementUpwards = playerActionsControls.Ground.MoveVertically.ReadValue<float>();


        Vector3 currentPosition = transform.position;
        Vector3 moveVector = Vector3.right * movementSideways + Vector3.forward * movementUpwards;
        moveVector.Normalize();

        transform.Translate(moveVector*Time.deltaTime*movementSpeed);
    }


}
