using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class OclusionAudio : MonoBehaviour
{
    public Transform jugador;
    public LayerMask capaObstaculos;

    public float frecuenciaClara = 22000f; 
    public float frecuenciaAhogada = 1500f; 

    public float volumenNormal = 1f;
    public float volumenTapado = 0.5f;

    private AudioLowPassFilter filtro;
    private AudioSource fuenteAudio;

    void Start()
    {
        filtro = GetComponent<AudioLowPassFilter>();
        fuenteAudio = GetComponent<AudioSource>();
        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;
        Vector3 origen = transform.position + Vector3.up * 1.5f;
        Vector3 destinoJugador = jugador.position + Vector3.up * 1.5f;

        Vector3 direccion = destinoJugador - origen;
        float distanciaAlJugador = direccion.magnitude;
        Debug.DrawRay(origen, direccion, Color.red);

        if (Physics.Raycast(origen, direccion, out RaycastHit hit, distanciaAlJugador, capaObstaculos))
        {
            filtro.cutoffFrequency = Mathf.Lerp(filtro.cutoffFrequency, frecuenciaAhogada, Time.deltaTime * 5f);
            fuenteAudio.volume = Mathf.Lerp(fuenteAudio.volume, volumenTapado, Time.deltaTime * 5f);
        }
        else
        {
            filtro.cutoffFrequency = Mathf.Lerp(filtro.cutoffFrequency, frecuenciaClara, Time.deltaTime * 5f);
            fuenteAudio.volume = Mathf.Lerp(fuenteAudio.volume, volumenNormal, Time.deltaTime * 5f);
        }
    }
}
