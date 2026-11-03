using UnityEngine;

public class ApagarLamparaMenu : MonoBehaviour, IInteractua
{
    public GameObject lampara;

    public void Interactuar()
    {
        lampara.SetActive(!lampara.activeSelf);
    }
}
