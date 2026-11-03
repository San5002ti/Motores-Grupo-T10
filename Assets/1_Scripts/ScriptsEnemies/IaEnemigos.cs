using UnityEngine;
using UnityEngine.AI; 

public class IAEnemigo : MonoBehaviour
{
    [Header("referencia jugador y patrullas")]
    public Transform jugador;
    public Transform[] puntosDePatrulla; 
    private NavMeshAgent agente;

    [Header("rangos")]
    public float distanciaVision = 10f;  
    public float distanciaAtaque = 2f;   
    private enum Estado { Patrullar, Perseguir, Atacar }
    private Estado estadoActual = Estado.Patrullar;

    private int indicePatrullaActual = 0;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        if (puntosDePatrulla.Length > 0)
        {
            agente.SetDestination(puntosDePatrulla[indicePatrullaActual].position);
        }
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

    void ComportamientoPatrullar(float distanciaAlJugador)
    {
        if (!agente.pathPending && agente.remainingDistance < 0.5f)
        {
            if (puntosDePatrulla.Length == 0) return;

            indicePatrullaActual = (indicePatrullaActual + 1) % puntosDePatrulla.Length;
            agente.SetDestination(puntosDePatrulla[indicePatrullaActual].position);
        }
        if (distanciaAlJugador <= distanciaVision)
        {
            estadoActual = Estado.Perseguir;
        }
    }

    void ComportamientoPerseguir(float distanciaAlJugador)
    {
        agente.SetDestination(jugador.position);
        if (distanciaAlJugador > distanciaVision)
        {
            estadoActual = Estado.Patrullar;
            agente.SetDestination(puntosDePatrulla[indicePatrullaActual].position);
        }
        else if (distanciaAlJugador <= distanciaAtaque)
        {
            estadoActual = Estado.Atacar;
        }
    }
    //Esta logica de aca es si van a poner ataques de enemigos
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
    }
}