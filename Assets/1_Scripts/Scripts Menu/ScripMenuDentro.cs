using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ScripMenuDentro : MonoBehaviour
{
    public GameObject menu;
    public List<ordenPaneles> paneles = new List<ordenPaneles>();
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
            if (menu.activeSelf == true)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
    }
    public void Continuar()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    public void ConfiguracionesGraficas()
    {
        foreach (var item in paneles)
        {
            if (item.panel.activeSelf == true)
            {
                item.panel.SetActive(false);
            }
        }
        foreach (var item in paneles)
        {
            if (item.tipoPaneles == listaPaneles.configuracionGrafica)
            {
                item.panel.SetActive(true);
                break;
            }
        }
    }
    public void listaTeclas()
    {
        foreach (var item in paneles)
        {
            if (item.panel.activeSelf == true)
            {
                item.panel.SetActive(false);
            }
        }
        foreach (var item in paneles)
        {
            if (item.tipoPaneles == listaPaneles.teclas)
            {
                item.panel.SetActive(true);
                break;
            }
        }
    }
    public void ConfiguracionSonido()
    {
        foreach (var item in paneles)
        {
            if (item.panel.activeSelf == true)
            {
                item.panel.SetActive(false);
            }
        }
        foreach (var item in paneles)
        {
            if (item.tipoPaneles == listaPaneles.configuracionAudio)
            {
                item.panel.SetActive(true);
                break;
            }
        }
    }
    public void Preferencias()
    {
        foreach (var item in paneles)
        {
            if (item.panel.activeSelf == true)
            {
                item.panel.SetActive(false);
            }
        }
        foreach (var item in paneles)
        {
            if (item.tipoPaneles == listaPaneles.preferencias)
            {
                item.panel.SetActive(true);
                break;
            }
        }
    }
    public void Salir()
    {
        Application.Quit();
    }
}
[Serializable]
public enum listaPaneles
{
    configuracionGrafica = 0,
    teclas = 1,
    configuracionAudio = 2,
    preferencias = 3
}
[Serializable]
public class ordenPaneles
{
    public listaPaneles tipoPaneles;
    public GameObject panel;
}