using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleTest : MonoBehaviour
{
    //public GameObject parent;
 
    void OnTriggerEnter(Collider collider)
    {
        transform.parent.GetComponent<BrickHandler>().OnTriggerEnterMaleStud(this.gameObject, collider.gameObject);
    }
    void OnTriggerExit(Collider collider)
    {
        transform.parent.GetComponent<BrickHandler>().OnTriggerExitMaleStud(this.gameObject, collider.gameObject);
    }
}
