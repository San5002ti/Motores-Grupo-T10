using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Tiempo de aparicion Enemigos")]
        public float TiempoAparicion;
        [Header("Cuantos Enemigos quieres que aparescan")]
        public int cantidad;
        [Header("Ingrese Los bloques spawner")]
        public List<Transform> spawners;
        public enum Identificador { enemigoA, enemigoB, enemigoC }

        [System.Serializable]
        public class Enemys
        {
            public Identificador enemigo;
            public GameObject PrefabGameobject;
        }
        [Header("Lista de enemigos")]
        public List<Enemys> enemigosD;
        private bool bandera = false;
        public void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!bandera)
                {
                    Debug.Log("Iniciando oleada de enemigos...");
                    StartCoroutine(spawn());
                }
            }
        }
        private IEnumerator spawn()
        {
            bandera = true; 
            for (int i = 0; i < cantidad; i++)
            {
                int randomI = Random.Range(0, enemigosD.Count);
                int RandomSpawn = Random.Range(0, spawners.Count);
                Enemys enemigoSpawnear = enemigosD[randomI];
                Transform auxiliar = spawners[RandomSpawn];
                Instantiate(enemigoSpawnear.PrefabGameobject, auxiliar.position, auxiliar.rotation);
                yield return new WaitForSeconds(TiempoAparicion);
            }
            bandera = false; 
        }
    }
}