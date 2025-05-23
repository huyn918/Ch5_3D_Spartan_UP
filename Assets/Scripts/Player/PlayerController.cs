using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration;
    public float maxSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;
    public int jumpStaminaUse;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    public bool canLook = true;

    public Rigidbody player_Rigidbody;
    public Collider player_Collider;
    public Action inventory;

    [SerializeField]
    private PhysicMaterial noFriction;
    [SerializeField]
    private PhysicMaterial defaultFrction;

    private void Awake()
    {
        player_Rigidbody = GetComponent<Rigidbody>();
        player_Collider = GetComponent<Collider>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        
      
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();

            player_Collider.material.staticFriction = 0f;
            player_Collider.material.dynamicFriction = 0f;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            player_Collider.material.staticFriction = 0.5f;
            player_Collider.material.dynamicFriction = 0.3f;
            //Debug.Log("입력 중지");
        }
    }
    
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            player_Rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            CharacterManager.Instance.Player.condition.UseStamina(jumpStaminaUse);
            Debug.Log("점프 입력됨");
        }
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    private void Move()
    {
        Vector3 moveDir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        moveDir.Normalize(); // 대각선 속도 균일화

        // 현재 수평 속도만 고려
        Vector3 horizontalVelocity = new Vector3(player_Rigidbody.velocity.x, 0, player_Rigidbody.velocity.z);
        
        player_Rigidbody.AddForce(moveDir * acceleration, ForceMode.Acceleration);
        // 수평속도가 최대치에 도달하면 최대속도보다 빨라지지 않도록 클램핑, 대신 y축 속도는 노터치
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            player_Rigidbody.velocity = new Vector3(horizontalVelocity.x, player_Rigidbody.velocity.y, horizontalVelocity.z);
        }
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        // 아래쪽으로 다리 4개를 뻗어둠 / rigidbody 바깥쪽으로 살짝 나오도록.
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.1f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.1f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.1f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.1f) + (transform.up * 0.1f), Vector3.down)
        };
        // 땅에 하나라도 닿으면 true 반환
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 1.5f, groundLayerMask))
            {
                Debug.Log("땅에 있음");
                return true;
            }
        }
        // 땅에 하나도 안 닿으면 false 반환
        Debug.Log("공중에 있음");
        return false;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }







}
