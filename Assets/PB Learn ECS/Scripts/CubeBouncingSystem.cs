
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine.InputSystem;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
[DisableAutoCreation]
public class CubeBouncingSystem : JobComponentSystem
{
    GameInput input;
    protected override void OnCreate()
    {
        base.OnCreate();
        input = new GameInput();
        input.Enable();
  
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        input.Disable();
    }
    protected override JobHandle OnUpdate(JobHandle deps)
    {
        int multiplier = input.Gameplay.Jump.triggered ? 80 : 5;
        var ellapseTime = Time.ElapsedTime;

        JobHandle jh = Entities.WithAll<Cube>().ForEach((ref Translation t) =>
        {
            float3 value = t.Value;
            value.y = math.cos((float)ellapseTime * multiplier);
            t.Value = value;
        }).Schedule(deps);

        return jh;
    }
}