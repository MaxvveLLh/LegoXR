using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RenderStreaming;

public class WRTC : MonoBehaviour
{
     [SerializeField] RenderStreaming renderStreaming;

     // Start is called before the first frame update
     void Start()
     {
          if (!renderStreaming.runOnAwake)
          {
               renderStreaming.Run();
          }
     }
}
