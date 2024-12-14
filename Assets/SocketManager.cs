using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketManager : MonoBehaviour {

    private GameObject ghostAtom;
    public GameObject bondPrefab; // Assign a cylinder prefab for the bond

    public GameObject ghostAtomPrefab;
    public event System.Action<GameObject, XRSocketInteractor> OnAnyItemEquipped; // Event for all socket interactions

    private Transform socketParent;
    private Transform socketLocation;

    private void Start() {
        // Automatically find and register all XRSocketInteractor components with the "Atom" tag or class
        RegisterAllAtomSockets();
    }

    private void Update(){
    }

    private void RegisterAllAtomSockets() {
        // Find all GameObjects tagged as "Atom"
        GameObject[] socketObjects = GameObject.FindGameObjectsWithTag("Socket");

        // Iterate through all found objects and collect sockets
        foreach (GameObject currSocket in socketObjects) {
            XRSocketInteractor socketInteractor = currSocket.GetComponentInChildren<XRSocketInteractor>();
            socketInteractor.selectEntered.AddListener(args => HandleSelectEntered(args, socketInteractor));

        }
    }

    private GameObject CreateGhostAtom(Transform socketParent, Transform equippedItem, Transform socketLocation){
        Vector3 position = socketLocation.position;
        GameObject ghostAtom = Instantiate(ghostAtomPrefab, position, Quaternion.identity);
        ghostAtom.transform.SetParent(socketParent, true);
        ghostAtom.transform.localRotation = Quaternion.identity; // Reset rotation if needed

        return ghostAtom;
    }

    public void CreateBond(Transform socketParent, GameObject equippedItem, Transform socketLocation) {

        // Calculate the position as the midpoint between the socket parent and atom
        Vector3 position = (socketParent.position + socketLocation.position) / 2;

        // Calculate the direction between the socket parent and the atom
        Vector3 direction = socketLocation.position - socketParent.position;

        // Instantiate the bond at the calculated position
        GameObject bond = Instantiate(bondPrefab, position, Quaternion.identity);

        // Align the bond to the direction between the socket parent and atom
        bond.transform.up = direction.normalized;
        float distance = direction.magnitude;
        bond.transform.localScale = new Vector3(bond.transform.localScale.x, distance / 2, bond.transform.localScale.z);
        bond.transform.SetParent(socketParent, true); 
        BondUpdater bondUpdater = bond.GetComponent<BondUpdater>();
        bondUpdater.equippedAtom = equippedItem;
        Debug.Log("added: "+bondUpdater.equippedAtom);
    }

    private void HandleSelectEntered(SelectEnterEventArgs args, XRSocketInteractor socket) {
        GameObject equippedItem = args.interactableObject.transform.gameObject;
        OnAnyItemEquipped?.Invoke(equippedItem, socket);
        GameObject socketParent = socket.transform.parent.gameObject;
        ghostAtom = CreateGhostAtom(socketParent.transform, equippedItem.transform, socket.transform);
        CreateBond(socketParent.transform, equippedItem, socket.transform);
    }

    
}
