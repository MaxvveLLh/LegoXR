using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectStructure : MonoBehaviour
{
    public Dictionary<string, List<GameObject>[]> Graph = new Dictionary<string, List<GameObject>[]>();

    public void TestGraph(){
		foreach (var kvp in Graph) {
            Debug.Log("Key = " + kvp.Key + "-------------");
			Debug.Log("female: ");
			foreach (var female in kvp.Value[0]){
                if(female != null)
				    Debug.Log(female.name);
			}
			Debug.Log("male: ");
			foreach (var male in kvp.Value[1]){
                if(male != null)
				    Debug.Log(male.name);
			}
        }
	}
}
