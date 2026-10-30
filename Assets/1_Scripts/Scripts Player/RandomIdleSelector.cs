using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class RandomIdleSelector : MonoBehaviour
{
    [Header("Tiempos de Espera")]
    [SerializeField] private float tiempoMinimoEntreIdles = 5f; // Tiempo mínimo en el Idle principal antes de repetir
    [SerializeField] private float tiempoMaximoEntreIdles = 10f; // Tiempo máximo

    [Header("Configuración de Animaciones")]
    [SerializeField] private int totalIdles = 4; // Cantidad de idles alternativos

    private Animator animator;
    private Coroutine idleRoutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        idleRoutine = StartCoroutine(CambiarIdleAleatorioRoutine());
    }

    private void OnDisable()
    {
        if (idleRoutine != null)
        {
            StopCoroutine(idleRoutine);
        }
    }

    private IEnumerator CambiarIdleAleatorioRoutine()
    {
        while (true)
        {
            // 1. Espera un tiempo largo estando en el Idle principal
            float tiempoEspera = Random.Range(tiempoMinimoEntreIdles, tiempoMaximoEntreIdles);
            yield return new WaitForSeconds(tiempoEspera);

            // 2. Elige un número aleatorio para el idle alternativo
            int idleIndex = Random.Range(0, totalIdles);

            // 3. Envía el número al Animator
            animator.SetInteger("IdleIndex", idleIndex);

            // 4. Reseteamos el parámetro un frame después para evitar bloqueos
            yield return null;
            animator.SetInteger("IdleIndex", -1);

            // 5. IMPORTANTE: Esperamos a que la animación alternativa termine 
            // (Asegúrate de ajustar este tiempo según lo que duren tus animaciones, ej: 3 o 4 segundos)
            yield return new WaitForSeconds(4f);
        }
    }
}