using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

public struct Cube : IComponentData {

}
[DisableAutoCreation]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public class CubeGameSystem : JobComponentSystem
{

    protected override void OnCreate()
    {
        base.OnCreate();
        var myCube = EntityManager
        .CreateEntity(
            ComponentType.ReadOnly<Translation>(),
            ComponentType.ReadOnly<Cube>(),
            ComponentType.ReadOnly<LocalToWorld>(),
            ComponentType.ReadOnly<RenderMesh>()
        );
        Addressables.LoadAssetAsync<GameObject>("AssetHolder").Completed += (r)=>{
            var ah  = r.Result.GetComponent<AssetHolder>();
            var desc = new RenderMeshDescription(
                ah.myMesh,
                ah.myMaterial,
                shadowCastingMode: ShadowCastingMode.Off,
                receiveShadows: false);
            RenderMeshUtility.AddComponents(myCube,EntityManager,desc);
            EntityManager.AddComponentData(myCube,new LocalToWorld());
            EntityManager.AddComponentData(myCube,new Translation{
                Value = new float3(1,2,3)
            });
        };
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return default;
    }
}
