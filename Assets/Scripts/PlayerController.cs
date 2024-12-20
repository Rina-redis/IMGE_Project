using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference movementControlls;
    [SerializeField]
    private InputActionReference jumpControlls;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private Transform cameraMainTransform;

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float rotationSpeed = 2.0f;


    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]

    //private float gravityValue = -9.81f;

    public void OnEnable()
    {
        movementControlls.action.Enable();
        jumpControlls.action.Enable();
    }

    public void OnDisable()
    {
        movementControlls.action.Disable();
        jumpControlls.action.Disable();
    }


    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
       // var gravity = CustomGravityManager.GetGravity(transform.position);
       var gravity = new Vector3 (1,1,1);
        playerVelocity += gravity * Time.deltaTime;
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = movementControlls.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        
        // Makes the player jump
        if (jumpControlls.action.triggered && groundedPlayer)
        {
            Jump(gravity);
        }

        playerVelocity.y += gravity.magnitude * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void Jump(Vector3 gravity){
        float jumpSpeed = (float) Math.Sqrt(2f * gravity.magnitude * jumpHeight);
        Vector3 jumpDir = (transform.up).normalized;
        playerVelocity.y += Mathf.Sqrt(jumpHeight * 2.0f * gravity.magnitude);
    }

}
