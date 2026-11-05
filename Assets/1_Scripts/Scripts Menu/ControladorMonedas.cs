using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorMonedas : MonoBehaviour
{
    public static ControladorMonedas Instancia { get; private set; }

    [Header("ui")]
    public TextMeshProUGUI textoMonedas;
    [Header("Dato")]
    public int totalMonedas = 0;
    public int monedasParaCambiarEscena = 10;   

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        ActualizarUI();
    }
    public void AgregarMoneda(int cantidad = 1)
    {
        totalMonedas += cantidad;
        ActualizarUI();
        if (totalMonedas >= monedasParaCambiarEscena)
        {

                SceneManager.LoadScene(2);
            
        }
    }
    private void ActualizarUI()
    {
        if (textoMonedas != null)
        {
            textoMonedas.text = ":" + totalMonedas.ToString();
        }
    }
    public void RestarMoneda(int cantidad = 1)
    {
        totalMonedas -= cantidad;
        ActualizarUI();
    }
}