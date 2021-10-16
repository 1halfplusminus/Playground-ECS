
using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public struct BlinkingButton : IComponentData {

}
public class BlinkingButtonAuth : MonoBehaviour, IConvertGameObjectToEntity
{

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Debug.Log("here");
        
     /*    conversionSystem.AddHybridComponent(image);
        conversionSystem.AddHybridComponent(transform);
        conversionSystem.AddHybridComponent(renderer);
        conversionSystem.AddHybridComponent(button); */
/*         dstManager.AddComponentObject(entity, button); */
/*         dstManager.AddComponentObject(entity, GetComponent<Button>());
        dstManager.AddComponentData(entity, new BlinkingButton()); */
    }
}