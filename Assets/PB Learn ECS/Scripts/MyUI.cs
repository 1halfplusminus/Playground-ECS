using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MyUI : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField]
    GameObject prefab;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
 
      foreach (var button in gameObject.GetComponentsInChildren<BlinkingButtonAuth>())
      {
         var e = conversionSystem.CreateAdditionalEntity(gameObject);
         button.Convert(e,dstManager,conversionSystem);
      }
      /* dstManager.AddComponentObject(entity,rectTransform);
      dstManager.AddComponentObject(entity,canvas);
      dstManager.AddComponentObject(entity,gameObject);

      conversionSystem.AddHybridComponent(button);
      conversionSystem.AddHybridComponent(image);
      conversionSystem.AddHybridComponent(react); */
       /*  conversionSystem.DeclareLinkedEntityGroup(canvas.gameObject); */
      /*   conversionSystem.CreateAdditionalEntity(canvas.gameObject); */

    }

}
