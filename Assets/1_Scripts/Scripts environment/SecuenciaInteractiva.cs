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
    public List<float> listaGradosEjeX = new List<float>() { 90f, 180f, 270f };
    [Tooltip("Si pones -1, elegirá un valor al azar de la lista para cada objeto")]
    public int indiceGradoAUsar = -1;
    public AudioClip sonidoRotacionFinal;

    [Header("6. Componentes de Audio")]
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

        // Iniciamos la alternancia de texturas en segundo plano
        bool alternandoActivo = true;
        StartCoroutine(RutinaAlternanciaTexturas(() => alternandoActivo));

        // =========================================================================
        // FASE 3: Acción Final - Los objetos rotan uno detrás del otro 
        // MIENTRAS la alternancia de texturas sigue corriendo.
        // =========================================================================
        if (demoraAntesDeAccionFinal > 0f)
            yield return new WaitForSeconds(demoraAntesDeAccionFinal);

        foreach (var objTextura in objetosTexturas)
        {
            if (objTextura.rendererObjeto != null)
            {
                float gradosX = ObtenerGradoXAleatorio();

                Transform tObj = objTextura.rendererObjeto.transform;
                Quaternion rotIniX = tObj.localRotation;
                Quaternion rotFinX = rotIniX * Quaternion.Euler(gradosX, 0f, 0f);

                // Reproduce el sonido de rotación final para cada objeto
                ReproducirSonido(fuenteAudioEfectos, sonidoRotacionFinal, false);

                // Rota uno tras otro
                yield return StartCoroutine(InterpolarRotacion(tObj, rotIniX, rotFinX, duracionRotacionFinalX));
            }
        }

        // =========================================================================
        // CIERRE INMEDIATO: Apenas termina de girar el último objeto, 
        // detenemos la alternancia y el sonido en bucle de golpe.
        // =========================================================================
        alternandoActivo = false;
        if (fuenteAudio.isPlaying) fuenteAudio.Stop();

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