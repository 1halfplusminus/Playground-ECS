using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public struct Armature: IComponentData {
   public Entity Animator;
}
public struct Bone: IBufferElementData {
   public Entity Value;
   public Entity Parent;
}
public struct BindPose: IBufferElementData {
   public float4x4 Value;
}

public struct Render: IComponentData {
   public Entity Animator;
   public Entity SkinnedMeshRenderer;
}
public struct MyAnimator: IComponentData {
    public AnimationScriptPlayable Playable;
    public PlayableGraph Graph;
}
public class ConvertSheep : MonoBehaviour, IConvertGameObjectToEntity
{

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {   

        var animator = GetComponentInChildren<Animator>();
        var renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        var newMesh = ScriptableObject.Instantiate(renderer.sharedMesh);
        renderer.BakeMesh(newMesh); 
        var desc = new RenderMeshDescription(
                newMesh,
                renderer.sharedMaterial,
                shadowCastingMode: ShadowCastingMode.Off,
                receiveShadows: false);
        RenderMeshUtility.AddComponents(entity,dstManager,desc); 
        dstManager.AddComponentData(entity, new LocalToWorld());
  /*       renderer.enabled = true; */
/*         renderer.updateWhenOffscreen = true; */
/*         animator.enabled = true; */
/*          conversionSystem.DeclareLinkedEntityGroup(gameObject); */
     /*    conversionSystem.DeclareLinkedEntityGroup(animator.gameObject); */
        conversionSystem.AddHybridComponent(animator);
        conversionSystem.AddHybridComponent(renderer);
  

        foreach(var item in renderer.bones) {
            conversionSystem.AddHybridComponent(item);
            var boneEntity = conversionSystem.GetPrimaryEntity(item);
            dstManager.AddComponentData(boneEntity, new CopyTransformFromGameObject());
        }
        var bonesBuffer = dstManager.AddBuffer<Bone>(entity);
        var armature = renderer.bones[0].parent;

        conversionSystem.AddHybridComponent(armature.transform);
        bonesBuffer.Add(new Bone{Value =  conversionSystem.GetPrimaryEntity(armature)});
        foreach (var item in renderer.bones)
        {
            var boneEntity = conversionSystem.GetPrimaryEntity(item);
            bonesBuffer.Add(new Bone{Value = boneEntity,Parent = conversionSystem.GetPrimaryEntity(item.parent)});
        } 
        
       /*  foreach (var item in renderer.bones)
        {
            Debug.Log("bone name:" + item.gameObject.name);
            conversionSystem.AddHybridComponent(item);

        } */

        dstManager.AddComponentData(entity, new Render{Animator = conversionSystem.GetPrimaryEntity(animator),SkinnedMeshRenderer = conversionSystem.GetPrimaryEntity(renderer)});
  /*       var sharedMesh = renderer.sharedMesh;
        if (sharedMesh.boneWeights.Length > 0 && sharedMesh.bindposes.Length > 0)
        {
            dstManager.AddBuffer<BindPose>(rendererEntity);
            var bindPoseArray = dstManager.GetBuffer<BindPose>(rendererEntity);
            bindPoseArray.ResizeUninitialized( renderer.bones.Length);
            for (int boneIndex = 0; boneIndex != renderer.bones.Length; ++boneIndex)
            {
                var bindPose = renderer.sharedMesh.bindposes[boneIndex];
                bindPoseArray[boneIndex] = new BindPose { Value = bindPose };
            }
        }
        var bones = dstManager.AddBuffer<Bone>(rendererEntity);
        foreach (var item in renderer.bones)
        {

            var boneEntity = conversionSystem.GetPrimaryEntity(item);
            bones.Add(new Bone() { Value = boneEntity});
        
        } 
        conversionSystem.AddHybridComponent(Armature); 
        var animatorEntity = conversionSystem.GetPrimaryEntity(animator);
        var armatureEntity = conversionSystem.GetPrimaryEntity(Armature);
        dstManager.AddComponentData(armatureEntity, new Armature( ) {Animator = animatorEntity});
 */
    }
}