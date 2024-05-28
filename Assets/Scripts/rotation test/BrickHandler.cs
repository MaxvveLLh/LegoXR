using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BrickHandler : MonoBehaviour
{
    public List<GameObject> MaleList = new List<GameObject>();
    public List<GameObject> FemaleList = new List<GameObject>();
    public bool IsSelectedFlag = false; 
    public bool GeneratePreview = true;
    //private Transform Parent; //must destoryed its instance when Select Exited is called from female brick
    public GameObject Preview = null; //must set to null when BricksConnect is invoked by Select Exited.
    private bool MaleOrNot = false;

    void InitiatePreview(GameObject male, GameObject female, bool IsMale)
    {
        Debug.Log("Spawning Preview");
        if(IsMale){
            /*Preview = Instantiate(female);
            Destroy (Preview.GetComponent<BrickHandler>());
            Destroy (Preview.GetComponent<XRGrabInteractable>());
            Destroy (Preview.GetComponent<Rigidbody>());*/
            Preview = Instantiate(female.transform.Find("mesh").gameObject);
            female.GetComponent<BrickHandler>().Preview = Preview;
            Preview.AddComponent<PreviewTest>().Grabbed = female;
            Preview.GetComponent<PreviewTest>().UnGrabbed = male;
            Debug.Log("Preview Loaded: " +  female.name);
        }else{
            /*Preview = Instantiate(male);
            Destroy (Preview.GetComponent<BrickHandler>());
            Destroy (Preview.GetComponent<XRGrabInteractable>());
            Destroy (Preview.GetComponent<Rigidbody>());*/
            Preview = Instantiate(male.transform.Find("mesh").gameObject);
            male.GetComponent<BrickHandler>().Preview = Preview;
            Preview.AddComponent<PreviewTest>().Grabbed = male;
            Preview.GetComponent<PreviewTest>().UnGrabbed = female;
            Debug.Log("Preview Loaded: " +  male.name);
        }
        Preview.transform.name = "Preview";

        ///////////////////////////////////////////
        Preview.GetComponent<PreviewTest>().Anchor = new GameObject("anchor");
        Preview.GetComponent<PreviewTest>().Anchor.transform.SetParent(Preview.gameObject.transform);
        ///////////////////////////////////////////

        Preview.GetComponent<PreviewTest>().Male = male;
        Preview.GetComponent<PreviewTest>().Female = female;
        //Preview.GetComponent<PreviewTest>().Female.GetComponent<BrickHandler>().Preview = Preview;
        Preview.transform.localScale += new Vector3(7.466875f, 7.966875f, 7.566875f);
        Preview.GetComponent<MeshRenderer>().material = Resources.Load("Materials/MaterialPreview") as Material;
        Preview.GetComponent<MeshRenderer>().enabled = false;
        Preview.transform.SetParent(this.gameObject.transform);
    }
    /*Vector3 GetPreviewPosition()
    {
        //need to update this function to get correct position
        Vector3 Position = Parent.transform.position;
        return Position;
    }
    void TransformPreview(GameObject female)
    {
        Preview.GetComponent<MeshRenderer>().enabled = true;
        
        //match male's rotation
        Quaternion Rotation = this.transform.rotation;
        Preview.transform.rotation = Rotation;
        //match correct position
        Vector3 Position = GetPreviewPosition(); 
        Preview.transform.position = Position;
        
        //Debug.Log("Preview Postion: " + Position);
        //Debug.Log("Preview Rotation: " + Rotation);

    }*/

    public void OnTriggerEnterMaleStud(GameObject maleStud, GameObject femaleStud)
    {
        MaleList.Add(maleStud);
        FemaleList.Add(femaleStud);
        maleStud.GetComponent<MeshRenderer>().enabled = true;
        

        
            if(Preview == null)
            {
                GameObject GrabbedPreview;
                
                InitiatePreview(maleStud.transform.parent.gameObject, femaleStud.transform.parent.gameObject, true);
                GeneratePreview = false;
                femaleStud.transform.parent.GetComponent<BrickHandler>().GeneratePreview = false;
            }
            
        
    }
    public void OnTriggerExitMaleStud(GameObject maleStud, GameObject femaleStud)
    {
        MaleList.Remove(maleStud);
        FemaleList.Remove(femaleStud);
        maleStud.GetComponent<MeshRenderer>().enabled = false;
    }

    public void OnTriggerEnterFemaleStud(GameObject maleStud, GameObject femaleStud)
    {
        femaleStud.GetComponent<MeshRenderer>().enabled = true;
       
            /*if(GeneratePreview)
            {
                GameObject GrabbedPreview;
                if(GrabbedPreview = maleStud.transform.parent.GetComponent<BrickHandler>().Preview){
                    GrabbedPreview.GetComponent<PreviewTest>().SetToNull();
                    Destroy(GrabbedPreview);
                }
                InitiatePreview(maleStud.transform.parent.gameObject, femaleStud.transform.parent.gameObject, false);
                GeneratePreview = false;
                maleStud.transform.parent.GetComponent<BrickHandler>().GeneratePreview = false;
            }
            if(MaleOrNot == true)
            {
                Preview.GetComponent<PreviewTest>().Male = maleStud.transform.parent.gameObject; 
                Preview.GetComponent<PreviewTest>().Female = this.gameObject; 
            }
            MaleOrNot = false;*/
        
    }
    public void OnTriggerExitFemaleStud(GameObject maleStud, GameObject femaleStud)
    {
        
        femaleStud.GetComponent<MeshRenderer>().enabled = false;
    }
}
