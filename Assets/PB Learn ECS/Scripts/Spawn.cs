using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject sheepPrefabs;
    [SerializeField] int numSheep = 2000;
    
    [SerializeField] BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numSheep; i++)
        {
            Vector3 pos = GetRandomPointInsideCollider(boxCollider);
            Instantiate(sheepPrefabs,pos,Quaternion.identity);
        }
    }
    public Vector3 GetRandomPointInsideCollider( BoxCollider boxCollider )
    {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range( -extents.x, extents.x ),
            Random.Range( -extents.y, extents.y ),
            Random.Range( -extents.z, extents.z )
        )  + boxCollider.center;
        return boxCollider.transform.TransformPoint( point );
    }
    
}
