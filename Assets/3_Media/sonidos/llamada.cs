using System.Collections;
using UnityEngine;

public class ControladorLlamada : MonoBehaviour
{
    public float tiempoParaIniciar = 5f;
    public GameObject objetoADesactivar;
    public GameObject segundoObjeto;
    public AudioSource fuenteAudio;
    public AudioClip sonidoContinuo;
    public AudioClip sonidoApagado;

    void Start()
    {
        if (fuenteAudio == null)
        {
            fuenteAudio = GetComponent<AudioSource>();
        }

        StartCoroutine(RutinaAutomaticaDeSonidos());
    }
    private IEnumerator RutinaAutomaticaDeSonidos()
    {
        yield return new WaitForSeconds(tiempoParaIniciar);

        if (fuenteAudio != null && sonidoContinuo != null)
        {
            fuenteAudio.clip = sonidoContinuo;
            fuenteAudio.loop = false;
            fuenteAudio.Play();
            yield return new WaitForSeconds(sonidoContinuo.length);
        }

        if (fuenteAudio != null && sonidoApagado != null)
        {
            fuenteAudio.clip = sonidoApagado;
            fuenteAudio.Play();
            yield return new WaitForSeconds(sonidoApagado.length);
        }
        if (objetoADesactivar != null)
        {
            objetoADesactivar.SetActive(false);
            segundoObjeto.SetActive(true);
        }
        this.enabled = false;
    }
}