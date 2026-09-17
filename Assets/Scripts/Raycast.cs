using UnityEngine;
using UnityEngine.InputSystem;

public class Raycast : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaMaxima = 100f;
    public GameObject ventanaVictoria;

    void Start()
    {
        if (ventanaVictoria != null)
        {
            ventanaVictoria.SetActive(false);
        }

        // Opcional: Bloquea y oculta el cursor al iniciar el juego
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 origen = transform.position;
        Vector3 direccion = transform.forward;

        RaycastHit hit;
        Debug.DrawRay(origen, direccion * distanciaMaxima, Color.red);

        if (Physics.Raycast(origen, direccion, out hit, distanciaMaxima))
        {
            // Verificamos si se presionó la E este frame
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                // LÓGICA 1: Si es el objetivo de victoria
                if (hit.collider.CompareTag("objetivo"))
                {
                    MostrarVictoria();
                }

                // LÓGICA 2: Verificamos si el objeto chocado tiene el script de rotación
                // Usamos GetComponent para buscar el script en el objeto golpeado
                InteractionRotacion objetoRotatorio = hit.collider.GetComponent<InteractionRotacion>();

                // Si el objeto tiene el script adjunto, llamamos a la función
                if (objetoRotatorio != null)
                {
                    objetoRotatorio.ActivarRotacion();
                }
            }
        }
    }

    void MostrarVictoria()
    {
        if (ventanaVictoria != null)
        {
            ventanaVictoria.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void AceptarVictoria()
    {
        if (ventanaVictoria != null)
        {
            ventanaVictoria.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}