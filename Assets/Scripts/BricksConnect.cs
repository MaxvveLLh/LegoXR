using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BricksConnect : MonoBehaviour
{
    public GameObject Preview;
    public bool BrickHovered;

    public void BricksFixedConnect(SelectExitEventArgs args)
    {
        if(Preview = args.interactableObject.transform.GetComponent<BrickCollisionHandler>().Preview)
        //if(Preview = GameObject.Find("Preview"))
        {
            //get male and female
            GameObject maleBrick = Preview.GetComponent<PreviewScript>().Male;
            GameObject femaleBrick = Preview.GetComponent<PreviewScript>().Female;
            GameObject grabbedBrick = Preview.GetComponent<PreviewScript>().Grabbed;
            
            if(!Preview.GetComponent<PreviewScript>().BlueOrNot){
                Preview.GetComponent<PreviewScript>().SetToNull();
                Destroy(Preview);
                grabbedBrick.GetComponent<BrickCollisionHandler>().IsSelectedFlag = false;
                grabbedBrick.GetComponent<BrickCollisionHandler>().PreviewLock = false;
            }
            else if(Preview.GetComponent<PreviewScript>().BlueOrNot) //&& list should not be empty
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
                //Debug.Log(Preview.GetComponent<PreviewScript>().UnGrabbed.transform.position);
                //grabbedBrick.transform.position = Preview.GetComponent<PreviewScript>().GetPreviewPosition();
                grabbedBrick.transform.position = Preview.transform.position;
                grabbedBrick.transform.rotation = Preview.transform.rotation;
                femaleBrick.AddComponent<FixedJoint>().connectedBody = maleBrick.GetComponent<Rigidbody>();
                Debug.Log("here is the previous frame------------------");
                foreach (GameObject maleStud in grabbedBrick.GetComponent<BrickCollisionHandler>().MaleList)
                {
                    Debug.Log(maleStud.transform.name);
                }
                foreach (GameObject femaleStud in grabbedBrick.GetComponent<BrickCollisionHandler>().FemaleList)
                {
                    Debug.Log(femaleStud.transform.name);
                }
                //Debug.Log(Preview.GetComponent<PreviewScript>().UnGrabbed.transform.position);

                //coroutine here, let the list collect studs in the next frame before delete
                //use the list to get all touched bricks
                grabbedBrick.GetComponent<BrickCollisionHandler>().PreviewLock = true;
                StartCoroutine(ConnectPostProcess(grabbedBrick, maleBrick, femaleBrick));
                Debug.Log("PostProcess finished");
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
            
            //destory preview
            //set preview to null in male bricks
            //clear lists if possible
            //diabled connected studs (both male and female)            //what is not all studs connected when SelectExited?
            
            
        }else{
            Debug.Log("Nothing Selected");
            args.interactableObject.transform.GetComponent<BrickCollisionHandler>().IsSelectedFlag = false;
        }
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

    List<GameObject>[] ArrayOfList(List<GameObject> FemaleBricksList, List<GameObject> MaleBricksList){
		if(FemaleBricksList == null){
            List<GameObject>[] list = {new List<GameObject>(){null}, MaleBricksList};
            return list;
        }else{
            List<GameObject>[] list = {FemaleBricksList, new List<GameObject>(){null}};
            return list;
        }
		
	}

    //bug: not adding structure obj to the top one 
    void AddGraph(List<GameObject> FemaleBricksList, List<GameObject> MaleBricksList, List<GameObject> StructureList,
                        GameObject grabbedBrick, GameObject maleBrick, GameObject femaleBrick){
        if (StructureList.Count == 0){
            //Dictionary<string, List<GameObject>[]> Graph = new Dictionary<string, List<GameObject>[]>();

            GameObject ConnectStructure = new GameObject("ConnectStructure");
            ConnectStructure.AddComponent<ConnectStructure>();
            Dictionary<GameObject, List<GameObject>[]> Graph = ConnectStructure.GetComponent<ConnectStructure>().Graph;

            foreach (GameObject female in FemaleBricksList){
                Graph.Add(female, ArrayOfList(null, MaleBricksList));
                female.GetComponent<BrickCollisionHandler>().ConnectStructure = ConnectStructure;
            } 
            foreach (GameObject male in MaleBricksList){
                Graph.Add(male, ArrayOfList(FemaleBricksList, null));
                male.GetComponent<BrickCollisionHandler>().ConnectStructure = ConnectStructure;
            }
            Debug.Log("Added new graph-------------------");
        }
        //need to modify to not compare grabbed, but the outer one from GetStructureList function
        else if(StructureList.Count == 1){
            if (grabbedBrick == femaleBrick){
                Dictionary<GameObject, List<GameObject>[]> Graph 
                    = maleBrick.GetComponent<BrickCollisionHandler>().ConnectStructure.GetComponent<ConnectStructure>().Graph;
                foreach (GameObject brick in MaleBricksList){
                    if(Graph[brick][0][0] == null){
                        Graph[brick][0].Remove(null);
                    }
                    Graph[brick][0].Add(femaleBrick);
                }
                Graph.Add(femaleBrick, ArrayOfList(null, MaleBricksList));
                grabbedBrick.GetComponent<BrickCollisionHandler>().ConnectStructure = maleBrick.GetComponent<BrickCollisionHandler>().ConnectStructure;
            }
            else if(grabbedBrick == maleBrick){
                Dictionary<GameObject, List<GameObject>[]> Graph 
                    = femaleBrick.GetComponent<BrickCollisionHandler>().ConnectStructure.GetComponent<ConnectStructure>().Graph;
                foreach (GameObject brick in FemaleBricksList){
                    if(Graph[brick][1][0] == null){
                        Graph[brick][1].Remove(null);
                    }
                    Graph[brick][1].Add(maleBrick);
                }
                grabbedBrick.GetComponent<BrickCollisionHandler>().ConnectStructure = femaleBrick.GetComponent<BrickCollisionHandler>().ConnectStructure;
                Graph.Add(maleBrick, ArrayOfList(FemaleBricksList, null));
            }
            Debug.Log("Added to exist graph-------------------");
        }
        else if (StructureList.Count > 1){
            //have to determine how to mix two graphs together
        }
    }

    IEnumerator ConnectPostProcess(GameObject grabbedBrick, GameObject maleBrick, GameObject femaleBrick)
    {
        yield return null;
        Debug.Log("here is the next frame----------------");
        //yield return null;
        foreach (GameObject maleStud in grabbedBrick.GetComponent<BrickCollisionHandler>().MaleList)
        {
            Debug.Log(maleStud.transform.name);
        }
        foreach (GameObject femaleStud in grabbedBrick.GetComponent<BrickCollisionHandler>().FemaleList)
        {
            Debug.Log(femaleStud.transform.name);
        }
        
        List<GameObject> FemaleBricksList = GetBrickList(grabbedBrick.GetComponent<BrickCollisionHandler>().FemaleList);
        List<GameObject> MaleBricksList = GetBrickList(grabbedBrick.GetComponent<BrickCollisionHandler>().MaleList);

        List<GameObject> StructureList = GetStructureList(FemaleBricksList, MaleBricksList);

        AddGraph(FemaleBricksList, MaleBricksList, StructureList, grabbedBrick, maleBrick, femaleBrick);
        maleBrick.GetComponent<BrickCollisionHandler>().ConnectStructure.GetComponent<ConnectStructure>().TestGraph();
        /*Spwan graph
        *   //Dictionary<string, List<GameObject>[]> ConnectStructure = 
        *   //        new Dictionary<string, List<GameObject>[]>();
        *   //List<GameObject>[] connect = {MaleBricksList, FemaleBricksList};
        *   //ConnectStructure.Add("name", connect);

        *
        if (GraphList.Count == 0){         //under this situation, both side must be single object? NOT TRUE
            GameObject graph = Instantiate(graph gameObject);
            Dictionary<string, List<GameObject>[]> ConnectStructure = 
                new Dictionary<string, List<GameObject>[]>();
            
            foreach female in FemaleBricksList{
                ConnectStructure.Add("female.name", {new List<GameObject>(){null}, MaleBricksList});
            } 
            foreach male in MaleBricksList{
                ConnectStructure.Add("male.name", {FemaleBricksList, new List<GameObject>(){null}});
            }
            graph.getcomponent<graph>().graph = ConnectStructure;
            assign graph to every brick();
        }else if(GraphList.Count == 1){
            ConnectStructure = graph.getcomponent<graph>().graph;
            if (grabbedBrick == femaleBrick){
                foreach brick in MaleBricksList{
                    if(ConnectStructure[brick.name][0][0] == null){
                        ConnectStructure[brick.name][0].Remove("null");
                    }
                    ConnectStructure[brick.name][0].Add(female);
                }
                ConnectStructure.Add("female.name", {new List<GameObject>(){null}, MaleBricksList});
            }
            else if(grabbedBrick == maleBrick){
                foreach brick in FemaleBricksList{
                    if(ConnectStructure[brick.name][1][0] == null){
                        ConnectStructure[brick.name][1].Remove("null");
                    }
                    ConnectStructure[brick.name][1].Add(male);
                }
                ConnectStructure.Add("male.name", {FemaleBricksList, new List<GameObject>(){null}});
            }
        }else if (GraphList.Count > 1){
            //have to determine how to mix two graphs together
        }    */

        


        //Debug.Log(grabbedBrick.GetComponent<BrickCollisionHandler>().MaleList);
        //Debug.Log(grabbedBrick.GetComponent<BrickCollisionHandler>().FemaleList);
        Preview.GetComponent<PreviewScript>().SwitchLayerToConnectedStuds();
        maleBrick.layer = LayerMask.NameToLayer("connected bricks");
        femaleBrick.layer = LayerMask.NameToLayer("connected bricks");

        Debug.Log("Connected " + femaleBrick +" to " + maleBrick);
        Preview.GetComponent<PreviewScript>().SetToNull();
        Destroy(Preview);
        grabbedBrick.GetComponent<BrickCollisionHandler>().IsSelectedFlag = false;
        grabbedBrick.GetComponent<BrickCollisionHandler>().PreviewLock = false;
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