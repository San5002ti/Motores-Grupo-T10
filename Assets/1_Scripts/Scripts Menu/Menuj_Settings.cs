using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Menuj_Settings : MonoBehaviour
{
    public void PantallaCompleta()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("Pantalla completa: " + Screen.fullScreen);
    }
    public void CambiarResolucion(int indice)
    {
        bool pantalla_completa = Screen.fullScreen;
        switch (indice)
        {
            case 0:
                Debug.Log("En full hd");
                Screen.SetResolution(1920, 1080, pantalla_completa);
                break;
            case 1:
                Debug.Log("En hd");
                Screen.SetResolution(1280, 720, pantalla_completa);
                break;
            case 2:
                Debug.Log("En 2k");
                Screen.SetResolution(2560, 1440, pantalla_completa);
                break;
            case 3:
                Debug.Log("En 4k");
                Screen.SetResolution(3840, 2160, pantalla_completa);
                break;
        }
    }
    public void cambiar_ResolucionGrafica(int indice)
    {
        QualitySettings.SetQualityLevel(indice);
        Debug.Log("La calidad grafica es: " + QualitySettings.names[indice]);
    }
    public void cambiarAntialising(int indice)
    {
        if (Camera.main == null) return;
        UniversalAdditionalCameraData datosCamara = Camera.main.GetComponent<UniversalAdditionalCameraData>();
        if (datosCamara != null)
        {
            if (indice == 0)
                datosCamara.antialiasing = AntialiasingMode.None; 
            else if (indice == 1)
                datosCamara.antialiasing = AntialiasingMode.FastApproximateAntialiasing; 
            else if (indice >= 2)
                datosCamara.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing; 
            Debug.Log("Anti-Aliasing ajustado al índice: " + indice);
        }
    }
    public void CambiarSombras(int indice)
    {
        LightShadows tipoSombra = LightShadows.None; 
        if (indice == 1) tipoSombra = LightShadows.Hard; 
        if (indice >= 2) tipoSombra = LightShadows.Soft;
        Light[] lucesDelClub = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light luz in lucesDelClub)
        {
            luz.shadows = tipoSombra;
        }
        Debug.Log("Sombras ajustadas a: " + tipoSombra.ToString());
    }
}