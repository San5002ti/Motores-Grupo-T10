using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Apagarmaus : MonoBehaviour
{
    private bool maus_activo = false;
    private float tiempo = 0f;
    [Header("Tiempo de espera entre cambio")]
    public float retraso = 0f;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        tiempo += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && tiempo > retraso)
        {
                maus_activo = !maus_activo;
                if (maus_activo)
                {
                    Debug.Log("maus Activo");
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else
                {
                    Debug.Log("maus desactivado");
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                tiempo = 0;
        }

    }
}
