using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RenderStreaming;

public class RenderCamera : MonoBehaviour
{
     [SerializeField] Transform origin;

    // Start is called before the first frame update
    void Start()
    {
          this.transform.position = Vector3.zero;
          this.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
          this.transform.position = origin.position;
          this.transform.rotation = origin.rotation;
    }
}
