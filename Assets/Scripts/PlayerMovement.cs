using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Gravedad")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Cámara")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraTransform = mainCam.transform;
            }
            else
            {
                Debug.LogError("[PlayerMovement] No se encontró ninguna cámara con el Tag 'MainCamera'. Asigna una manualmente en el Inspector.");
            }
        }
    }

    private void Update()
    {
        Move();
        ApplyGravity();
    }

    private void Move()
    {
        if (cameraTransform == null)
            return;

        // Dirección frontal y lateral de la cámara proyectadas en el suelo (plano horizontal)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // W/S mueve adelante/atrás, A/D mueve izquierda/derecha
        Vector3 moveDirection = (forward * moveInput.y) + (right * moveInput.x);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        // Desplazamiento
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // El cuerpo siempre mira hacia el frente de la cámara, sin importar la tecla presionada
        if (forward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (controller.isGrounded &&
            Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    // Compatible con Player Input en modo "Send Messages"
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("[PlayerMovement] Recibiendo input (Send Messages): " + moveInput);
    }

    // Compatible con Player Input en modo "Invoke Unity Events" o llamadas directas
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}