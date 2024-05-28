using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnableSelectFlag : MonoBehaviour
{
    private XRDirectInteractor Selected;
    public void EableFlag(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.GetComponent<BrickCollisionHandler>().IsSelectedFlag = true;
        //Debug.Log("selected: " + Selected.GetOldestInteractableSelected().transform.name);
        //Debug.Log($"{args.interactorObject} selected over {args.interactableObject}", this);
    }
    public void DisableFlag(SelectExitEventArgs args)
    {
        args.interactableObject.transform.GetComponent<BrickCollisionHandler>().IsSelectedFlag = false;
        //Debug.Log($"{args.interactorObject} stopped selected over {args.interactableObject}", this);
    }

}
