using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AtomScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrabbed(){
        
        // Make array of child elements
        foreach (Transform child in transform)
        {
            // If element has tag "bond"
            if (child.CompareTag("Bond") || child.CompareTag("GhostAtom"))
            {
                // Destroy it
                Destroy(child.gameObject);
            }
        }
    }

    public void toggleChildAtomSockets(){
        RecursionHelper(transform);
    }
    

    private void RecursionHelper(Transform currentTransform)
{
    // The "base case" here is implicit:
    // If there are no children tagged with "Bond", the recursion ends naturally.
    foreach (Transform child in currentTransform)
    {
        if (child.CompareTag("Bond"))
        {
            BondUpdater bondUpdater = child.GetComponent<BondUpdater>();
            if (bondUpdater == null || bondUpdater.equippedAtom == null)
            {
                // If we don't have a valid BondUpdater or equipped atom, just continue.
                continue;
            }

            // Toggle sockets on the currently found bonded atom
            toggleSockets(bondUpdater.equippedAtom);

            // Recursively call the helper on the child's atom
            RecursionHelper(bondUpdater.equippedAtom.transform);
        }
    }

    // When this loop finishes, if no Bond children were found,
    // recursion does not continue further. This is effectively the "base case."
}

private void toggleSockets(GameObject atom)
{
    foreach (Transform child in atom.transform)
    {
        if (child.CompareTag("Socket"))
        {
            XRSocketInteractor childSocketInteractor = child.GetComponent<XRSocketInteractor>();
            if (childSocketInteractor != null && !childSocketInteractor.hasSelection)
            {
                // Toggle the socketActive flag
                childSocketInteractor.socketActive = !childSocketInteractor.socketActive;
            }
        }
    }
}

}
