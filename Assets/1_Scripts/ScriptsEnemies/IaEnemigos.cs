using Interact;
using UnityEngine;
using UnityEngine.AI;

public class IAEnemigo : MonoBehaviour, IinteractuarDEnpc
{
    [Header("referencias")]
    public Transform jugador;
    private NavMeshAgent agente;

    [Header("controla los movimientos")]
    public float radioDePatrulla = 15f; 
    public float tiempoEsperaMin = 1f;  
    public float tiempoEsperaMax = 4f;  

    private float temporizadorEspera;
    private bool estaEsperando;

    [Header("Rangoo que el npc busca")]
    public float distanciaVision = 10f;
    public float distanciaAtaque = 2f;

    private enum Estado { Patrullar, Perseguir, Atacar }
    private Estado estadoActual = Estado.Patrullar;
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        BuscarNuevoPuntoAleatorio();
    }
    void Update()
    {
        if (jugador == null) return;

        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        switch (estadoActual)
        {
            case Estado.Patrullar:
                ComportamientoPatrullar(distanciaAlJugador);
                break;
            case Estado.Perseguir:
                ComportamientoPerseguir(distanciaAlJugador);
                break;
            case Estado.Atacar:
                ComportamientoAtacar(distanciaAlJugador);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        IinteractuarDEnpc interactuable = other.GetComponent<IinteractuarDEnpc>();

        if (interactuable != null)
        {
            interactuable.interactuarNPC();
        }
    }
    void ComportamientoPatrullar(float distanciaAlJugador)
    {
        if (distanciaAlJugador <= distanciaVision)
        {
            estaEsperando = false;
            estadoActual = Estado.Perseguir;
            return;
        }
        if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance)
        {
            if (!estaEsperando)
            {
                estaEsperando = true;
                temporizadorEspera = Random.Range(tiempoEsperaMin, tiempoEsperaMax);
            }
            else
            {
                temporizadorEspera -= Time.deltaTime;

                if (temporizadorEspera <= 0f)
                {
                    estaEsperando = false;
                    BuscarNuevoPuntoAleatorio();
                }
            }
        }
    }
    void BuscarNuevoPuntoAleatorio()
    {
        Vector3 direccionAleatoria = Random.insideUnitSphere * radioDePatrulla;
        direccionAleatoria += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(direccionAleatoria, out hit, radioDePatrulla, NavMesh.AllAreas))
        {
            agente.SetDestination(hit.position);
        }
    }
    void ComportamientoPerseguir(float distanciaAlJugador)
    {
        agente.isStopped = false;
        agente.SetDestination(jugador.position);

        if (distanciaAlJugador > distanciaVision)
        {
            estadoActual = Estado.Patrullar;
            BuscarNuevoPuntoAleatorio();
        }
        else if (distanciaAlJugador <= distanciaAtaque)
        {
            estadoActual = Estado.Atacar;
        }
    }

    void ComportamientoAtacar(float distanciaAlJugador)
    {
        agente.isStopped = true;
        Debug.Log("Ataca");

        if (distanciaAlJugador > distanciaAtaque)
        {
            agente.isStopped = false;
            estadoActual = Estado.Perseguir;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaVision);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radioDePatrulla);
    }
}