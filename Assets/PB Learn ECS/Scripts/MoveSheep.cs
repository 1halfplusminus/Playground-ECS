using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSheep : MonoBehaviour
{
 
    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0.0f,0.0f,0.1f);
        if(this.transform.position.z > 20) {
            this.transform.position = new Unity.Mathematics.float3(this.transform.position.x,this.transform.position.y,-51.4f);
        }
    }
}
