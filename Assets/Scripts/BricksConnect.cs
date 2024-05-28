using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BricksConnect : MonoBehaviour
{
    public GameObject Preview;

    void Start()
    {
    }

    void Update()
    {
    }
    public void BricksDisconnect(HoverEnterEventArgs args)
    {
        if(args.interactableObject.transform.gameObject.layer == 12)
        {
            args.interactableObject.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material 
            = Resources.Load("Materials/MaterialPreview") as Material;
        }
    }
    public void BricksDisconnectExited(HoverExitEventArgs args)
    {
        if(args.interactableObject.transform.gameObject.layer == 12)
        {
            args.interactableObject.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material 
            = Resources.Load("Materials/RAL_2002") as Material;
        }
    }
    public void BricksFixedConnect(SelectExitEventArgs args)
    {
        if(Preview = args.interactableObject.transform.GetComponent<BrickCollisionHandler>().Preview)
        //if(Preview = GameObject.Find("Preview"))
        {
            //get male and female
            GameObject maleBrick = Preview.GetComponent<PreviewScript>().Male;
            GameObject femaleBrick = Preview.GetComponent<PreviewScript>().Female;
            GameObject grabbedBrick = Preview.GetComponent<PreviewScript>().Grabbed;
            //get preview postion
            if(Preview.GetComponent<PreviewScript>().BlueOrNot) //&& list should not be empty
            {
                //Debug.Log("selected male: " + maleBrick);
                //Debug.Log("selected female: " + femaleBrick);

                //maleBrick.transform.parent.transform.name = "parent";
                if(!maleBrick.GetComponent<FixedJoint>()){
                    //Debug.Log("Male has no parent hinge");
                }else{
                     //Debug.Log("Male has parent hinge");
                }
                    //grabbedBrick.transform.rotation = Preview.GetComponent<PreviewScript>().GetPreviewRotation();
                    Debug.Log(Preview.GetComponent<PreviewScript>().UnGrabbed.transform.position);
                    //grabbedBrick.transform.position = Preview.GetComponent<PreviewScript>().GetPreviewPosition();
                    grabbedBrick.transform.position = Preview.transform.position;
                    grabbedBrick.transform.rotation = Preview.transform.rotation;
                    femaleBrick.AddComponent<FixedJoint>().connectedBody = maleBrick.GetComponent<Rigidbody>();
                    Debug.Log(Preview.GetComponent<PreviewScript>().UnGrabbed.transform.position);
                Preview.GetComponent<PreviewScript>().SwitchLayerToConnectedStuds();
                maleBrick.layer = LayerMask.NameToLayer("connected bricks");
                femaleBrick.layer = LayerMask.NameToLayer("connected bricks");
                Debug.Log("Connected " + femaleBrick +" to " + maleBrick);
            }
            Preview.GetComponent<PreviewScript>().SetToNull();
            //Preview.GetComponent<PreviewScript>().Male.GetComponent<BrickCollisionHandler>().Preview = null;
            //Preview.GetComponent<PreviewScript>().Female.GetComponent<BrickCollisionHandler>().Preview = null;
            Destroy(Preview);
            //if blue
                //if no connection locator
                    //declare new gameobject as locator                 //declare male as locator
                    //assign male and female as children of locator
                    //fixedhinge both to locator                        //if want to disconnect, assign disconnected objects together to a new locator, or to be itself
                    //configure hinge with position 
                //else 
                    //assign female as child of existing locator
                    //assign female to locator
                    //configure hinge with position                     //store them in data structure in future to represent "who connects whoms"
            
            //destory preview
            //set preview to null in male bricks
            //clear lists if possible
            //diabled connected studs (both male and female)            //what is not all studs connected when SelectExited?
            
            
        }else{
            Debug.Log("Nothing Selected");
        }
    }

    public void EableFlag(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.GetComponent<BrickCollisionHandler>().IsSelectedFlag = true;
        //Debug.Log("selected: " + Selected.GetOldestInteractableSelected().transform.name);
        //Debug.Log($"{args.interactorObject} selected over {args.interactableObject}", this);
    }
    public void DisableFlag(SelectExitEventArgs args)
    {
        args.interactableObject.transform.GetComponent<BrickCollisionHandler>().IsSelectedFlag = false;
    }

}