using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjetoTextura
{
    public string nombreReferencia;
    public Renderer rendererObjeto;
    public Texture2D texturaA; // Textura original
    public Texture2D texturaB; // Textura secundaria para alternar
}

[System.Serializable]
public class RecompensaPorAngulo
{
    [Tooltip("Grado exacto en el eje X (ej: 0, 60, 120, 180, 240, 300)")]
    public float gradosX;
    public int monedasRecompensa;
    public float tiempoRecompensa;
}

public class SecuenciaInteractiva : MonoBehaviour
{
    [Header("2. Rotación del Primer Objeto (ej. Palanca)")]
    public Transform objetoARotar;
    public Vector3 gradosRotacion = new Vector3(45f, 0f, 0f);
    public float demoraInicioRotacion = 0f;
    public float duracionRotacionIda = 1f;
    public AudioClip sonidoRotacion;

    [Header("3. Retorno del Primer Objeto a la Normalidad")]
    public float demoraAntesDeVolver = 0.2f;
    public float duracionRotacionVuelta = 1.5f;

    [Header("4. Alternancia de Texturas (3 Objetos)")]
    public ObjetoTextura[] objetosTexturas = new ObjetoTextura[3];
    public float demoraInicioAlternancia = 0f;
    public float intervaloCambioTextura = 0.2f;
    public AudioClip sonidoAlternancia;

    [Header("5. Acción Final (Rotación secuencial y Grados en X)")]
    public float demoraAntesDeAccionFinal = 0.5f;
    public float duracionRotacionFinalX = 1f;
    [Tooltip("Lista de grados posibles en el eje X para la rotación final")]
    public List<float> listaGradosEjeX = new List<float>() { 0f, 60f, 120f, 180f, 240f, 300f };
    [Tooltip("Si pones -1, elegirá un valor al azar de la lista para cada objeto")]
    public int indiceGradoAUsar = -1;
    public AudioClip sonidoRotacionFinal;

    [Header("6. Sistema de Economía, Costos y Recompensas por Ángulo")]
    [Tooltip("Cantidad de monedas que se restan al interactuar")]
    public int costoInteraccionMonedas = 1;
    [Tooltip("Sonido que suena cuando el jugador NO tiene suficientes monedas")]
    public AudioClip sonidoSinMonedas; // <-- Nuevo sonido para cuando falten monedas

    [Tooltip("Configura aquí cuántas monedas y tiempo da cada ángulo si los 3 objetos coinciden")]
    public List<RecompensaPorAngulo> tablaRecompensas = new List<RecompensaPorAngulo>()
    {
        new RecompensaPorAngulo { gradosX = 0f, monedasRecompensa = 5, tiempoRecompensa = 20f },
        new RecompensaPorAngulo { gradosX = 60f, monedasRecompensa = 10, tiempoRecompensa = 25f },
        new RecompensaPorAngulo { gradosX = 120f, monedasRecompensa = 15, tiempoRecompensa = 30f },
        new RecompensaPorAngulo { gradosX = 180f, monedasRecompensa = 20, tiempoRecompensa = 35f },
        new RecompensaPorAngulo { gradosX = 240f, monedasRecompensa = 25, tiempoRecompensa = 40f },
        new RecompensaPorAngulo { gradosX = 300f, monedasRecompensa = 30, tiempoRecompensa = 45f }
    };
    public AudioClip sonidoVictoriaCoincidencia;

    [Header("7. Componentes de Audio")]
    public AudioSource fuenteAudio; // Audio para loops (Alternancia)
    public AudioSource fuenteAudioEfectos; // Audio para sonidos puntuales

    private bool enEjecucion = false;

