using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Menu
{
    internal class MenuManager : MonoBehaviour
    {
        public GameObject Panel_Opciones;
        public GameObject Letras;
        public Animator animator_Panel;
        public Animator animator_Letras;
        [SerializeField] private float duracionAnimacionSalir;
        public void Play()
        {
            SceneManager.LoadScene(1);
            Debug.Log("Jugando............");
        }

        public void Settings()
        {
            Panel_Opciones.SetActive(true);
            animator_Panel.SetBool("Entrar", true);
            animator_Letras.SetBool("MoverDerecha", true);
        }
        public void QuitSettings()
        {
            StartCoroutine(QuitRoutine());
            Debug.Log("Cerrando....");
        }
        private IEnumerator QuitRoutine()
        {
            animator_Panel.SetBool("Entrar", false);
            animator_Letras.SetBool("MoverDerecha", false);
            animator_Panel.SetBool("Salir", true);
            yield return new WaitForSeconds(duracionAnimacionSalir);
            animator_Letras.SetBool("MoverIzquierda", true);
        }
        


        public void Quit()
        {
            Debug.Log("Saliendo...........");
            Application.Quit();
        }
    }
}
