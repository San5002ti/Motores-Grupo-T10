using UnityEngine;

public class MenuConfiguracionPreferencias : MonoBehaviour
{
    public GameObject miniMapa;
    public void ActivarMiniMapa()
    {
        miniMapa.SetActive(!miniMapa.activeSelf);
    }
}