    void Start()
    {
        if (fuenteAudio == null)
        {
            fuenteAudio = gameObject.AddComponent<AudioSource>();
        }
        if (fuenteAudioEfectos == null)
        {
            fuenteAudioEfectos = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Interactuar()
    {
        if (enEjecucion) return;

        // Validar si hay suficientes monedas antes de continuar
        if (ControladorMonedas.Instancia != null)
        {
            if (ControladorMonedas.Instancia.totalMonedas < costoInteraccionMonedas)
            {
                // No hay suficientes monedas: reproducir sonido de error y cancelar
                ReproducirSonido(fuenteAudioEfectos, sonidoSinMonedas, false);
                return;
            }

            // Si tiene suficientes, restamos las monedas
            ControladorMonedas.Instancia.RestarMoneda(costoInteraccionMonedas);
        }
        else
        {
            Debug.LogWarning("No se encontró el ControladorMonedas en la escena.");
        }

        StartCoroutine(SecuenciaCompleta());
    }

    private IEnumerator SecuenciaCompleta()
    {
        enEjecucion = true;

        // =========================================================================
        // FASE 1: Rotar el primer objeto mientras suena el primer efecto
        // =========================================================================
        if (demoraInicioRotacion > 0f)
            yield return new WaitForSeconds(demoraInicioRotacion);

        Quaternion rotacionOriginal = objetoARotar != null ? objetoARotar.localRotation : Quaternion.identity;
        Quaternion rotacionDestino = rotacionOriginal * Quaternion.Euler(gradosRotacion);

        if (objetoARotar != null)
        {
            ReproducirSonido(fuenteAudioEfectos, sonidoRotacion, false);
            yield return StartCoroutine(InterpolarRotacion(objetoARotar, rotacionOriginal, rotacionDestino, duracionRotacionIda));
        }

        // =========================================================================
        // FASE 2: El primer objeto vuelve a la normalidad MIENTRAS los 3 objetos 
        // alternan texturas en paralelo.
        // =========================================================================
        if (objetoARotar != null)
        {
            StartCoroutine(RetornarObjetoConDemora(objetoARotar, rotacionDestino, rotacionOriginal, demoraAntesDeVolver, duracionRotacionVuelta));
        }

        if (demoraInicioAlternancia > 0f)
            yield return new WaitForSeconds(demoraInicioAlternancia);

        bool alternandoActivo = true;
        StartCoroutine(RutinaAlternanciaTexturas(() => alternandoActivo));

        // =========================================================================
        // FASE 3: Acción Final - Los objetos rotan uno detrás del otro
        // =========================================================================
        if (demoraAntesDeAccionFinal > 0f)
            yield return new WaitForSeconds(demoraAntesDeAccionFinal);

        for (int i = 0; i < objetosTexturas.Length; i++)
        {
            var objTextura = objetosTexturas[i];
            if (objTextura.rendererObjeto != null)
            {
                float gradosX = ObtenerGradoXAleatorio();

                Transform tObj = objTextura.rendererObjeto.transform;
                Quaternion rotIniX = tObj.localRotation;
                Quaternion rotFinX = rotIniX * Quaternion.Euler(gradosX, 0f, 0f);

                ReproducirSonido(fuenteAudioEfectos, sonidoRotacionFinal, false);
                yield return StartCoroutine(InterpolarRotacion(tObj, rotIniX, rotFinX, duracionRotacionFinalX));
            }
        }

        // =========================================================================
        // CIERRE INMEDIATO Y VALIDACIÓN DE RECOMPENSAS POR ÁNGULO
        // =========================================================================
        alternandoActivo = false;
        if (fuenteAudio.isPlaying) fuenteAudio.Stop();

        float? anguloComun = ObtenerAnguloXComunDeObjetos();

        if (anguloComun.HasValue)
        {
            RecompensaPorAngulo recompensaEncontrada = tablaRecompensas.Find(r => Mathf.Approximately(r.gradosX, anguloComun.Value));

            if (recompensaEncontrada != null)
            {
                ReproducirSonido(fuenteAudioEfectos, sonidoVictoriaCoincidencia, false);

                if (ControladorMonedas.Instancia != null)
                {
                    ControladorMonedas.Instancia.AgregarMoneda(recompensaEncontrada.monedasRecompensa);
                }

                HUDTiempo hudTiempo = FindFirstObjectByType<HUDTiempo>();
                if (hudTiempo != null)
                {
                    hudTiempo.SumarTiempo(recompensaEncontrada.tiempoRecompensa);
                }
            }
        }

        // =========================================================================
        // RESTAURACIÓN: Todos los objetos vuelven a sus texturas originales (texturaA)
        // =========================================================================
        foreach (var objTextura in objetosTexturas)
        {
            if (objTextura.rendererObjeto != null)
            {
                CambiarTexturaMaterial(objTextura.rendererObjeto.material, objTextura.texturaA);
            }
        }

        enEjecucion = false;
    }

    private float? ObtenerAnguloXComunDeObjetos()
    {
        if (objetosTexturas.Length == 0 || objetosTexturas[0].rendererObjeto == null) return null;

        float primerAngulo = Mathf.Round(objetosTexturas[0].rendererObjeto.transform.localEulerAngles.x);
        primerAngulo = (primerAngulo % 360 + 360) % 360;

        for (int i = 1; i < objetosTexturas.Length; i++)
        {
            if (objetosTexturas[i].rendererObjeto == null) return null;

            float anguloActual = Mathf.Round(objetosTexturas[i].rendererObjeto.transform.localEulerAngles.x);
            anguloActual = (anguloActual % 360 + 360) % 360;

            if (!Mathf.Approximately(anguloActual, primerAngulo))
            {
                return null;
            }
        }

        return primerAngulo;
    }

    private IEnumerator RutinaAlternanciaTexturas(System.Func<bool> condicionContinuar)
    {
        ReproducirSonido(fuenteAudio, sonidoAlternancia, true);
        bool mostrarTexturaB = true;

        while (condicionContinuar())
        {
            foreach (var item in objetosTexturas)
            {
                if (item.rendererObjeto != null)
                {
                    Texture2D texActual = mostrarTexturaB ? item.texturaB : item.texturaA;
                    CambiarTexturaMaterial(item.rendererObjeto.material, texActual);
                }
            }

            mostrarTexturaB = !mostrarTexturaB;
            yield return new WaitForSeconds(intervaloCambioTextura);
        }
    }

    private float ObtenerGradoXAleatorio()
    {
        if (listaGradosEjeX == null || listaGradosEjeX.Count == 0) return 0f;

        if (indiceGradoAUsar < 0)
        {
            int randomIndex = Random.Range(0, listaGradosEjeX.Count);
            return listaGradosEjeX[randomIndex];
        }
        else
        {
            int indexSeguro = Mathf.Clamp(indiceGradoAUsar, 0, listaGradosEjeX.Count - 1);
            return listaGradosEjeX[indexSeguro];
        }
    }

    private IEnumerator RetornarObjetoConDemora(Transform objetivo, Quaternion desde, Quaternion hasta, float demora, float duracion)
    {
        if (demora > 0f)
            yield return new WaitForSeconds(demora);

        yield return StartCoroutine(InterpolarRotacion(objetivo, desde, hasta, duracion));
    }

    private IEnumerator InterpolarRotacion(Transform objetivo, Quaternion desde, Quaternion hasta, float duracion)
    {
        if (duracion <= 0f)
        {
            objetivo.localRotation = hasta;
            yield break;
        }

        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            float progreso = Mathf.Clamp01(t / duracion);
            objetivo.localRotation = Quaternion.Slerp(desde, hasta, progreso);
            yield return null;
        }
        objetivo.localRotation = hasta;
    }

    private void CambiarTexturaMaterial(Material mat, Texture2D nuevaTextura)
    {
        if (mat == null || nuevaTextura == null) return;

        mat.mainTexture = nuevaTextura;
        if (mat.HasProperty("_BaseMap"))
        {
            mat.SetTexture("_BaseMap", nuevaTextura);
        }
    }

    private void ReproducirSonido(AudioSource fuente, AudioClip clip, bool enBucle)
    {
        if (fuente != null && clip != null)
        {
            fuente.clip = clip;
            fuente.loop = enBucle;
            fuente.Play();
        }
    }
}