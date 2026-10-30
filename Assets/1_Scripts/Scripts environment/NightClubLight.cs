using UnityEngine;

[RequireComponent(typeof(Light))]
public class NightClubLight : MonoBehaviour
{
    private Light clubLight;

    [Header("Configuración de Movimiento")]
    public float velocidadMovimiento = 1f;
    public Vector3 rangoRotacion = new Vector3(30f, 45f, 0f);

    [Header("Configuración de Color")]
    public float velocidadColor = 0.5f;

    private Vector3 rotacionInicial;
    private float tiempoMovimiento;
    private float tiempoColor;

    void Start()
    {
        clubLight = GetComponent<Light>();
        rotacionInicial = transform.localEulerAngles;

        // Forzar a que sea una Spot Light para el efecto reflector
        clubLight.type = LightType.Spot;
    }

    void Update()
    {
        ManejarMovimiento();
        ManejarColor();
    }

    void ManejarMovimiento()
    {
        tiempoMovimiento += Time.deltaTime * velocidadMovimiento;

        // Movimiento fluido usando Perlin Noise para simular motores mecánicos
        float rotX = (Mathf.PerlinNoise(tiempoMovimiento, 0f) - 0.5f) * 2f * rangoRotacion.x;
        float rotY = (Mathf.PerlinNoise(0f, tiempoMovimiento) - 0.5f) * 2f * rangoRotacion.y;

        transform.localEulerAngles = rotacionInicial + new Vector3(rotX, rotY, 0f);
    }

    void ManejarColor()
    {
        tiempoColor += Time.deltaTime * velocidadColor;

        // Mantiene el color ciclando por todo el espectro HSV con máxima saturación y brillo
        float matiz = tiempoColor % 1f;
        clubLight.color = Color.HSVToRGB(matiz, 1f, 1f);
    }
}
