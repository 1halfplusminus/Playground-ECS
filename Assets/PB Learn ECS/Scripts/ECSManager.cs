using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public struct Sheep: IComponentData{
   public int Number;
   public Entity prefabs;
}
public class ECSManager : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] int numSheep = 8000;
    [SerializeField] GameObject sheepPrefab;
    Entity sheepEntity;
    EntityManager manager;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {

        sheepEntity = conversionSystem.GetPrimaryEntity(sheepPrefab);
        dstManager.AddComponentData(entity, new Sheep() { Number = numSheep, prefabs = sheepEntity });
/*         dstManager.AddComponentObject(entity, boxCollider); */
        conversionSystem.AddHybridComponent(boxCollider);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(sheepPrefab);
    }


   
}
