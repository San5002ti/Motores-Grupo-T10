using UnityEngine;
using TMPro;

public class HUDTiempo : MonoBehaviour
{
    [Header("ui")]
    public TextMeshProUGUI textoTiempo;

    [Header("config tiempo")]
    public bool contarHaciaArriba = true;
    public float tiempoInicial = 0f;

    [Header("Efectos por las dudas")]
    public bool activar = true;
    public float escala = 1.25f; 
    public float velocidad = 8f; 

    private float tiempoActual;
    private int ultimoSegundoGuardado = -1;
    private Vector3 escalaOriginal;
    void Start()
    {
        tiempoActual = tiempoInicial;

        if (textoTiempo != null)
        {
            escalaOriginal = textoTiempo.transform.localScale;
        }

    }
    void Update()
    {
        if (contarHaciaArriba)
        {
            tiempoActual += Time.deltaTime;
        }
        else
        {
            tiempoActual -= Time.deltaTime;
            if (tiempoActual < 0f) tiempoActual = 0f;
        }
        int minutos = Mathf.FloorToInt(tiempoActual / 60f);
        int segundos = Mathf.FloorToInt(tiempoActual % 60f);

        if (textoTiempo != null)
        {
            textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
            if (activar && segundos != ultimoSegundoGuardado)
            {
                ultimoSegundoGuardado = segundos;
                textoTiempo.transform.localScale = escalaOriginal * escala;
            }
            textoTiempo.transform.localScale = Vector3.Lerp(
                textoTiempo.transform.localScale,
                escalaOriginal,
                Time.deltaTime * velocidad
            );
        }
    }
    public void ReiniciarTiempo(float nuevoTiempo = 0f)
    {
        tiempoActual = nuevoTiempo;
    }
    public float ObtenerTiempoSegundos()
    {
        return tiempoActual;
    }
}