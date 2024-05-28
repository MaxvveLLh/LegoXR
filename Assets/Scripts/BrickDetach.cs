using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BrickDetach : MonoBehaviour
{
    bool value;
    private GameObject DetachObject = null;
    bool TriggerPressed = false;

    void FixedUpdate()
    {
        UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out TriggerPressed);
        if (DetachObject != null && TriggerPressed)
        {
            DetachObject.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material 
                = Resources.Load("Materials/RAL_2002") as Material;
            Destroy(DetachObject.GetComponent<FixedJoint>());
            DetachObject.layer = LayerMask.NameToLayer("Default");
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == 12)
        {
            DetachObject = collider.gameObject;
            //transform.parent.GetComponent<BricksConnect>().BrickHovered = true;
            UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(new ("TriggerTouch"), out value);
            if(value)
            {
                collider.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material 
                    = Resources.Load("Materials/MaterialPreview") as Material;
                //Debug.Log("trigger pressed: " + value);
            }
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.layer == 12)
        {
            DetachObject = null;
            //transform.parent.GetComponent<BricksConnect>().BrickHovered = false;
            collider.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material 
                = Resources.Load("Materials/RAL_2002") as Material;
        }
    }
}
