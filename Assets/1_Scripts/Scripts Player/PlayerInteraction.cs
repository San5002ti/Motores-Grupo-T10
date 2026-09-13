using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interacción")]
    [SerializeField] private float interactionDistance = 2.5f;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private LayerMask interactionLayer;

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        if (interactionPoint == null)
            return;

        Ray ray = new Ray(
            interactionPoint.position,
            interactionPoint.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance,
            interactionLayer))
        {
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionPoint == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(
            interactionPoint.position,
            interactionPoint.forward * interactionDistance
        );
    }
}