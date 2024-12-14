using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The GhostAtom is a GameObject that is transparent and sits where the equipped atom is placed and is used for its sphereical collider
public class GhostAtomUpdater : MonoBehaviour
{
    public void destroyGhostAtom(){
        Destroy(gameObject);
    }
}
