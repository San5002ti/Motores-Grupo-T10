using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorBotonesCanvas : MonoBehaviour
{
    public void VolverEscenaAnterior()
    {
        SceneManager.LoadScene(1);
    }
    public void VolverAlMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}