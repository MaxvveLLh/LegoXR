using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleStudCollisionHandler : MonoBehaviour
{
    public GameObject parent;
 
    void OnTriggerEnter(Collider collider)
    {
        parent.GetComponent<BrickCollisionHandler>().OnTriggerEnterMaleStud(this.gameObject, collider.gameObject);
    }
    void OnTriggerExit(Collider collider)
    {
        parent.GetComponent<BrickCollisionHandler>().OnTriggerExitMaleStud(this.gameObject, collider.gameObject);
    }
}
