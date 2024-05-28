using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/*namespace UnityEngine.UI
{

    //ontrigger2d
        //if button and hand collide
    // if commonusage.triggerbutton is pressed
        //onclick
}*/

public class ButtonXR : MonoBehaviour
{
    public bool PrevPage;
    

    private InputDevice device;
    void OnTriggerEnter(Collider collider)
    {
        //Touched = true;
        //InputDevices.GetDevicesAtXRNode(XRNode.RightHand, device);
        this.transform.localScale = this.transform.localScale * 1.2f;
    }
    void OnTriggerExit(Collider collider)
    {
        this.transform.localScale = this.transform.localScale / 1.2f;
        if(PrevPage)
            this.GetComponent<TabButton>().PrevPageClick();
        else
            this.GetComponent<TabButton>().NextPageClick();
        //Touched = false;
        //this.GetComponent<Image>().OnTriggerExitMaleStud(this.gameObject, collider.gameObject);
    }
}
