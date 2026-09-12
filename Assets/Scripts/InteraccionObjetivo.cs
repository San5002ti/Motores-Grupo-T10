using UnityEngine;

public class InteraccionObjetivo : MonoBehaviour
{
    [Header("Configuración del Raycast")]
    public float distanciaInteraccion = 3f;
    public Transform puntoDisparo; // Si es null, usa el propio objeto

    [Header("UI")]
    public GameObject panelVictoria; // Panel/Canvas del mensaje de victoria

    void Start()
    {
        // Oculta la ventana de victoria al iniciar
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(false);
        }
    }

    void Update()
    {
        // Define origen y dirección (desde la cámara o el objeto actual)
        Vector3 origen = (puntoDisparo != null) ? puntoDisparo.position : transform.position;
        Vector3 direccion = (puntoDisparo != null) ? puntoDisparo.forward : transform.forward;

        RaycastHit hit;

        // Comprueba si el raycast impacta contra algo dentro del rango
        if (Physics.Raycast(origen, direccion, out hit, distanciaInteraccion))
        {
            // Verifica que tenga el Tag correspondiente
            if (hit.collider.CompareTag("objetivo"))
            {
                // Detecta si se presiona la tecla 'E'
                if (Input.GetKeyDown(KeyCode.E))
                {
                    MostrarVictoria();
                }
            }
        }

        // Línea de depuración en la escena (verde si está listo para probar)
        Debug.DrawRay(origen, direccion * distanciaInteraccion, Color.green);
    }

    void MostrarVictoria()
    {
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
            // Opcional: pausar el juego al ganar
            // Time.timeScale = 0f;
        }
    }
}