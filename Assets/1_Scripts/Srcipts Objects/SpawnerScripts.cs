using System.Collections;
using UnityEngine;

public class SpawnerObjetos : MonoBehaviour
{
    public GameObject[] prefabsAInstanciar;
    public Transform[] puntosDeSpawn;
    public float tiempoEntreSpawns = 3f;
    public int limiteMaximo = 10;
    public bool limiteEsTotalHistorico = true;
    public string tagParaContar = "Npc";

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
        if (prefabsAInstanciar == null || prefabsAInstanciar.Length == 0) return false;

        if (limiteEsTotalHistorico)
        {
            return contadorTotalGenerado < limiteMaximo;
        }
        else
        {
            int objetosActivos = GameObject.FindGameObjectsWithTag(tagParaContar).Length;
            return objetosActivos < limiteMaximo;
        }
    }

    private void InstanciarObjeto()
    {
        int indicePrefabAleatorio = Random.Range(0, prefabsAInstanciar.Length);
        GameObject prefabElegido = prefabsAInstanciar[indicePrefabAleatorio];

        if (prefabElegido == null) return;
        Vector3 posicion = transform.position;
        Quaternion rotacion = transform.rotation;

        if (puntosDeSpawn != null && puntosDeSpawn.Length > 0)
        {
            int indicePuntoAleatorio = Random.Range(0, puntosDeSpawn.Length);
            if (puntosDeSpawn[indicePuntoAleatorio] != null)
            {
                posicion = puntosDeSpawn[indicePuntoAleatorio].position;
                rotacion = puntosDeSpawn[indicePuntoAleatorio].rotation;
            }
        }
        Instantiate(prefabElegido, posicion, rotacion);
        contadorTotalGenerado++;
    }
}