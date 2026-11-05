using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDTiempo : MonoBehaviour
{
    public static HUDTiempo Instancia;

    [Header("UI General")]
    public TextMeshProUGUI textoContador; // El contador normal en pantalla
    public GameObject panelFinDelJuego;   // El panel principal

    [Header("Elementos del Panel (Orden de aparición)")]
    public TextMeshProUGUI textoTituloPanel;       // 1er Texto (Ej: "¡Fin del Juego!")
    public TextMeshProUGUI textoTiempoTranscurrido; // 2do Texto (El tiempo que pasó)
    public GameObject botonUno;                    // 3er Elemento (Aparece después del texto 2)
    public GameObject botonDos;                    // 4to Elemento (Aparece al final de todo)

    [Header("Audio y Efectos de Sonido")]
    public AudioSource fuenteMusicaFondo;
    public AudioSource fuenteEfectos;
    public AudioClip sonidoFinDelJuego;   // Sonido al aparecer el panel
    public AudioClip sonidoTexto1;        // Sonido al aparecer el título
    public AudioClip sonidoTexto2;        // Sonido al aparecer el tiempo transcurrido
    public AudioClip sonidoBotonUno;      // Sonido al aparecer el primer botón
    public AudioClip sonidoBotonDos;      // Sonido diferente al aparecer el segundo botón

    [Header("Configuración")]
    public float tiempoRestante = 60f;
    private float tiempoTotalTranscurrido = 0f;
    private bool juegoTerminado = false;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (fuenteEfectos == null)
        {
            fuenteEfectos = gameObject.AddComponent<AudioSource>();
        }

        // Aseguramos que al iniciar el juego el mouse esté oculto y bloqueado
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (juegoTerminado) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            tiempoTotalTranscurrido += Time.deltaTime;

            ActualizarTextoHUD();

            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                TerminarJuego();
            }
        }
    }

    void ActualizarTextoHUD()
    {
        if (textoContador != null)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);
            textoContador.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    public void SumarTiempo(float cantidad)
    {
        if (juegoTerminado) return;
        tiempoRestante += cantidad;
    }

    void TerminarJuego()
    {
        juegoTerminado = true;

        // Cortar música de fondo inmediatamente
        if (fuenteMusicaFondo != null) fuenteMusicaFondo.Stop();

        // Deshabilitar el bloqueo del mouse y hacerlo visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Iniciar la secuencia visual y de sonido escalonada
        StartCoroutine(SecuenciaFinDelJuego());
    }

    private IEnumerator SecuenciaFinDelJuego()
    {
        // 1. Aseguramos de inmediato que los textos y ambos botones empiecen ocultos
        if (textoTituloPanel != null) textoTituloPanel.gameObject.SetActive(false);
        if (textoTiempoTranscurrido != null) textoTiempoTranscurrido.gameObject.SetActive(false);
        if (botonUno != null) botonUno.SetActive(false);
        if (botonDos != null) botonDos.SetActive(false);

        // 2. Activamos el panel y reproducimos su sonido
        if (panelFinDelJuego != null)
        {
            panelFinDelJuego.SetActive(true);
        }

        if (fuenteEfectos != null && sonidoFinDelJuego != null)
        {
            fuenteEfectos.PlayOneShot(sonidoFinDelJuego);
        }

        // Pausamos el juego por completo
        Time.timeScale = 0f;

        // Breve pausa antes del primer texto
        yield return new WaitForSecondsRealtime(0.4f);

        // 3. PASO 1: Aparece el primer texto (Título) + su sonido
        if (textoTituloPanel != null)
        {
            textoTituloPanel.gameObject.SetActive(true);
            if (fuenteEfectos != null && sonidoTexto1 != null)
            {
                fuenteEfectos.PlayOneShot(sonidoTexto1);
            }
        }

        // Pausa antes del segundo texto
        yield return new WaitForSecondsRealtime(0.5f);

        // 4. PASO 2: Se calcula y aparece el texto del tiempo transcurrido + su sonido
        if (textoTiempoTranscurrido != null)
        {
            int minutos = Mathf.FloorToInt(tiempoTotalTranscurrido / 60);
            int segundos = Mathf.FloorToInt(tiempoTotalTranscurrido % 60);
            textoTiempoTranscurrido.text = $"Tiempo transcurrido: {minutos:00}:{segundos:00}";
            textoTiempoTranscurrido.gameObject.SetActive(true);

            if (fuenteEfectos != null && sonidoTexto2 != null)
            {
                fuenteEfectos.PlayOneShot(sonidoTexto2);
            }
        }

        // Pausa antes del primer botón
        yield return new WaitForSecondsRealtime(0.5f);

        // 5. PASO 3: Aparece el primer botón + su sonido (AHORA SÍ, después del texto 2)
        if (botonUno != null)
        {
            botonUno.SetActive(true);
            if (fuenteEfectos != null && sonidoBotonUno != null)
            {
                fuenteEfectos.PlayOneShot(sonidoBotonUno);
            }
        }

        // Breve pausa antes del segundo botón
        yield return new WaitForSecondsRealtime(0.4f);

        // 6. PASO 4: Finalmente aparece el segundo botón + su sonido exclusivo
        if (botonDos != null)
        {
            botonDos.SetActive(true);
            if (fuenteEfectos != null && sonidoBotonDos != null)
            {
                fuenteEfectos.PlayOneShot(sonidoBotonDos);
            }
        }
    }
    // Método para reiniciar o cambiar de escena por su nombre
    public void CambiarEscena(string nombreEscena)
    {
        // ¡Muy importante! Restaurar el Time.timeScale a 1 antes de cambiar de escena, 
        // de lo contrario la siguiente escena iniciará congelada.
        Time.timeScale = 1f;

        SceneManager.LoadScene(nombreEscena);
    }

    // O si solo quieres un botón para reiniciar la escena en la que estás actualmente:
    public void ReiniciarEscenaActual()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}