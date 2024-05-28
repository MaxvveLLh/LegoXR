using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewScript : MonoBehaviour
{
    public GameObject Male;
    public GameObject Female;

    public GameObject Grabbed;
    public GameObject UnGrabbed;
    
    public GameObject Anchor;
    public bool IsBlue = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Grabbed.GetComponent<BrickCollisionHandler>().MaleList.Count == 0)
        {
            //if assigned male and female are not colliding, make preview invisible 
            IsBlue = false;
            this.GetComponent<MeshRenderer>().enabled = false;
        }else{
            IsBlue = true;
            TransformPreview();
            this.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    /*
    * Move the Preview to appropriate position 
    *
    * - whether grabbed is female or male does matter
    * - Anchor represents/mimics the ungrabbed brick's stud's transform property
    * - Anchor is updated in GetPreviewRotation()
    *
    * 1. Use Anchor and ungrabbed stud to calculate translate difference between grabbed and ungrabbed bricks
    * 2. Apply this difference to preview's transform.position
    */
    public Vector3 GetPreviewPosition()
    {
        Vector3 Difference;
        if(Grabbed == Female){
            Difference =  Anchor.transform.position
             - Grabbed.GetComponent<BrickCollisionHandler>().MaleList[0].transform.position;  
        }else{
            Difference = Anchor.transform.position
             - Grabbed.GetComponent<BrickCollisionHandler>().FemaleList[0].transform.position;
        }
        Vector3 Position = this.transform.position-Difference;
        return Position;
    }
    
    /*
    * Return signed angle, which is betwween A and B, along with the axis 
    *
    * - the angle's surface is perpendicular to the axis
    * - axis is usually transform.forward of the traget stud.
    */
    public static float GetSignedAngle(Quaternion A, Quaternion B, Vector3 axis) 
    {
        float angle = 0f;
        Vector3 angleAxis = Vector3.zero;
        (B*Quaternion.Inverse(A)).ToAngleAxis(out angle, out angleAxis);
            if(Vector3.Angle(axis, angleAxis) > 90f) {
                angle = -angle;
            }
        return Mathf.DeltaAngle(0f, angle);
    }

    /*
    * Return modified targetRotaion based on angle,
    *
    * - Imagine you want to connect a lego brick to another brick, with 90 degrees
    * - the modification is either 90, -90, 180 or none
    * - targetRotaion will be used to calculate RotationDifference between studs.
    */
    Quaternion TargetRotation(GameObject target, float angle){
        Quaternion targetRotaion = target.transform.rotation;
        if(45 < angle && angle < 135){
            return targetRotaion *= Quaternion.Euler(0, 0, 90);
        }else if(-45 > angle && angle > -135){
            return targetRotaion *= Quaternion.Euler(0, 0, -90);
        }else if((angle < 0 ? -angle : angle) > 135){
            return targetRotaion *= Quaternion.Euler(0, 0, 180);
        }
        return targetRotaion;
    }

    /*
    * Rotate the Preview to appropriate angles/rotation  / match UnGrabbed's rotation
    *
    * - whether grabbed is female or male does matter
    *
    * 1. Get signed angle based on the first studs in the stud list 
    * 2. Apply modification of turning anles to the ungrabbed (target) stud's rotation
    * 3. Calculate RotationDifference based on grabbed brick's stud and modified stud's rotaion 
    * 4. Apply this stud's RotationDifference to preview's transform.rotation
    */
    public void GetPreviewRotation()
    {
        Quaternion RotationDifference;
        float angle;
        
        if (Grabbed == Female){

            angle = GetSignedAngle(Grabbed.GetComponent<BrickCollisionHandler>().MaleList[0].transform.rotation, 
                                   Grabbed.GetComponent<BrickCollisionHandler>().FemaleList[0].transform.rotation, 
                                   Grabbed.GetComponent<BrickCollisionHandler>().MaleList[0].transform.forward);
            Anchor.transform.localPosition = Grabbed.GetComponent<BrickCollisionHandler>().FemaleList[0].transform.localPosition;
            RotationDifference = Grabbed.GetComponent<BrickCollisionHandler>().FemaleList[0].transform.rotation
                                * Quaternion.Inverse(TargetRotation(Grabbed.GetComponent<BrickCollisionHandler>().MaleList[0], angle));
            transform.rotation = Quaternion.Inverse(RotationDifference) * Grabbed.transform.rotation;
        }else{
            
            angle = GetSignedAngle(Grabbed.GetComponent<BrickCollisionHandler>().FemaleList[0].transform.rotation,  
                                   Grabbed.GetComponent<BrickCollisionHandler>().MaleList[0].transform.rotation, 
                                   Grabbed.GetComponent<BrickCollisionHandler>().FemaleList[0].transform.forward);
            Anchor.transform.localPosition = Grabbed.GetComponent<BrickCollisionHandler>().MaleList[0].transform.localPosition;
            RotationDifference = Grabbed.GetComponent<BrickCollisionHandler>().MaleList[0].transform.rotation
                                * Quaternion.Inverse(TargetRotation(Grabbed.GetComponent<BrickCollisionHandler>().FemaleList[0], angle));
            transform.rotation = Quaternion.Inverse(RotationDifference) * Grabbed.transform.rotation;
        }
    }
    

    void TransformPreview()
    {
        //match UnGrabbed's rotation
        GetPreviewRotation();
        //match UnGrabbed's position with connecting studs
        Vector3 Position = GetPreviewPosition(); 
        this.transform.position = Position;
        
    }


    public void SwitchLayerToConnected()
    {
        foreach (GameObject maleStud in Grabbed.GetComponent<BrickCollisionHandler>().MaleList)
        {
            maleStud.layer = LayerMask.NameToLayer("connected studs");
        }
        foreach (GameObject femaleStud in Grabbed.GetComponent<BrickCollisionHandler>().FemaleList)
        {
            femaleStud.layer = LayerMask.NameToLayer("connected studs");
        }
        Male.layer = LayerMask.NameToLayer("connected bricks");
        Female.layer = LayerMask.NameToLayer("connected bricks");
    }

    public void SetToNull()
    {
        Male.GetComponent<BrickCollisionHandler>().Preview = null;
        Male.GetComponent<BrickCollisionHandler>().GeneratePreview = true;
        Female.GetComponent<BrickCollisionHandler>().Preview = null;
        Female.GetComponent<BrickCollisionHandler>().GeneratePreview = true;
        Grabbed.GetComponent<BrickCollisionHandler>().Preview = null;
        UnGrabbed.GetComponent<BrickCollisionHandler>().Preview = null;
    }
}
