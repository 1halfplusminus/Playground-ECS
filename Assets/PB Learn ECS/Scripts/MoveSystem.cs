using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;


struct MovingSheep : IComponentData {

}

namespace MyPackage.Systems
{    

    public class MoveSystem : JobComponentSystem
    {


        protected override void OnCreate()
        {
            base.OnCreate();
           
        }
        public float3 GetRandomPointInsideCollider( BoxCollider boxCollider )
        {
            float3 extents = boxCollider.size / 2f;
            Vector3 point = new Vector3(
                UnityEngine.Random.Range( -extents.x, extents.x ),
                UnityEngine.Random.Range( -extents.y, extents.y ),
                UnityEngine.Random.Range( -extents.z, extents.z )
            )  + boxCollider.center;
            return boxCollider.transform.TransformPoint( point );
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var manager = EntityManager;
            Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity e,BoxCollider boxCollider,Sheep sheep) => {
                for(int i = 0; i < sheep.Number; i++) {
                    var instance = manager.Instantiate(sheep.prefabs);
                    var position = GetRandomPointInsideCollider(boxCollider);
                    manager.AddComponentData(instance, new MovingSheep());
                    manager.AddComponentData(instance, new Translation{ Value = position});
                    manager.AddComponentData(instance,new Rotation{Value = new quaternion(0,0,0,0)});
                }
                manager.RemoveComponent<Sheep>(e);
            }).Run();
            return Entities.WithAll<MovingSheep>()
            .ForEach((Entity e,Rotation rotation,ref Translation position) =>{
                position.Value += 0.1f * math.forward(rotation.Value);
                if(position.Value.z > 20) {
                   position.Value = new Unity.Mathematics.float3(position.Value.x,position.Value.y,-51.4f);
                }
            }).Schedule(inputDeps);
        }
    }
}