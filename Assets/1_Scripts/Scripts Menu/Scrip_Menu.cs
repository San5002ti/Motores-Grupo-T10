using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MonitorInteractuable : MonoBehaviour
{
    public GameObject pantallaDeCarga;
    public Slider barraDeProgreso;
    public TextMeshProUGUI textoProgreso;
    public float tiempoDeMas = 2f;
    [Header("Panel de Configuraciones")]
    public GameObject panelOpciones;
    [Header("Panel de datos")]
    public GameObject paneldata;
    public void IniciarJuego(int indice)
    {
        Debug.Log("Jugandoooooooooooo");
        pantallaDeCarga.SetActive(true);
        StartCoroutine(CargarAsincronamente(indice));
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
    private IEnumerator CargarAsincronamente(int indiceEscena)
    {
        AsyncOperation operacion = SceneManager.LoadSceneAsync(indiceEscena);
        operacion.allowSceneActivation = false;
        float progresoVisual = 0f;
        float velocidadLlenado = 1f / tiempoDeMas;
        while (!operacion.isDone)
        {
            float progresoReal = Mathf.Clamp01(operacion.progress / 0.9f);
            progresoVisual = Mathf.MoveTowards(progresoVisual, progresoReal, velocidadLlenado * Time.deltaTime);
            if (barraDeProgreso != null) barraDeProgreso.value = progresoVisual;
            if (textoProgreso != null) textoProgreso.text = "CARGANDO... " + (progresoVisual * 100f).ToString("F0") + "%";
            if (progresoVisual >= 1f && operacion.progress >= 0.9f)
            {
                if (textoProgreso != null) textoProgreso.text = "CARGADO 100%";
                operacion.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}