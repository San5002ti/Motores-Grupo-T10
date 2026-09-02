using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float velocidadRotacion = 10f;

    [Header("Gravedad y salto")]
    [SerializeField] private float gravedad = -20f;
    [SerializeField] private float alturaSalto = 1.2f;

    [Header("Interacción")]
    [SerializeField] private float distanciaInteraccion = 2f;
    [SerializeField] private LayerMask capaInteractuable;

    [Header("Cámara")]
    [SerializeField] private Transform camara;

    private CharacterController characterController;

    private Vector2 movimientoInput;
    private float velocidadVertical;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        // Si no asignamos la cámara manualmente,
        // buscamos la cámara principal.
        if (camara == null && Camera.main != null)
        {
            camara = Camera.main.transform;
        }
    }

    private void Update()
    {
        MoverPersonaje();
        AplicarGravedad();
    }

    // =========================
    // MOVIMIENTO
    // =========================

    private void MoverPersonaje()
    {
        if (camara == null)
            return;

        // Dirección de la cámara
        Vector3 adelante = camara.forward;
        Vector3 derecha = camara.right;

        // Evitamos que la inclinación vertical de la cámara
        // afecte al movimiento.
        adelante.y = 0f;
        derecha.y = 0f;

        adelante.Normalize();
        derecha.Normalize();

        // Movimiento relativo a la cámara
        Vector3 direccion = adelante * movimientoInput.y +
                            derecha * movimientoInput.x;

        if (direccion.magnitude > 1f)
        {
            direccion.Normalize();
        }

        // Mover al personaje
        characterController.Move(direccion * velocidad * Time.deltaTime);

        // Rotar el personaje hacia donde se mueve
        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo =
                Quaternion.LookRotation(direccion);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionObjetivo,
                velocidadRotacion * Time.deltaTime
            );
        }
    }

    // =========================
    // GRAVEDAD
    // =========================

    private void AplicarGravedad()
    {
        if (characterController.isGrounded && velocidadVertical < 0)
        {
            velocidadVertical = -2f;
        }

        velocidadVertical += gravedad * Time.deltaTime;

        Vector3 movimientoVertical =
            Vector3.up * velocidadVertical;

        characterController.Move(
            movimientoVertical * Time.deltaTime
        );
    }

    // =========================
    // INPUT - MOVIMIENTO
    // =========================

    public void OnMove(InputAction.CallbackContext context)
    {
        movimientoInput = context.ReadValue<Vector2>();
    }

    // =========================
    // INPUT - SALTO
    // =========================

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && characterController.isGrounded)
        {
            velocidadVertical =
                Mathf.Sqrt(alturaSalto * -2f * gravedad);
        }
    }

    // =========================
    // INPUT - INTERACTUAR
    // =========================

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Interactuar();
        }
    }

    private void Interactuar()
    {
        Vector3 origen = transform.position + Vector3.up;

        Vector3 direccion = transform.forward;

        if (Physics.Raycast(
            origen,
            direccion,
            out RaycastHit hit,
            distanciaInteraccion,
            capaInteractuable))
        {
            Debug.Log("Interactuando con: " + hit.collider.name);

            // Buscamos un componente Interactable
            Interactable objeto =
                hit.collider.GetComponent<Interactable>();

            if (objeto != null)
            {
                objeto.Interactuar();
            }
        }
    }

    // =========================
    // RAYO DE INTERACCIÓN
    // =========================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 origen = transform.position + Vector3.up;

        Gizmos.DrawRay(
            origen,
            transform.forward * distanciaInteraccion
        );
    }
}