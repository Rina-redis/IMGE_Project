using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerController_ThirdPerson_Climbing pm;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    public float wallLookAngle;

    private RaycastHit frontWallHit;
    public bool wallFront;

    Vector3 upVektor = new Vector3(0, 0.1f, 0);
    Vector3 move = Vector3.zero;

    private void Update()
    {
        WallCheck();
        StateMachine();
        Debug.DrawRay(orientation.position, orientation.forward * detectionLength, Color.red);

        if (climbing) ClimbingMovement(); //&& !exitingWall
    }

    private void StateMachine()
    {
        // State 1 - Climbing
        if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)  //&& !exitingWall) 
        {
            Debug.Log("Cliiiimb");
            if (!climbing && climbTimer > 0) StartClimbing();

            // Timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        }
        // State 3 - None
        else
        {
            if (climbing) StopClimbing();
        }

        // if (wallFront && Input.GetKeyDown(jumpKey) && climbJumpsLeft > 0) ClimbJump();
    }

    private void WallCheck()
    {                                   //transform
        wallFront = Physics.SphereCast(orientation.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);

        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if (pm.grounded)
        {
            climbTimer = maxClimbTime;
        }
    }

    private void StartClimbing()
    {
        climbing = true;
        move = Vector3.zero;
    }

    private void ClimbingMovement()
    {

        move += upVektor;
        pm.controller.Move(move * Time.deltaTime * climbSpeed);

        Debug.LogWarning("dddddddddddddd");

    }

    private void StopClimbing()
    {
        climbing = false;
        pm.jump();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Calculate the start and end positions of the sphere cast
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + orientation.forward * detectionLength;

        // Draw the starting and ending spheres
        Gizmos.DrawWireSphere(startPosition, sphereCastRadius);
        Gizmos.DrawWireSphere(endPosition, sphereCastRadius);

        // Calculate directions perpendicular to the cast direction
        Vector3 up = orientation.up * sphereCastRadius;
        Vector3 right = orientation.right * sphereCastRadius;

        // Draw lines between the spheres to represent the sides of the capsule
        Gizmos.DrawLine(startPosition + up, endPosition + up);
        Gizmos.DrawLine(startPosition - up, endPosition - up);
        Gizmos.DrawLine(startPosition + right, endPosition + right);
        Gizmos.DrawLine(startPosition - right, endPosition - right);

        // Optionally, draw more lines for a better approximation
        Vector3 upRight = (up + right).normalized * sphereCastRadius;
        Vector3 upLeft = (up - right).normalized * sphereCastRadius;
        Vector3 downRight = (-up + right).normalized * sphereCastRadius;
        Vector3 downLeft = (-up - right).normalized * sphereCastRadius;

        Gizmos.DrawLine(startPosition + upRight, endPosition + upRight);
        Gizmos.DrawLine(startPosition + upLeft, endPosition + upLeft);
        Gizmos.DrawLine(startPosition + downRight, endPosition + downRight);
        Gizmos.DrawLine(startPosition + downLeft, endPosition + downLeft);
    }
}
