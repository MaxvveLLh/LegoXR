using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleStudCollisionHandler : MonoBehaviour
{
    public GameObject parent;
   
    void OnTriggerEnter(Collider collider)
    {
        parent.GetComponent<BrickCollisionHandler>().OnTriggerEnterFemaleStud(collider.gameObject, this.gameObject);
    }
    void OnTriggerExit(Collider collider)
    {
        parent.GetComponent<BrickCollisionHandler>().OnTriggerExitFemaleStud(collider.gameObject, this.gameObject);
    }
}
