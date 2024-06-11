using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    
    #region MovementParameters
    [SerializeField] private float minSpeed = 5.0f;
    [SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float acceleration = 0.1f;
    #endregion

    #region AirMovementParameters
    [SerializeField] private float airMovementFactor = 0.25f;

    [SerializeField] private float airMovementScaling = 0.05f;

    [SerializeField] private float minAirSpeed = 0.0f;

    [SerializeField] private float maxAirSpeed = 7.0f;

    [SerializeField] private float airAcceleration = 0.1f;
    #endregion


    #region JumpingParameters
    private float jumpPower = 4.0f;

    private float jumpScaling = 0.25f;

    private float playerDistanceToGround = 1.0f;

    #endregion


    #region Variables
    private Vector3 movementVector;

    private Vector3 prevMovementVector;

    private Vector3 airMovementVector;

    private Vector3 jumpMovementVector;

    private Rigidbody rb;
    public bool grounded = false;

    private float currSpeed = 0.0f;

    private float currAirSpeed = 0.0f;

    private float currAirMovementSpeed = 0.0f;

    private float currAirMovementScaling = 0.0f;

    #endregion

    void Start() {
        currSpeed = minSpeed;
        prevMovementVector = Vector3.zero;
        movementVector = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CheckForGround());
    }

    void FixedUpdate() {
        if (grounded) {
            Move();
        }
        else {
            AirMove();
        }
    }

    
    #region Movement
    void Move() {
        if (grounded && movementVector != Vector3.zero) {
            transform.position += movementVector * currSpeed * Time.deltaTime;
            StartCoroutine(Acceleration());
        }
    }

    void StopMoving() {
        StopCoroutine(Acceleration());
        currSpeed = minSpeed;
        StopCoroutine(AirAcceleration());
        currAirMovementSpeed = minAirSpeed;
    }

    void OnMovement(InputValue value) {
        Vector2 currInput = value.Get<Vector2>();
        prevMovementVector = movementVector;
        movementVector = new Vector3(currInput.x, 0, currInput.y);

        if (movementVector == Vector3.zero || movementVector.x - prevMovementVector.x == 0 || movementVector.z - prevMovementVector.z == 0) {
            StopMoving();
        }

        airMovementVector = movementVector * airMovementFactor;
    }

    IEnumerator Acceleration() {
        while (currSpeed < maxSpeed) {
            if (grounded) {
                currSpeed += acceleration * Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region AirMovement

    void AirMove() {
        transform.position += (jumpMovementVector + (airMovementVector * (airMovementScaling * currAirSpeed) * currAirMovementSpeed)) * currAirSpeed * Time.deltaTime;
        StartCoroutine(AirAcceleration());
    }

    IEnumerator AirAcceleration() {
        while (currAirMovementSpeed < maxAirSpeed) {
            currAirMovementSpeed += airAcceleration * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion


    #region Jumping
    void OnJump() {
        if (!grounded) {
            return;
        }
        float currJumpPower = jumpPower + (jumpScaling * (currSpeed - minSpeed));
        rb.AddForce(Vector3.up * currJumpPower, ForceMode.Impulse);
    }

    void UpdateJumpVector() {
        jumpMovementVector = movementVector;
        currAirSpeed = currSpeed;
        currAirMovementScaling = currSpeed;
        currAirMovementSpeed = minAirSpeed;
    }

    IEnumerator CheckForGround() {
        RaycastHit hit;

        while (true) {
            bool raycastSuccess = Physics.Raycast(transform.position, transform.up * -1, out hit);
            if (raycastSuccess && hit.collider.gameObject.CompareTag("Ground") && hit.distance <= playerDistanceToGround) {
                grounded = true;
            }
            else {
                if (grounded) {
                    UpdateJumpVector();
                }
                grounded = false;
            }
            yield return null;
        }
    }
    #endregion
    

}

