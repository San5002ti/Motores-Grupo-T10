using UnityEngine;

public class CamaraMinimapa : MonoBehaviour
{
    [Header("referencias")]
    public Transform jugador;
    public float altura = 30f;

    void LateUpdate()
    {
        if (jugador == null) return;
        Vector3 nuevaPosicion = jugador.position;
        nuevaPosicion.y = jugador.position.y + altura;
        transform.position = nuevaPosicion;
        transform.rotation = Quaternion.Euler(90f, jugador.eulerAngles.y, 0f);
    }
}
