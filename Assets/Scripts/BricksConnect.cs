using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BricksConnect : MonoBehaviour
{
    public GameObject Preview;
    public bool BrickHovered;

    GameObject maleBrick;
    GameObject femaleBrick;
    GameObject grabbedBrick;

    public void BricksFixedConnect(SelectExitEventArgs args)
    {
        if(Preview = args.interactableObject.transform.GetComponent<BrickCollisionHandler>().Preview)
        //if(Preview = GameObject.Find("Preview"))
        {
            //get male and female
            maleBrick = Preview.GetComponent<PreviewScript>().Male;
            femaleBrick = Preview.GetComponent<PreviewScript>().Female;
            grabbedBrick = Preview.GetComponent<PreviewScript>().Grabbed;
            
            if(!Preview.GetComponent<PreviewScript>().IsBlue){
                Preview.GetComponent<PreviewScript>().SetToNull();
                Destroy(Preview);
                grabbedBrick.GetComponent<BrickCollisionHandler>().IsSelectedFlag = false;
                grabbedBrick.GetComponent<BrickCollisionHandler>().PreviewLock = false;
            }
            else if(Preview.GetComponent<PreviewScript>().IsBlue) //&& list should not be empty
            {
                //maleBrick.transform.parent.transform.name = "parent";
                if(!maleBrick.GetComponent<FixedJoint>()){
                    //Debug.Log("Male has no parent hinge");
                }else{
                     //Debug.Log("Male has parent hinge");
                }
                grabbedBrick.transform.position = Preview.transform.position;
                grabbedBrick.transform.rotation = Preview.transform.rotation;
                //femaleBrick.AddComponent<FixedJoint>().connectedBody = maleBrick.GetComponent<Rigidbody>();
                maleBrick.AddComponent<FixedJoint>().connectedBody = femaleBrick.GetComponent<Rigidbody>();
                //coroutine here, let the list collects all connected studs in the next frame before change them to connected stud layer
                grabbedBrick.GetComponent<BrickCollisionHandler>().PreviewLock = true;
                StartCoroutine(ConnectPostProcess());
                //Debug.Log("here is the previous frame------------------PostProcess finished");
            }
            
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
        }else{
            //Debug.Log("Nothing Selected");
            args.interactableObject.transform.GetComponent<BrickCollisionHandler>().IsSelectedFlag = false;
        }
    }

    IEnumerator ConnectPostProcess()
    {
        yield return null;
        //Debug.Log("here is the next frame----------------");
        
        List<GameObject> FemaleBricksList = GetBrickList(grabbedBrick.GetComponent<BrickCollisionHandler>().FemaleList);
        List<GameObject> MaleBricksList = GetBrickList(grabbedBrick.GetComponent<BrickCollisionHandler>().MaleList);
        List<GameObject> StructureList = GetStructureList(FemaleBricksList, MaleBricksList);

        AddGraph(FemaleBricksList, MaleBricksList, StructureList);
        
        Preview.GetComponent<PreviewScript>().SetToNull();
        Preview.GetComponent<PreviewScript>().SwitchLayerToConnected();
        grabbedBrick.GetComponent<BrickCollisionHandler>().PostProcess();
        Destroy(Preview);

        //maleBrick.GetComponent<BrickCollisionHandler>().ConnectStructure.GetComponent<ConnectStructure>().TestGraph();
        Debug.Log("============== Connected " + femaleBrick +" to " + maleBrick +" ==============");
    }





    List<GameObject> GetBrickList(List<GameObject> studList){
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject stud in studList)
        {
            if (!list.Contains(stud.transform.parent.gameObject)){
                list.Add(stud.transform.parent.gameObject);
            }
        }
        return list;
    }


    Dictionary<GameObject, Dictionary<GameObject, List<GameObject>>[]> GetBrickStudsDictionary(){

        Dictionary<GameObject, Dictionary<GameObject, List<GameObject>>[]> pairedList = 
                    new Dictionary<GameObject, Dictionary<GameObject, List<GameObject>>[]>();

        List<GameObject> maleStudList = grabbedBrick.GetComponent<BrickCollisionHandler>().MaleList;
        List<GameObject> femaleStudList = grabbedBrick.GetComponent<BrickCollisionHandler>().FemaleList;

        GameObject maleStud, femaleStud;
        GameObject male, female;

        for (int i = 0; i < maleStudList.Count; i++){
            maleStud = maleStudList[i];
            femaleStud = femaleStudList[i];
            male = maleStud.transform.parent.gameObject;
            female = femaleStud.transform.parent.gameObject;

            if (!pairedList.ContainsKey(male)){
                pairedList.Add(male, new[]{new Dictionary<GameObject, List<GameObject>>(), new Dictionary<GameObject, List<GameObject>>()} );
            }
            if (!pairedList.ContainsKey(female)){
                pairedList.Add(female, new[]{new Dictionary<GameObject, List<GameObject>>(), new Dictionary<GameObject, List<GameObject>>()} );
            }
            
            if(!pairedList[male][0].ContainsKey(female)){
                pairedList[male][0].Add(female, new List<GameObject>(){maleStud});
            }else{
                pairedList[male][0][female].Add(maleStud);
            }

            if(!pairedList[female][1].ContainsKey(male)){
                pairedList[female][1].Add(male, new List<GameObject>(){femaleStud});
            }else{
                pairedList[female][1][male].Add(femaleStud);
            }
        }
        return pairedList;
    }

    List<GameObject> GetStructureList(List<GameObject> FemaleBricksList, List<GameObject> MaleBricksList){
        List<GameObject> StructureList = new List<GameObject>();
        foreach (GameObject female in FemaleBricksList){
            if(female.GetComponent<BrickCollisionHandler>().ConnectStructure != null)
                if (!StructureList.Contains(female.GetComponent<BrickCollisionHandler>().ConnectStructure))
                    StructureList.Add(female.GetComponent<BrickCollisionHandler>().ConnectStructure);
        } 
        foreach (GameObject male in MaleBricksList){
           if(male.GetComponent<BrickCollisionHandler>().ConnectStructure != null)
                if (!StructureList.Contains(male.GetComponent<BrickCollisionHandler>().ConnectStructure))
                    StructureList.Add(male.GetComponent<BrickCollisionHandler>().ConnectStructure);
        }
        //need to update with null and out it. 
        return StructureList;
    }

    //bug: not adding structure obj to the top one 
    void AddGraph(List<GameObject> FemaleBricksList, List<GameObject> MaleBricksList, List<GameObject> StructureList){

        Dictionary<GameObject, Dictionary<GameObject, List<GameObject>>[]> pairedList = GetBrickStudsDictionary();

        if (StructureList.Count == 0){
            //Dictionary<string, List<GameObject>[]> Graph = new Dictionary<string, List<GameObject>[]>();

            GameObject ConnectStructure = new GameObject("ConnectStructure");
            ConnectStructure.AddComponent<ConnectStructure>();
            
            //Dictionary<GameObject, List<GameObject>[]> Graph = ConnectStructure.GetComponent<ConnectStructure>().Graph;
            Dictionary<GameObject, Dictionary<GameObject, List<GameObject>>[]> Graph = ConnectStructure.GetComponent<ConnectStructure>().Graph;
            foreach (GameObject female in FemaleBricksList){

                Graph.Add(female, new[]{new Dictionary<GameObject, List<GameObject>>(), pairedList[female][1]} );
            
                //Graph.Add(female, ArrayOfList(null, MaleBricksList));
                female.GetComponent<BrickCollisionHandler>().ConnectStructure = ConnectStructure;
            } 
            foreach (GameObject male in MaleBricksList){

                Graph.Add(male, new[]{pairedList[male][0], new Dictionary<GameObject, List<GameObject>>()} );
                //Graph.Add(male, ArrayOfList(FemaleBricksList, null));
                male.GetComponent<BrickCollisionHandler>().ConnectStructure = ConnectStructure;
            }
            Debug.Log("Added new graph-------------------");
        }
        //need to modify to not compare grabbed, but the outer one from GetStructureList function
        else if(StructureList.Count == 1){
            if (grabbedBrick == femaleBrick){
                Dictionary<GameObject, Dictionary<GameObject, List<GameObject>>[]> Graph 
                    = maleBrick.GetComponent<BrickCollisionHandler>().ConnectStructure.GetComponent<ConnectStructure>().Graph;
                foreach (GameObject mbrick in MaleBricksList){
                    //if(Graph[brick][0][0] == null){
                    //    Graph[brick][0].Remove(null);
                    //}
                    //Graph[mbrick][0].Add(femaleBrick);
                    Graph[mbrick][0].Add(femaleBrick, pairedList[mbrick][0][femaleBrick]);
                }
                Graph.Add(femaleBrick, new[]{new Dictionary<GameObject, List<GameObject>>(), pairedList[femaleBrick][1]} );
                //Graph.Add(femaleBrick, ArrayOfList(null, MaleBricksList));
                grabbedBrick.GetComponent<BrickCollisionHandler>().ConnectStructure = maleBrick.GetComponent<BrickCollisionHandler>().ConnectStructure;
            }
            else if(grabbedBrick == maleBrick){
                Dictionary<GameObject, Dictionary<GameObject, List<GameObject>>[]> Graph 
                    = femaleBrick.GetComponent<BrickCollisionHandler>().ConnectStructure.GetComponent<ConnectStructure>().Graph;
                foreach (GameObject fbrick in FemaleBricksList){
                    //if(Graph[brick][1][0] == null){
                    //    Graph[brick][1].Remove(null);
                    //}
                    //Graph[fbrick][1].Add(maleBrick);
                    Graph[fbrick][1].Add(maleBrick, pairedList[fbrick][1][maleBrick]);


                }
                Graph.Add(maleBrick, new[]{pairedList[maleBrick][0], new Dictionary<GameObject, List<GameObject>>()} );
                //Graph.Add(maleBrick, ArrayOfList(FemaleBricksList, null));
                grabbedBrick.GetComponent<BrickCollisionHandler>().ConnectStructure = femaleBrick.GetComponent<BrickCollisionHandler>().ConnectStructure;
            }
            Debug.Log("Added to exist graph-------------------");
        }
        else if (StructureList.Count > 1){
            //have to determine how to mix two graphs together
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

    
    /*List<GameObject>[] ArrayOfList(List<GameObject> FemaleBricksList, List<GameObject> MaleBricksList){
		if(FemaleBricksList == null){
            List<GameObject>[] list = {new List<GameObject>(){null}, MaleBricksList};
            return list;
        }else{
            List<GameObject>[] list = {FemaleBricksList, new List<GameObject>(){null}};
            return list;
        }
	}
    Dictionary<GameObject, List<GameObject>>[] ArrayOfDictionary(List<GameObject> FemaleBricksList, List<GameObject> MaleBricksList){
		if(FemaleBricksList == null){
            Dictionary<GameObject, List<GameObject>>[] list = {new List<GameObject>(){null}, MaleBricksList};
            return list;
        }else{
            Dictionary<GameObject, List<GameObject>>[] list = {FemaleBricksList, new List<GameObject>(){null}};
            return list;
        }
	}*/
}