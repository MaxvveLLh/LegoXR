using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewTest : MonoBehaviour
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
        if (Male.GetComponent<BrickHandler>().MaleList.Count == 0)
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

    public Vector3 GetPreviewPosition()
    {
        //need to update this function to get correct position
        //currently using Grabbed's position to test basic function
        Vector3 Difference;
        if(Grabbed == Female){
            Anchor.transform.localPosition = Male.GetComponent<BrickHandler>().FemaleList[0].transform.localPosition;
            Difference =  Anchor.transform.position
             - Male.GetComponent<BrickHandler>().MaleList[0].transform.position; 
        }else{
            Anchor.transform.localPosition = Male.GetComponent<BrickHandler>().MaleList[0].transform.localPosition;
            Difference = Anchor.transform.position
             - Male.GetComponent<BrickHandler>().FemaleList[0].transform.position; 
        }
        Vector3 Position = this.transform.position-Difference;// should use preview's stud difference (after rotation) to calculate diff
        return Position;
    }

    public void GetPreviewRotation()
    {
        //match UnGrabbed's rotation
        Quaternion Rotation = UnGrabbed.transform.rotation;
        this.transform.rotation = Rotation;
        //Debug.Log("Grabbed angle: " + Grabbed.transform.rotation + " UnGrabbed: " + UnGrabbed.transform.rotation);
        //Debug.Log("Grabbed angle: " + Grabbed.transform.forward + " UnGrabbed: " + UnGrabbed.transform.forward);
        //float angle = Quaternion.Angle (Grabbed.transform.rotation, UnGrabbed.transform.rotation);
        //float angle = Vector3.SignedAngle(Grabbed.transform.rotation.eulerAngles, UnGrabbed.transform.rotation.eulerAngles, Vector3.up);
        //float angle = Vector3.Angle(Grabbed.transform.rotation.eulerAngles, UnGrabbed.transform.rotation.eulerAngles);
        float angle = GetSignedAngle(Grabbed.transform.rotation, UnGrabbed.transform.rotation, Vector3.up);
        //angle = angle < 0 ? -angle : angle;
        Debug.Log("Angle: "+angle);
        //Debug.Log(Grabbed.transform.rotation.eulerAngles);
        //Debug.Log(UnGrabbed.transform.rotation.eulerAngles);
        if(45 < angle && angle < 135){
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        }else if(-45 > angle && angle > -135){
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }else if((angle < 0 ? -angle : angle) > 135){
            transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        
        /*else if(225 < angle && angle < 315)
        {
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        }*/
    }

    void TransformPreview()
    {
        //match UnGrabbed's rotation
        GetPreviewRotation();
        //Vector3 UngrabbedNormal = Vector3.Cross(transform.position-Male.GetComponent<BrickHandler>().MaleList[0].transform.position, 
        //transform.position-Male.GetComponent<BrickHandler>().MaleList[1].transform.position);
        //transform.rotation = Quaternion.FromToRotation (transform.up, UngrabbedNormal) * transform.rotation;
        //match correct position
        Vector3 Position = GetPreviewPosition(); 
        this.transform.position = Position;
        
    }


    public void SwitchLayerToConnectedStuds()
    {
        foreach (GameObject maleStud in Male.GetComponent<BrickHandler>().MaleList)
        {
            maleStud.layer = LayerMask.NameToLayer("connected studs");
        }
        foreach (GameObject femaleStud in Male.GetComponent<BrickHandler>().FemaleList)
        {
            femaleStud.layer = LayerMask.NameToLayer("connected studs");
        }
    }

    public void SetToNull()
    {
        Male.GetComponent<BrickHandler>().Preview = null;
        Male.GetComponent<BrickHandler>().GeneratePreview = true;
        Female.GetComponent<BrickHandler>().Preview = null;
        Female.GetComponent<BrickHandler>().GeneratePreview = true;
        Grabbed.GetComponent<BrickHandler>().Preview = null;
        UnGrabbed.GetComponent<BrickHandler>().Preview = null;
    }
}
