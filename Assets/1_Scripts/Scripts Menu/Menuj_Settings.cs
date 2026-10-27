using UnityEngine;

public class Menuj_Settings : MonoBehaviour
{
    public void PantallaCompleta()
    {
        Debug.Log("Pantalla completa");
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void CambiarResolucion(int indice)
    {
        bool pantalla_completa = Screen.fullScreen;
        switch (indice)
        {
            case 0:
                Debug.Log("En full hd");
                Screen.SetResolution(1920, 1080 , pantalla_completa);
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
        Debug.Log("La calidad grafica es: " + QualitySettings.names[indice] );
    }
}
