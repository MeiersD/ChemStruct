using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketController : MonoBehaviour
{
    private XRSocketInteractor socketInteractor;

    void Awake()
    {
        // Get the XRSocketInteractor component attached to this GameObject
        socketInteractor = GetComponent<XRSocketInteractor>();

    }

    public void EjectIfNotHeld()
    {
        XRBaseInteractable interactable = socketInteractor.firstInteractableSelected as XRBaseInteractable;
        string layerName = InteractionLayerMask.LayerToName(interactable.gameObject.layer);

        // Check if the layer is not "grabbedThings"
        if (layerName != "grabbedThings")
        {
            // Eject the object from the socket
            socketInteractor.interactionManager.SelectExit(socketInteractor, interactable);
            Debug.Log($"Ejected {interactable.gameObject.name} from socket because it's not held.");
        }
    }
}
