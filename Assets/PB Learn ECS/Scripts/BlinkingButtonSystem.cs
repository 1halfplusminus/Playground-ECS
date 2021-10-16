

using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UIElements;


[UpdateInGroup(typeof(PresentationSystemGroup))]
public class BlinkingButtonSystem : JobComponentSystem
{
    EntityQuery blinkingButtonQuery;
    float collectTime;
    bool on;

    protected override void OnCreate()
    {
        base.OnCreate();
     /*    blinkingButtonQuery = GetEntityQuery(
            ComponentType.ReadWrite<UIDocument>(),
            ComponentType.ReadOnly<BlinkingButton>()
        ); */

    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        collectTime += Time.DeltaTime;
        if (collectTime > 0.2f)
        {
            collectTime -= 0.2f;
            on = !on;
        }
      /*   foreach (var entity in blinkingButtonQuery.ToEntityArray(Unity.Collections.Allocator.Temp))
        {
            var button = EntityManager.GetComponentObject<Button>(entity);
            button.visible = on;
        } */
      /*   foreach (var entity in blinkingButtonQuery.ToEntityArray(Unity.Collections.Allocator.Temp))
        {
            var button = EntityManager.GetComponentObject<Button>(entity);
            button.visible = on;
        } */
        Entities
        .WithoutBurst()
        .WithAll<BlinkingButton>()
        .ForEach((Entity e, UIDocument b) => { 
            foreach (var button in b.rootVisualElement.Query<Button>().ToList())
            {
                Debug.Log("Button Find");
                button.visible = on;
            }
         })
        .Run(); 
        return default;
    }
}