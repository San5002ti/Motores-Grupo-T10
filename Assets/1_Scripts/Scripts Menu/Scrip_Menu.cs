using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonitorInteractuable : MonoBehaviour
{
    [Header("Panel de Configuraciones")]
    public GameObject panelOpciones;
    [Header("Panel de datos")]
    public GameObject paneldata;
    public void IniciarJuego()
    {
        Debug.Log("Jugandoooooooooooo");
        SceneManager.LoadScene(1);
    }
    public void PanelOpciones()
    {
        panelOpciones.SetActive(!panelOpciones.activeSelf);
    }
    public void PanelData()
    {
        paneldata.SetActive(!paneldata.activeSelf);
    }
    public void salir()
    {
        Debug.Log("Saliendo....");
        Application.Quit();
    }
}