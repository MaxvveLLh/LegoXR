using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BrickCollisionHandler : MonoBehaviour
{
    public List<GameObject> MaleList = new List<GameObject>();
    public List<GameObject> FemaleList = new List<GameObject>();
    public bool IsSelectedFlag = false; 
    public bool PreviewLock = false;
    public bool GeneratePreview = true;
    //private Transform Parent; //must destoryed its instance when Select Exited is called from female brick
    public GameObject Preview = null; //must set to null when BricksConnect is invoked by Select Exited.
    private bool MaleOrNot = false;
    public GameObject ConnectStructure = null;

    void InitiatePreview(GameObject male, GameObject female, bool IsMale)
    {
        //Parent = female.transform.parent;
        if(IsMale){
            //Preview = new GameObject("Preview");
            /*
            foreach (var brick in structure){  //change dictionary's string to gameobject 
                preview + brick.mesh & transform; v
            }
            */
            Preview = Instantiate(female.transform.Find("mesh").gameObject);
            female.GetComponent<BrickCollisionHandler>().Preview = Preview;
            Preview.AddComponent<PreviewScript>().Grabbed = female;
            Preview.GetComponent<PreviewScript>().UnGrabbed = male;
            Debug.Log("Preview Loaded female model: " +  female.name);
        }else{
            Preview = Instantiate(male.transform.Find("mesh").gameObject);
            male.GetComponent<BrickCollisionHandler>().Preview = Preview;
            Preview.AddComponent<PreviewScript>().Grabbed = male;
            Preview.GetComponent<PreviewScript>().UnGrabbed = female;
            Debug.Log("Preview Loaded male model: " +  male.name);
        }
        Preview.transform.name = "Preview";
        ///////////////////////////////////////////
        Preview.GetComponent<PreviewScript>().Anchor = new GameObject("anchor");
        Preview.GetComponent<PreviewScript>().Anchor.transform.SetParent(Preview.gameObject.transform);
        ///////////////////////////////////////////
        Preview.GetComponent<PreviewScript>().Male = male;
        Preview.GetComponent<PreviewScript>().Female = female;
        //Preview.GetComponent<PreviewScript>().Female.GetComponent<BrickCollisionHandler>().Preview = Preview;
        Preview.transform.localScale += new Vector3(-0.8f, -0.8f, -0.8f);
        Preview.GetComponent<MeshRenderer>().material = Resources.Load("Materials/MaterialPreview") as Material;
        Preview.GetComponent<MeshRenderer>().enabled = false;
        Preview.transform.SetParent(this.gameObject.transform);
    }

    public void OnTriggerEnterMaleStud(GameObject maleStud, GameObject femaleStud)
    {
        if(IsSelectedFlag){
            MaleList.Add(maleStud);
            FemaleList.Add(femaleStud);
        }
        //maleStud.GetComponent<MeshRenderer>().enabled = true;
        
        //if(femaleStud.transform.parent.GetComponent<BrickCollisionHandler>().IsSelectedFlag)
        if(femaleStud.transform.parent.GetComponent<BrickCollisionHandler>().IsSelectedFlag && !femaleStud.transform.parent.GetComponent<BrickCollisionHandler>().PreviewLock)
        {
            if(GeneratePreview)
            {
                if(IsNotDuplicated(femaleStud.transform.parent.gameObject, maleStud.transform.parent.gameObject)){
                    GameObject GrabbedPreview;
                    if(GrabbedPreview = femaleStud.transform.parent.GetComponent<BrickCollisionHandler>().Preview){
                        Debug.Log("destory preview of grabbed: " + GrabbedPreview.GetComponent<PreviewScript>().Grabbed.name);
                        GrabbedPreview.GetComponent<PreviewScript>().SetToNull();
                        Destroy(GrabbedPreview);
                    }
                    InitiatePreview(maleStud.transform.parent.gameObject, femaleStud.transform.parent.gameObject, true);
                    GeneratePreview = false;
                    femaleStud.transform.parent.GetComponent<BrickCollisionHandler>().GeneratePreview = false;
                }
            }
            
            if(MaleOrNot == false)
            {
                Preview.GetComponent<PreviewScript>().Male = this.gameObject; 
                Preview.GetComponent<PreviewScript>().Female = femaleStud.transform.parent.gameObject; 
            }
            MaleOrNot = true;
        }
    }
    public void OnTriggerExitMaleStud(GameObject maleStud, GameObject femaleStud)
    {
        if(IsSelectedFlag){
            MaleList.Remove(maleStud);
            FemaleList.Remove(femaleStud);
        }
        //maleStud.GetComponent<MeshRenderer>().enabled = false; 
    }

    public void OnTriggerEnterFemaleStud(GameObject maleStud, GameObject femaleStud)
    {
        if(IsSelectedFlag){
            MaleList.Add(maleStud);
            FemaleList.Add(femaleStud);
        }
        //femaleStud.GetComponent<MeshRenderer>().enabled = true;
        //if(maleStud.transform.parent.GetComponent<BrickCollisionHandler>().IsSelectedFlag)
        if(maleStud.transform.parent.GetComponent<BrickCollisionHandler>().IsSelectedFlag && !maleStud.transform.parent.GetComponent<BrickCollisionHandler>().PreviewLock)
        {
            if(GeneratePreview)
            {
                if(IsNotDuplicated(maleStud.transform.parent.gameObject, femaleStud.transform.parent.gameObject)){
                    GameObject GrabbedPreview;
                    if(GrabbedPreview = maleStud.transform.parent.GetComponent<BrickCollisionHandler>().Preview){
                        Debug.Log("destory preview of grabbed: " + GrabbedPreview.GetComponent<PreviewScript>().Grabbed.name);
                        GrabbedPreview.GetComponent<PreviewScript>().SetToNull();
                        Destroy(GrabbedPreview);
                    }
                    InitiatePreview(maleStud.transform.parent.gameObject, femaleStud.transform.parent.gameObject, false);
                    GeneratePreview = false;
                    maleStud.transform.parent.GetComponent<BrickCollisionHandler>().GeneratePreview = false;
                }
            }
            
            if(MaleOrNot == true)
            {
                Preview.GetComponent<PreviewScript>().Male = maleStud.transform.parent.gameObject; 
                Preview.GetComponent<PreviewScript>().Female = this.gameObject; 
            }
            MaleOrNot = false;
        }
    }
    public void OnTriggerExitFemaleStud(GameObject maleStud, GameObject femaleStud)
    {
        if(IsSelectedFlag){
            MaleList.Remove(maleStud);
            FemaleList.Remove(femaleStud);
        }
        //femaleStud.GetComponent<MeshRenderer>().enabled = false;
    }

    public void PostProcess(){
        IsSelectedFlag = false;
        PreviewLock = false;
        MaleList.Clear();
        FemaleList.Clear();
    }

    bool IsNotDuplicated(GameObject grabbed, GameObject ungrabbed){
        if (ungrabbed.GetComponent<BrickCollisionHandler>().ConnectStructure != null){
            if (ungrabbed.GetComponent<BrickCollisionHandler>().ConnectStructure.GetComponent<ConnectStructure>().Graph.ContainsKey(grabbed)){
                return false;
            }
        }
        Debug.Log("ok to generate preview");
        return true;
    } 
}
