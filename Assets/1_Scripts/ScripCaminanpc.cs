using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class ControladorAnimacionesNPC : MonoBehaviour
{
    private NavMeshAgent agente;
    private Animator animador;
    public enum EstadoAnimacion
    {
        Quieto,
        Caminando,
        Corriendo,
        Atacando
    }
    [SerializeField] private EstadoAnimacion estadoVisualActual = EstadoAnimacion.Quieto;
    public string animacionQuieto = "Idle";
    public string animacionCaminando = "Walk";
    public string animacionCorriendo = "Run";
    public string animacionAtacando = "Attack";
    public float umbralCaminar = 0.1f;
    public float umbralCorrer = 2.5f;
    public float tiempoTransicion = 0.2f;
    private string ultimaAnimacionReproducida = "";
    private bool estaAtacandoFrenado = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animador = GetComponentInChildren<Animator>();
        CambiarAnimacion(animacionQuieto, EstadoAnimacion.Quieto);
    }
    void Update()
    {
        if (agente == null || animador == null) return;
        if (estaAtacandoFrenado)
        {
            CambiarAnimacion(animacionAtacando, EstadoAnimacion.Atacando);
            return;
        }
        Vector3 velocidadHorizontal = new Vector3(agente.velocity.x, 0f, agente.velocity.z);
        float velocidadActual = velocidadHorizontal.magnitude;
        bool tieneDestinoActivo = agente.hasPath && !agente.isStopped;
        bool estaCercaDeDetenerse = agente.remainingDistance <= agente.stoppingDistance && !agente.pathPending;

        if (velocidadActual > umbralCaminar && tieneDestinoActivo && !estaCercaDeDetenerse)
        {
            if (velocidadActual >= umbralCorrer)
            {
                CambiarAnimacion(animacionCorriendo, EstadoAnimacion.Corriendo);
            }
            else
            {
                CambiarAnimacion(animacionCaminando, EstadoAnimacion.Caminando);
            }
        }
        else
        {
            CambiarAnimacion(animacionQuieto, EstadoAnimacion.Quieto);
        }
    }
    private void CambiarAnimacion(string nombreAnimacion, EstadoAnimacion nuevoEstado)
    {
        if (string.IsNullOrEmpty(nombreAnimacion)) return;
        if (ultimaAnimacionReproducida == nombreAnimacion) return;
        animador.CrossFadeInFixedTime(nombreAnimacion, tiempoTransicion);

        ultimaAnimacionReproducida = nombreAnimacion;
        estadoVisualActual = nuevoEstado;
    }
    public void EstablecerAtacando(bool atacando)
    {
        estaAtacandoFrenado = atacando;
    }
    public EstadoAnimacion ObtenerEstadoActual()
    {
        return estadoVisualActual;
    }
}