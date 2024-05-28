using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.XR;
//import System.Linq;

public class BrickDetach : MonoBehaviour
{
    bool value;
    public GameObject indicator;
    private GameObject DetachObject = null;
    Material m_Material = null;
    bool TriggerPressed = false;
    bool TriggerTouched = false;
    bool GrabPressed = false;
    Dictionary<GameObject, Dictionary<GameObject, List<GameObject>>[]> Graph;

    void FixedUpdate()
    {
        
        UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.gripButton, out GrabPressed);
        if (GrabPressed){
            indicator.GetComponent<MeshRenderer>().enabled = false;
            if(DetachObject != null && m_Material != null){
                DetachObject.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material = m_Material;
            }
            m_Material = null;
            DetachObject = null;
        }else{
            UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(new ("TriggerTouch"), out TriggerTouched);
            
            if(TriggerTouched)
            {
                indicator.GetComponent<MeshRenderer>().enabled = true;
            }else{
                indicator.GetComponent<MeshRenderer>().enabled = false;
            }

            UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out TriggerPressed);
            if (DetachObject != null && TriggerPressed)
            {
                //DetachObject.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material 
                //    = Resources.Load("Materials/RAL_2002") as Material;
                DetachObject.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material = m_Material;
                //DestoryJoints();
                //Destroy(DetachObject.GetComponent<FixedJoint>());
                DetachObject.layer = LayerMask.NameToLayer("Default");

                Graph = DetachObject.GetComponent<BrickCollisionHandler>().ConnectStructure.GetComponent<ConnectStructure>().Graph;
                SwitchStudLayerAndRemoveFromGraph();
                m_Material = null;
                DetachObject = null;
            }
        }
        
    }

    void DestoryJoints(GameObject male, GameObject connectedFemale){
        FixedJoint[] fixedJoints;

        fixedJoints = male.GetComponents<FixedJoint>();

        //foreach (HingeJoint joint in hingeJoints)
        //    joint.useSpring = false;
        if (fixedJoints.Length != 0){
            for (int i = 0; i < fixedJoints.Length; i++){
                if (fixedJoints[i].connectedBody == connectedFemale.GetComponent<Rigidbody>())
                Destroy(fixedJoints[i]);
            }
        }
    }
    void SwitchStudLayerAndRemoveFromGraph(){
        foreach(var attachedFemale in Graph[DetachObject][0]){ //attachedFemale is a pair of attached female brick(Key) 
                                                               //and a list(Value) of self studs that connects to the female brick
            
            DestoryJoints(DetachObject, attachedFemale.Key);
            foreach(var mBrickStud in attachedFemale.Value){
                mBrickStud.layer = LayerMask.NameToLayer("male stud");
            }
            List<GameObject> keyList = new List<GameObject>(this.Graph[attachedFemale.Key][1].Keys);
            foreach(var attachedMale in keyList){
                if(attachedMale == DetachObject){
                    foreach(var fBrickStud in Graph[attachedFemale.Key][1][attachedMale]){
                        fBrickStud.layer = LayerMask.NameToLayer("female stud");
                    }
                    Graph[attachedFemale.Key][1].Remove(attachedMale);
                }
            }
        }

        foreach(var attachedMale in Graph[DetachObject][1]){ 
            foreach(var fBrickStud in attachedMale.Value){
                fBrickStud.layer = LayerMask.NameToLayer("female stud");
            }
            List<GameObject> keyList = new List<GameObject>(this.Graph[attachedMale.Key][0].Keys);
            foreach(var attachedFemale in keyList){
                if(attachedFemale == DetachObject){
                    foreach(var mBrickStud in Graph[attachedMale.Key][0][attachedFemale]){
                        mBrickStud.layer = LayerMask.NameToLayer("male stud");
                    }
                    DestoryJoints(attachedMale.Key, attachedFemale);
                    Graph[attachedMale.Key][0].Remove(attachedFemale);
                }
            }
        }

        Graph.Remove(DetachObject);
        //DetachObject.GetComponent<BrickCollisionHandler>().ConnectStructure.GetComponent<ConnectStructure>().TestGraph();
        //Destroy(DetachObject.GetComponent<ConnectStructure>());
        Debug.Log("============== Detached " + DetachObject +" ============== ");
        DetachObject.GetComponent<BrickCollisionHandler>().ConnectStructure = null;
        if(Graph.Count == 1){
            foreach (var last in Graph){
                Destroy(last.Key.GetComponent<BrickCollisionHandler>().ConnectStructure);
                last.Key.GetComponent<BrickCollisionHandler>().ConnectStructure = null;
                last.Key.layer = LayerMask.NameToLayer("Default");
            }
        }
        Graph = null;
        DetachObject = null;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == 12 && !GrabPressed)
        {
            if (DetachObject == null){
                DetachObject = collider.gameObject;
                //transform.parent.GetComponent<BricksConnect>().BrickHovered = true;
                //UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(new ("TriggerTouch"), out value);
                if(TriggerTouched)
                {
                    m_Material = collider.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material;
                    collider.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material 
                        = Resources.Load("Materials/MaterialPreview") as Material;
                    //Debug.Log("trigger pressed: " + value);
                }
            }
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.layer == 12 && !GrabPressed)
        {
            DetachObject = null;
            //transform.parent.GetComponent<BricksConnect>().BrickHovered = false;
            //collider.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material 
            //    = Resources.Load("Materials/RAL_2002") as Material;
            if(m_Material != null){
                collider.transform.Find("mesh").gameObject.GetComponent<MeshRenderer>().material = m_Material;
            }
        }
    }
}
