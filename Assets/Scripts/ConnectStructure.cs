using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectStructure : MonoBehaviour
{
    //public Dictionary<GameObject, List<GameObject>[]> Graph = new Dictionary<GameObject, List<GameObject>[]>();
	public Dictionary<GameObject, Dictionary<GameObject, List<GameObject>>[]> Graph 
										= new Dictionary<GameObject, Dictionary<GameObject, List<GameObject>>[]>();

	
    public void TestGraph(){
		Debug.Log("***************************Print Start***************************");
		
		int count = 1;
		Debug.Log("Graph elements count: " + Graph.Count);
		foreach (var kvp in Graph) {
            Debug.Log("============ " + count + "th KEY = " + kvp.Key.name + " ============");

			Debug.Log("------ Female List: ------");
			foreach (var female in kvp.Value[0]){
                //if(female != null)
				Debug.Log("+---- " + female.Key + " ----+");
				foreach (var stud in female.Value){
					Debug.Log("- " + stud);
				}
			}

			Debug.Log("------ Male List: ------");
			foreach (var male in kvp.Value[1]){
                //if(male != null)
				Debug.Log("+---- " + male.Key + " ----+");
				foreach (var stud in male.Value){
					Debug.Log("- " + stud);
				}
			}
			count++;
        }
		Debug.Log("***************************Print Finish***************************");
	}
}
