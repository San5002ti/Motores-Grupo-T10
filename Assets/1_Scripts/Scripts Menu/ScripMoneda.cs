using UnityEngine;

public class Moneda : MonoBehaviour
{
    public int valorMoneda = 1;
    public float velocidadRotacion = 100f;
    private void Update()
    {
        transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ControladorMonedas.Instancia != null)
            {
                ControladorMonedas.Instancia.AgregarMoneda(valorMoneda);
            }
            Destroy(gameObject);
        }
    }
}