using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BondUpdater : MonoBehaviour
{
    public GameObject equippedAtom;

    public int bondOrder = 1;

    public void destroyBond(){
        Destroy(gameObject);
    }

    public GameObject getEquippedAtom(){
        return equippedAtom;
    }

    public string getBondOrder(){
        if (bondOrder == 1){return "001";}
        return "002";
    }

    public void toggleBondOrder(){
        if (bondOrder == 1){
            bondOrder = 2;
        } else {
            bondOrder = 1;
        }
    }

    public void toggleOlefin(){
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            // Toggle the enabled state of the MeshRenderer
            meshRenderer.enabled = !meshRenderer.enabled;
        }
        toggleBondOrder();
    }    
}
