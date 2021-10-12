using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

using UnityEngine.AI;
using System;
using Unity.Physics;
using UnityEngine;
using Unity.Physics.Systems;

[Serializable]
public struct MyMove : IComponentData {
    public float3 WorldPosition;
}
[Serializable]
public struct Player : IComponentData {

}
[Serializable]
public struct MouseClickMouvement: IComponentData {
    public float2 ScreenPosition;
    public UnityEngine.Ray Ray;
}
public class MoveSystem : SystemBase
{ 
    struct PhysicsInput {
        [ReadOnly] public PhysicsWorld PhysicsWorld;
        [ReadOnly] public CollisionWorld CollisionWorld;
    }
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;
    private BuildPhysicsWorld physicsWorldSystem;

    protected override void OnCreate()
    {
        commandBufferSystem = World
            .DefaultGameObjectInjectionWorld
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    
        physicsWorldSystem=  World.DefaultGameObjectInjectionWorld.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>();
    }
    protected override void OnUpdate()
    {
        EntityCommandBuffer.ParallelWriter commandBuffer
        = commandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        var physicsInput = new PhysicsInput() {
            CollisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld,
            PhysicsWorld = physicsWorldSystem.PhysicsWorld
        };
        var handle = JobHandle.CombineDependencies(Dependency,physicsWorldSystem.GetOutputDependency());
        var rayCastCommands = new NativeList <RaycastCommand>(1,Allocator.TempJob);
        var mouseClickQuery = GetEntityQuery(typeof(MouseClickMouvement));
        handle = Entities.ForEach((Entity entity,int entityInQueryIndex,MouseClickMouvement mouseClick)=>{
            rayCastCommands.Add(
                new RaycastCommand(mouseClick.Ray.origin,mouseClick.Ray.origin + mouseClick.Ray.direction * 100.0f)
            );
           /*  var input = new RaycastInput
            {
                Start = mouseClick.Ray.origin,
                End = mouseClick.Ray.origin + mouseClick.Ray.direction * 100.0f,
                Filter = CollisionFilter.Default,
            };
            Unity.Physics.RaycastHit hit = new Unity.Physics.RaycastHit();
            bool haveHit = physicsInput.CollisionWorld.CastRay(input, out hit);
            if (haveHit)
            {
                // see hit.Position
                // see hit.SurfaceNormal
                Entity e = physicsInput.PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;
                Debug.Log("Player have a mouse click mouvement add the move component");
                commandBuffer.AddComponent(entityInQueryIndex,entity, new MyMove() {
                    WorldPosition = hit.Position
                });
            }    */ 
           
        }).Schedule(handle);
        handle.Complete();
        var raycastResults = new NativeArray<UnityEngine.RaycastHit>(rayCastCommands.Length,Allocator.TempJob);
        JobHandle rayCastHandle = RaycastCommand.ScheduleBatch(rayCastCommands, raycastResults, raycastResults.Length, default(JobHandle));
        rayCastHandle.Complete();
        var index = 0;
        foreach (var item in mouseClickQuery.ToEntityArray(Allocator.Temp))
        {
            if(raycastResults.Length > index) {
                var hit = raycastResults[index];
                if(hit.collider) {
                    Debug.Log("Player have a mouse click mouvement add the move component");
                    EntityManager.AddComponentData(item, new MyMove() {
                        WorldPosition = hit.point
                    });
                }
                index++;
            }
           
        }
        raycastResults.Dispose();
        rayCastCommands.Dispose();

        commandBufferSystem.AddJobHandleForProducer(handle);
        Entities
        .WithoutBurst()
        .ForEach((Entity entity, NavMeshAgent navMesh,MyMove move,ref LocalToWorld position) => {
            Debug.Log("Find entity with navmesh and move");
            navMesh.SetDestination(move.WorldPosition);
            position.Value = navMesh.transform.localToWorldMatrix;
        }).Run();

    }
}
