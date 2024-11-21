using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //Component in mage
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
    //Settings
    public float speed;
    public float runMultiplier;
    public float jumpForce;
    public float vSensitivy;
    public float hSensitivity;
    public float camLimit;
    public float detectLength;
    public float maxSprintValue;
    public float sprintDrainingAmmount;
    public float sprintDrainingRate;
    //Data
    private Vector2 moveDir;
    private Vector2 lookDir;
    private float horRot;
    private float verRot;
    private bool canJump;
    private float trueSpeed;
    private float currentSprintValue;
    private bool sprinting;
    private bool isDrainingSprint;
    private bool canSprint;
    private void Start()
    {
        canSprint = true;
        sprinting = false;
        currentSprintValue = maxSprintValue;
        sprintBar.maxValue = maxSprintValue;
        sprintBar.value = currentSprintValue;
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
        trueSpeed = sprinting ? speed * runMultiplier: speed;
        rb.linearVelocity = (transform.forward * moveDir.y + transform.right*moveDir.x)*trueSpeed + transform.up*rb.linearVelocity.y;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce);
        
    }

    private void Look()
    {
        horRot = lookInput.action.ReadValue<Vector2>().x * hSensitivity;
        verRot += -lookInput.action.ReadValue<Vector2>().y * vSensitivy;

        verRot = Mathf.Clamp(verRot, -camLimit, camLimit);

        transform.Rotate(Vector3.up * horRot);
        cam.localRotation = Quaternion.Euler(verRot, 0f, 0f);
    }

    private void DetectGround()
    {
        Debug.DrawRay(feet.position, Vector3.down * detectLength,Color.red);
        if (Physics.Raycast(feet.position,Vector3.down,detectLength,layer))
        {
            canJump = true;
        } else
        {
            canJump = false;
        }
    }

    private IEnumerator DrainWhileSprinting()
    {
        isDrainingSprint = true;
        while(sprinting && (currentSprintValue > 0))
        {
            currentSprintValue -= sprintDrainingAmmount;
            sprintBar.value = currentSprintValue;
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
        print("Con estamina");
        while (!sprinting && (currentSprintValue < maxSprintValue))
        {
            currentSprintValue += sprintDrainingAmmount;
            sprintBar.value = currentSprintValue;
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
        print("Sin estamina");
        while ((currentSprintValue < maxSprintValue))
        {
            currentSprintValue += sprintDrainingAmmount/2;
            sprintBar.value = currentSprintValue;
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
