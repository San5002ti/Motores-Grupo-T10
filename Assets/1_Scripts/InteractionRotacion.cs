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

    private Quaternion rotacionOriginalObjeto;
    private bool estaInteractuando = false;

    void Start()
    {
        if (objetoRotatorio == null)
        {
            objetoRotatorio = this.transform;
        }

        rotacionOriginalObjeto = objetoRotatorio.rotation;
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

        // --- PASO 1: Calcular cuál será la rotación final ---
        Quaternion rotacionInicial = objetoRotatorio.rotation;
        Quaternion rotacionDestino = rotacionInicial * Quaternion.Euler(gradosDeRotacion);

        float tiempoTranscurrido = 0f;

        // --- PASO 2: Rotar suavemente hacia el destino ---
        // Mientras el tiempo transcurrido sea menor a la duración que configuramos...
        while (tiempoTranscurrido < duracionRotacion)
        {
            // Slerp mezcla suavemente entre la rotación inicial y la final basado en el tiempo
            objetoRotatorio.rotation = Quaternion.Slerp(rotacionInicial, rotacionDestino, tiempoTranscurrido / duracionRotacion);

            // Sumamos el tiempo que tardó este frame
            tiempoTranscurrido += Time.deltaTime;

            // Esperamos al siguiente frame para continuar el bucle
            yield return null;
        }

        // Aseguramos que quede exactamente en la rotación de destino al terminar el bucle
        objetoRotatorio.rotation = rotacionDestino;

        // --- PASO 3: Esperar el tiempo definido en su nueva posición ---
        yield return new WaitForSeconds(tiempoDeEspera);

        // --- PASO 4: Volver a la rotación original suavemente ---
        tiempoTranscurrido = 0f; // Reiniciamos el contador de tiempo

        while (tiempoTranscurrido < duracionRotacion)
        {
            objetoRotatorio.rotation = Quaternion.Slerp(rotacionDestino, rotacionOriginalObjeto, tiempoTranscurrido / duracionRotacion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Aseguramos que quede exactamente en la rotación original
        objetoRotatorio.rotation = rotacionOriginalObjeto;

        // Liberamos el estado para permitir volver a interactuar
        estaInteractuando = false;
    }
}