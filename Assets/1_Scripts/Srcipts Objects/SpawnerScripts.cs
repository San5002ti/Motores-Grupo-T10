using System.Collections;
using UnityEngine;

public class SpawnerObjetos : MonoBehaviour
{
    [Header("ponga aqui los objetos que intanciara")]
    public GameObject prefabAInstanciar;
    public Transform[] puntosDeSpawn; 

    [Header("tipo entre apariciones")]
    public float tiempoEntreSpawns = 3f;
    public int limiteMaximo = 10;

    [Tooltip("detiene  generacion")]
    public bool limiteEsTotalHistorico = true;

    private int contadorTotalGenerado = 0;

    private void Start()
    {
        StartCoroutine(RutinaSpawn());
    }
    private IEnumerator RutinaSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreSpawns);

            if (PuedeGenerar())
            {
                InstanciarObjeto();
            }
        }
    }
    private bool PuedeGenerar()
    {
        if (prefabAInstanciar == null) return false;

        if (limiteEsTotalHistorico)
        {
            return contadorTotalGenerado < limiteMaximo;
        }
        else
        {
            int objetosActivos = GameObject.FindGameObjectsWithTag(prefabAInstanciar.tag).Length;
            return objetosActivos < limiteMaximo;
        }
    }
    private void InstanciarObjeto()
    {
        Vector3 posicion = transform.position;
        Quaternion rotacion = transform.rotation;
        if (puntosDeSpawn != null && puntosDeSpawn.Length > 0)
        {
            int indiceAleatorio = Random.Range(0, puntosDeSpawn.Length);
            posicion = puntosDeSpawn[indiceAleatorio].position;
            rotacion = puntosDeSpawn[indiceAleatorio].rotation;
        }
        Instantiate(prefabAInstanciar, posicion, rotacion);
        contadorTotalGenerado++;
    }
}