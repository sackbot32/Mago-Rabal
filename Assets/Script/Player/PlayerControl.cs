using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField]
    private InputActionReference moveInput;
    [SerializeField]
    private InputActionReference jumpInput;
    [SerializeField]
    private InputActionReference lookInput;
    [SerializeField]
    private InputActionReference sprintInput;
    [SerializeField]
    private Slider sprintBar;
    private Rigidbody rb;
    private Transform cam;
    private LayerMask layer;
    private Transform feet;
    private Camera handCamera;
    //Settings
    [Header("Movement Settings")]
    public float addedSpeed;
    public float maxSpeed;
    public float runMultiplier;
    [Tooltip("when stopped on the ground, by what does the speed get divided by")]
    public float speedDivider;
    [Tooltip("when the air the added speed will be divided by this")]
    public float jumpSpeedDivider;
    [Header("Jump Settings")]
    public float jumpForce;
    public float detectLength;
    [Tooltip("when the air this will force will be also added downwards")]
    public float addedGravity;
    [Header("Cam Settings")]
    public float vSensitivy;
    public float hSensitivity;
    public float camLimit;
    [Header("Sprint Settings")]
    public float maxSprintValue;
    public float sprintDrainingAmmount;
    public float sprintDrainingRate;
    //Data
    private Vector2 moveDir;
    private Vector2 lookDir;
    private float horRot;
    private float verRot;
    private bool canJump;
    private float trueAddedSpeed;
    private float trueMaxSpeed;
    private float currentForwardSpeed;
    private float currentRightSpeed;
    //Vestigial/Deprecated utility for sprint,we may not use it
    private float currentSprintValue;
    private bool sprinting;
    private bool isDrainingSprint;
    private bool canSprint;
    private void Start()
    {
        handCamera = transform.GetChild(4).gameObject.GetComponent<Camera>();
        canSprint = true;
        sprinting = false;
        currentSprintValue = maxSprintValue;
        //sprintBar.maxValue = maxSprintValue;
        //sprintBar.value = currentSprintValue;
        layer = LayerMask.GetMask("Ground");
        canJump = false;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        feet = transform.GetChild(2).transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug teleport
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0,1,0);
            rb.linearVelocity = new Vector3(0,0,0);
        }
        moveDir = moveInput.action.ReadValue<Vector2>().normalized;
        lookDir = lookInput.action.ReadValue<Vector2>();
        DetectGround();
        if (jumpInput.action.WasPressedThisFrame() && canJump)
        {
            Jump();
        }
        sprinting = sprintInput.action.IsInProgress() && canSprint;
        if(sprinting && !isDrainingSprint && canSprint)
        {
            StartCoroutine(DrainWhileSprinting());
        }
        Look();

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        trueAddedSpeed = sprinting ? addedSpeed * runMultiplier: addedSpeed;
        trueAddedSpeed = trueAddedSpeed * rb.mass;
        trueMaxSpeed = sprinting ? maxSpeed * runMultiplier: maxSpeed;
        if (!canJump)
        {
            rb.AddForce(-transform.up * addedGravity);
            trueAddedSpeed /= jumpSpeedDivider;
            trueMaxSpeed /= jumpSpeedDivider;
        }
        currentForwardSpeed = Vector3.Dot(transform.forward, rb.linearVelocity);
        currentRightSpeed = Vector3.Dot(transform.right, rb.linearVelocity);

        //A little bit of a trick to make the movement "snapier" will be to allow for sudden changes of direction
        //Forward
        if (currentForwardSpeed > 1 && moveDir.y < 0)
        {
            //print("Snap forward negative");
            rb.AddForce(transform.forward * -trueAddedSpeed * trueMaxSpeed / 4);
        }
        if (currentForwardSpeed < -1 && moveDir.y > 0)
        {
            //print("Snap forward pos");
            rb.AddForce(transform.forward * trueAddedSpeed * trueMaxSpeed / 4);
        }
        //Right
        if (currentRightSpeed > 1 && moveDir.x < 0)
        {
            //print("Snap right negative");
            rb.AddForce(transform.right * -trueAddedSpeed * trueMaxSpeed / 4);
        }
        if (currentRightSpeed < -1 && moveDir.x > 0)
        {
            //print("Snap right pos");
            rb.AddForce(transform.right * trueAddedSpeed * trueMaxSpeed / 4);
        }

        //We check that the speed doesn't go over the maxSpeed
        if (Mathf.Abs( currentForwardSpeed) < trueMaxSpeed)
        {
            rb.AddForce(transform.forward * moveDir.normalized.y * trueAddedSpeed);
        } else
        {
            //print("Max forward speed achieved");
        }
        if (Mathf.Abs(currentRightSpeed) < trueMaxSpeed)
        {
            rb.AddForce(transform.right * moveDir.normalized.x * trueAddedSpeed);
        }
        else
        {
            //print("Max right speed achieved");
        }

        //If it is on the ground it should lose speed if the input is none
        if (canJump && speedDivider != 0)
        {
            float newFValue = currentForwardSpeed;
            float newRValue = currentRightSpeed;
            if(moveDir.y == 0)
            {
                newFValue = currentForwardSpeed / speedDivider;
            }
            if(moveDir.x == 0)
            {
                newRValue = currentRightSpeed / speedDivider;
            }
            rb.linearVelocity = transform.forward * newFValue + transform.right * newRValue + transform.up*rb.linearVelocity.y;
        }
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce * rb.mass);
        
    }

    private void Look()
    {
        horRot = lookInput.action.ReadValue<Vector2>().x * hSensitivity;
        verRot += -lookInput.action.ReadValue<Vector2>().y * vSensitivy;

        verRot = Mathf.Clamp(verRot, -camLimit, camLimit);

        transform.Rotate(Vector3.up * horRot);
        cam.localRotation = Quaternion.Euler(verRot, 0f, 0f);
        handCamera.transform.localRotation = Quaternion.Euler(verRot, 0f, 0f);
    }

    public bool DetectGround()
    {
        Debug.DrawRay(feet.position, Vector3.down * detectLength,Color.red);
        if (Physics.Raycast(feet.position,Vector3.down,detectLength,layer))
        {
            //This is done to avoid losing speed when hitting the ground
            if (!canJump)
            {
                rb.linearVelocity = (transform.forward * currentForwardSpeed + transform.right * currentRightSpeed + transform.up* rb.linearVelocity.y);
            }
            canJump = true;
        } else
        {
            canJump = false;
        }
        return canJump;
    }

    private IEnumerator DrainWhileSprinting()
    {
        isDrainingSprint = true;
        while(sprinting && (currentSprintValue > 0))
        {
            currentSprintValue -= sprintDrainingAmmount;
            //sprintBar.value = currentSprintValue;
            yield return new WaitForSeconds(sprintDrainingRate);
        }
        
        if(currentSprintValue > 0)
        {
            StartCoroutine(NormalSprintRegen());
        } else
        {
            StartCoroutine(EmptySprintRegen());
        }
    }

    private IEnumerator NormalSprintRegen()
    {
        isDrainingSprint = false;
        //print("Con estamina");
        while (!sprinting && (currentSprintValue < maxSprintValue))
        {
            currentSprintValue += sprintDrainingAmmount;
            //sprintBar.value = currentSprintValue;
            yield return new WaitForSeconds(sprintDrainingRate);
        }
        if(currentSprintValue > maxSprintValue)
        {
            currentSprintValue = maxSprintValue;
        }
    }

    private IEnumerator EmptySprintRegen()
    {
        canSprint = false;
        //print("Sin estamina");
        while ((currentSprintValue < maxSprintValue))
        {
            currentSprintValue += sprintDrainingAmmount/2;
            //sprintBar.value = currentSprintValue;
            yield return new WaitForSeconds(sprintDrainingRate);
        }
        if (currentSprintValue > maxSprintValue)
        {
            currentSprintValue = maxSprintValue;
        }
        isDrainingSprint = false;
        canSprint = true;
    }


}
