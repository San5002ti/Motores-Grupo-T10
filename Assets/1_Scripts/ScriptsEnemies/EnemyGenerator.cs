using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [Header("Tiempo de aparicion Enemigos")]
    public float TiempoAparicion;
    [Header("Cuantos Enemigos quieres que aparescan")]
    public int cantidad;

    public enum Identificador {enemigoA,enemigoB,enemigoC }


    [System.Serializable]
    public class Enemys
    {
        public Identificador enemigo;
        public GameObject PrefabGameobject;
    }

    [Header("Lista de enemigos")]
    public List<Enemys> enemigosD;

    private bool bandera = false;

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Spawneando...");
        if (bandera)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(spawn());
        }
    }

    private IEnumerator spawn() 
    {
        bandera = true;

        for (int i = 0; i < cantidad; i++)
        {
            int randomI = Random.Range(0, enemigosD.Count);
            Enemys enemigoSpawnear = enemigosD[randomI];
            Instantiate(enemigoSpawnear.PrefabGameobject, transform.position, transform.rotation);
            yield return new WaitForSeconds(TiempoAparicion);
        }
        bandera = false;
    }
}
