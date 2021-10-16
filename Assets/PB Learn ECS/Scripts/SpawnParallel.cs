using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Entities;
using Unity.Burst;

public class SpawnParallel : MonoBehaviour
{
    [BurstCompile]
    struct MoveSheepJob : IJobParallelForTransform
    {

        public void Execute(int index, TransformAccess transform)
        {
            transform.position += 0.1f * (transform.rotation * new Vector3(0,0,1));
            if(transform.position.z > 20) {
                transform.position = new Unity.Mathematics.float3(transform.position.x,transform.position.y,-51.4f);
            }
        }
    }
    [SerializeField] GameObject sheepPrefabs;
    [SerializeField] int numSheep = 2000;
    
    [SerializeField] BoxCollider boxCollider;

    MoveSheepJob moveJob;
    JobHandle handle;

    TransformAccessArray transforms;
    // Start is called before the first frame update
    void Start()
    {
        Transform[] sheeps = new Transform[numSheep];
        for (int i = 0; i < numSheep; i++)
        {
            Vector3 pos = GetRandomPointInsideCollider(boxCollider);
            var sheep = Instantiate(sheepPrefabs,pos,Quaternion.identity);
            sheeps[i] = sheep.transform;
        }

        transforms = new TransformAccessArray(sheeps);
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
    void MoveSheep(GameObject ob) {
        ob.transform.Translate(0.0f,0.0f,0.1f);
        if(ob.transform.position.z > 20) {
            ob.transform.position = new Unity.Mathematics.float3(ob.transform.position.x,ob.transform.position.y,-51.4f);
        }
    }
    void Update() { 
        
        moveJob = new MoveSheepJob();
        handle = moveJob.Schedule(transforms);
    }
    void LateUpdate() {
        handle.Complete();
    }

    void OnDestroy() {
        transforms.Dispose();
    }
}
