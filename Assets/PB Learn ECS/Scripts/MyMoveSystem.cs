using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MyMoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = Entities.WithName("MoveSystem")
        .ForEach((ref Translation translation) =>{
            translation.Value += 0.01f * math.up();
        }).Schedule(inputDeps);
        return jobHandle;
    }
}
