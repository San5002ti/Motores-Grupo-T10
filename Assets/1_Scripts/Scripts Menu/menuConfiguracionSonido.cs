using UnityEngine;
using UnityEngine.UI;

public class ControlAudioGlobal : MonoBehaviour
{
    public Slider sliderVolumen;
    void Start()
    {
        float volumenGuardado = PlayerPrefs.GetFloat("volumen", 1f);
        AudioListener.volume = volumenGuardado;
        if (sliderVolumen != null)
        {
            sliderVolumen.value = volumenGuardado;
        }
    }
    public void CambiarVolumenGlobal(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("volumen", valor);
    }
}