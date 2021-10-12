

using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;



[UpdateInGroup(typeof(PresentationSystemGroup))]
public class BlinkingButtonSystem : JobComponentSystem
{
    EntityQuery blinkingButtonQuery;
    float collectTime;
    bool on;

    protected override void OnCreate()
    {
        base.OnCreate();
        blinkingButtonQuery = GetEntityQuery(
            ComponentType.ReadOnly<Button>(),
            ComponentType.ReadOnly<BlinkingButton>()
        );

    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        collectTime += Time.DeltaTime;
        if (collectTime > 0.2f)
        {
            collectTime -= 0.2f;
            on = !on;
        }
        Entities
        .WithAll<BlinkingButton>()
        .ForEach((Button b) => { b.interactable = on; })
        .WithoutBurst()
        .Run();
        return default;
    }
}