using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleTest : MonoBehaviour
{
    //public GameObject parent;
   
    void OnTriggerEnter(Collider collider)
    {
        transform.parent.GetComponent<BrickHandler>().OnTriggerEnterFemaleStud(collider.gameObject, this.gameObject);
    }
    void OnTriggerExit(Collider collider)
    {
        transform.parent.GetComponent<BrickHandler>().OnTriggerExitFemaleStud(collider.gameObject, this.gameObject);
    }
}
