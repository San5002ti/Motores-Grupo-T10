using UnityEngine;

public class FootstepController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip footstepClip;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float footstepVolume = 0.5f; // Control de volumen (de 0 a 1)
    [Range(0.8f, 1.2f)]
    [SerializeField] private float minPitch = 0.9f;
    [Range(0.8f, 1.2f)]
    [SerializeField] private float maxPitch = 1.1f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayFootstep()
    {
        if (footstepClip != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            // Reproduce el clip aplicando el volumen configurado
            audioSource.PlayOneShot(footstepClip, footstepVolume);
        }
    }
}