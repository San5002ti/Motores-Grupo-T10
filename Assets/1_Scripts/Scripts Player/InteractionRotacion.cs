using UnityEngine;
using System.Collections;

public class InteractionRotacion : MonoBehaviour
{
    [Header("Configuración del Objeto que Rota")]
    public Transform objetoRotatorio;
    public Vector3 gradosDeRotacion = new Vector3(0, 90, 0);

    [Tooltip("Cuánto tiempo (en segundos) tarda en hacer el giro completo")]
    public float duracionRotacion = 1.0f;

    [Tooltip("Cuánto tiempo se queda quieto antes de volver")]
    public float tiempoDeEspera = 2.0f;

    [Header("Configuración de Audio")]
    [Tooltip("AudioSource que reproducirá el sonido. Si se deja vacío, se buscará automáticamente en este GameObject.")]
    public AudioSource audioSource;
    [Tooltip("Efecto de sonido que se reproducirá al iniciar la interacción.")]
    public AudioClip sonidoInteraccion;

    private Quaternion rotacionOriginalObjeto;
    private bool estaInteractuando = false;

    void Start()
    {
        if (objetoRotatorio == null)
        {
            objetoRotatorio = this.transform;
        }

        rotacionOriginalObjeto = objetoRotatorio.rotation;

        // Si no asignaste un AudioSource en el Inspector, intentamos obtenerlo del GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void ActivarRotacion()
    {
        if (!estaInteractuando)
        {
            StartCoroutine(SecuenciaDeRotacionSuave());
        }
    }

    IEnumerator SecuenciaDeRotacionSuave()
    {
        estaInteractuando = true;

        // --- PASO 0: Reproducir el efecto de sonido ---
        if (audioSource != null && sonidoInteraccion != null)
        {
            audioSource.PlayOneShot(sonidoInteraccion);
        }

        // --- PASO 1: Calcular cuál será la rotación final ---
        Quaternion rotacionInicial = objetoRotatorio.rotation;
        Quaternion rotacionDestino = rotacionInicial * Quaternion.Euler(gradosDeRotacion);

        float tiempoTranscurrido = 0f;

        // --- PASO 2: Rotar suavemente hacia el destino ---
        while (tiempoTranscurrido < duracionRotacion)
        {
            objetoRotatorio.rotation = Quaternion.Slerp(rotacionInicial, rotacionDestino, tiempoTranscurrido / duracionRotacion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        objetoRotatorio.rotation = rotacionDestino;

        // --- PASO 3: Esperar el tiempo definido en su nueva posición ---
        yield return new WaitForSeconds(tiempoDeEspera);

        // --- PASO 4: Volver a la rotación original suavemente ---
        tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionRotacion)
        {
            objetoRotatorio.rotation = Quaternion.Slerp(rotacionDestino, rotacionOriginalObjeto, tiempoTranscurrido / duracionRotacion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        objetoRotatorio.rotation = rotacionOriginalObjeto;

        // Liberamos el estado para permitir volver a interactuar
        estaInteractuando = false;
    }
}