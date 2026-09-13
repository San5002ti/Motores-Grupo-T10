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
            if (hit.collider.CompareTag("objetivo"))
            {
                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    MostrarVictoria();
                }
            }
        }
    }

    void MostrarVictoria()
    {
        if (ventanaVictoria != null)
        {
            ventanaVictoria.SetActive(true);

            // Desbloquea y hace visible el cursor:
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Opcional: Detener el tiempo del juego para que no sigan moviéndose
            // Time.timeScale = 0f;
        }
    }
}