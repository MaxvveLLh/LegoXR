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
    public bool BlueOrNot = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Male.GetComponent<BrickCollisionHandler>().MaleList.Count == 0)
        {
            //if assigned male and female are not colliding: 
            BlueOrNot = false;
            this.GetComponent<MeshRenderer>().enabled = false;
        }else{
            BlueOrNot = true;
            TransformPreview();
            this.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    public Vector3 GetPreviewPosition()
    {
        Vector3 Difference;
        if(Grabbed == Female){
            Anchor.transform.localPosition = Male.GetComponent<BrickCollisionHandler>().FemaleList[0].transform.localPosition;
            Difference =  Anchor.transform.position
             - Male.GetComponent<BrickCollisionHandler>().MaleList[0].transform.position;  
        }else{
            Anchor.transform.localPosition = Male.GetComponent<BrickCollisionHandler>().MaleList[0].transform.localPosition;
            Difference = Anchor.transform.position
             - Male.GetComponent<BrickCollisionHandler>().FemaleList[0].transform.position;
        }
        Vector3 Position = this.transform.position-Difference;// should use preview's stud difference (after rotation) to calculate diff
        return Position;
    }

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

    public void GetPreviewRotation()
    {
        //match UnGrabbed's rotation
        Quaternion Rotation = UnGrabbed.transform.rotation;
        this.transform.rotation = Rotation;

        float angle = GetSignedAngle(Grabbed.transform.rotation, UnGrabbed.transform.rotation, Vector3.up);
        if(45 < angle && angle < 135){
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        }else if(-45 > angle && angle > -135){
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }else if((angle < 0 ? -angle : angle) > 135){
            transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
    }

    void TransformPreview()
    {
        //match UnGrabbed's rotation
        GetPreviewRotation();
        //match correct position
        Vector3 Position = GetPreviewPosition(); 
        this.transform.position = Position;
    }


    public void SwitchLayerToConnectedStuds()
    {
        foreach (GameObject maleStud in Male.GetComponent<BrickCollisionHandler>().MaleList)
        {
            maleStud.layer = LayerMask.NameToLayer("connected studs");
        }
        foreach (GameObject femaleStud in Male.GetComponent<BrickCollisionHandler>().FemaleList)
        {
            femaleStud.layer = LayerMask.NameToLayer("connected studs");
        }
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
