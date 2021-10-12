using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
[DisallowMultipleComponent]
public class TerrainAuth : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    Material mat;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {

        dstManager.AddComponentObject(entity,GetComponent<Terrain>());
    }
}