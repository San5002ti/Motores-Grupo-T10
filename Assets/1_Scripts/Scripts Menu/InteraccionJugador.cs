using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class InteraccionJugador : MonoBehaviour
{
    public float distanciaInteraccion = 0f;
    public Camera camaraPrincipal;
    private GameObject objeto;
    void Update()
    {
        PointerEventData punto = new PointerEventData(EventSystem.current);
        punto.position = new Vector2(Screen.width / 2f, Screen.height / 2f);
        List<RaycastResult> resultados = new List<RaycastResult>();
        EventSystem.current.RaycastAll(punto, resultados);
        GameObject objetopunto = resultados.Count > 0 ? resultados[0].gameObject : null;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clikea");
            InteractuarJugador();
            objeto = objetopunto;
            ExecuteEvents.ExecuteHierarchy(objeto, punto, ExecuteEvents.pointerClickHandler);
            ExecuteEvents.ExecuteHierarchy(objeto, punto, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.ExecuteHierarchy(objeto, punto, ExecuteEvents.beginDragHandler);
        }
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.E)) && objeto != null)
        {
            Slider slider = objeto.GetComponentInParent<Slider>();
            if(slider != null)
            {
                RectTransform rect = slider.GetComponent<RectTransform>();
                Vector2 puntoLocal;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, punto.position, Camera.main, out puntoLocal);
                float porcentaje = (puntoLocal.x - rect.rect.x) / rect.rect.width;
                slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, porcentaje);
            }
            else
            {
                ExecuteEvents.ExecuteHierarchy(objeto, punto, ExecuteEvents.dragHandler);
            }
        }
        if (Input.GetMouseButton(0) || Input.GetKeyUp(KeyCode.E))
        {
            if (objeto != null)
            {
                ExecuteEvents.ExecuteHierarchy(objeto, punto, ExecuteEvents.pointerUpHandler);
                ExecuteEvents.ExecuteHierarchy(objeto, punto, ExecuteEvents.endDragHandler);
                objeto = null;
            }
        }
    }
    void InteractuarJugador()
    {
        Ray rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit golpe;
        if (Physics.Raycast(rayo, out golpe, distanciaInteraccion)) 
        {
            IInteractua objetoInteraccion = golpe.transform.GetComponent<IInteractua>();
            if (objetoInteraccion != null)
            {
                objetoInteraccion.Interactuar();
            }
        }
    }

}
