using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ChangeLayerOnGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Coroutine resetLayerCoroutine;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        grabInteractable.interactionLayers = InteractionLayerMask.GetMask("Everything");
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        // Cancel any ongoing coroutine to prevent premature reset
        if (resetLayerCoroutine != null)
        {
            StopCoroutine(resetLayerCoroutine);
        }

        // Start a coroutine to delay the reset of the interaction layer mask
        resetLayerCoroutine = StartCoroutine(DelayedResetInteractionLayer());
    }

    private IEnumerator DelayedResetInteractionLayer()
    {
        // Wait a short moment to allow the socket to attach the object
        yield return new WaitForSeconds(0.1f);

        // Revert the interaction layer mask
        grabInteractable.interactionLayers = InteractionLayerMask.GetMask("Default");
        Debug.Log($"Interaction layers reverted to 'Default' for {gameObject.name}");
    }
}
