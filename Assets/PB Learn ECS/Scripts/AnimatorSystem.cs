using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Deformations;
using Unity.Rendering;
using Unity.Collections;
using UnityEngine.Animations;
using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.Assertions;
using System.Linq;

namespace MyPackage.Systems
{    
    [DisableAutoCreation]

    public class AnimatorSystem : JobComponentSystem
    {
        Dictionary<Entity, NativeArray<TransformStreamHandle>> map_transformStream = new Dictionary<Entity, NativeArray<TransformStreamHandle>>();
        Dictionary<Entity, NativeArray<float>> entities_weight  = new Dictionary<Entity, NativeArray<float>>();
        protected override void OnCreate()
        {
            base.OnCreate();
           
        }
        protected override void OnDestroy() {
            base.OnDestroy();
            foreach (var item in map_transformStream)
            {
                item.Value.Dispose();
            }
             foreach (var item in entities_weight)
            {
                item.Value.Dispose();
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var time = Time.DeltaTime;
            Entities
            .WithNone<MyAnimator>()
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity,DynamicBuffer<Bone> bones,Render skinned)=>{
                var animator = EntityManager.GetComponentObject<Animator>(skinned.Animator);
                var skinnedRendered = EntityManager.GetComponentObject<SkinnedMeshRenderer>(skinned.SkinnedMeshRenderer);
                skinnedRendered.transform.SetParent(animator.transform);
                var clips = animator.GetCurrentAnimatorClipInfo(0);
             
                     // Load animation clips.
                var idleClip = clips[0].clip;
                var romClip =  clips[0].clip;

                if (idleClip == null || romClip == null)
                    return;


                // Get all the transforms in the hierarchy.

                var numTransforms = bones.Length - 1;

                map_transformStream[entity] = new NativeArray<TransformStreamHandle>(numTransforms, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                entities_weight[entity] = new NativeArray<float>(numTransforms, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
   
                var m_Handles= map_transformStream[entity];
                var m_BoneWeights = entities_weight[entity];
                var armatureTransform =    EntityManager.GetComponentObject<Transform>(bones[0].Value);

                armatureTransform.SetParent(animator.transform);

                // Add new skinned mesh
                animator.gameObject.hideFlags = HideFlags.None;
                foreach (var item in animator.GetComponentsInChildren<Transform>())
                {
                    item.gameObject.hideFlags = HideFlags.None;
                }
       
                for (var i = 0; i < numTransforms; ++i)
                {
                    
                    var transform = EntityManager.GetComponentObject<Transform>(bones[i+1].Value);
                    var parent = EntityManager.GetComponentObject<Transform>(bones[i+1].Parent);
                    parent.name = parent.name.Replace("(Clone)", "").Trim();
                    Debug.Log("parent : " + parent.name);
                    transform.name = transform.name.Replace("(Clone)", "").Trim();
                    Debug.Log("transform : " + transform.name);
                    transform.SetParent(parent);
                    m_Handles[i] = animator.BindStreamTransform(transform);
                    m_BoneWeights[i] = 1.0f;
                    if(i==0) {
                        skinnedRendered.rootBone.SetParent(transform);
                    }
                    Assert.IsNotNull(transform, "Transform at " + i + " is null");
/*                     skinnedRendered.bones[i] = transform.transform.Cl;
                    Assert.IsNotNull(skinnedRendered.bones[i], "hmmhmmmm at :" + i + " is null"); */
                }
                animator.Rebind();
                // Create job.
                var job = new MixerJob()
                {
                    handles = m_Handles,
                    boneWeights = m_BoneWeights,
                    weight = 0.0f
                };

                // Create graph with custom mixer.
                var m_Graph = PlayableGraph.Create("SimpleMixer");
                m_Graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

                var m_CustomMixerPlayable = AnimationScriptPlayable.Create(m_Graph, job);
                m_CustomMixerPlayable.SetProcessInputs(false);
                m_CustomMixerPlayable.AddInput(AnimationClipPlayable.Create(m_Graph, idleClip), 0, 1.0f);
                m_CustomMixerPlayable.AddInput(AnimationClipPlayable.Create(m_Graph, romClip), 0, 1.0f);

                var output = AnimationPlayableOutput.Create(m_Graph, "output", animator);
                output.SetSourcePlayable(m_CustomMixerPlayable);
                m_Graph.Play();

     
                EntityManager.AddComponentData(entity, new MyAnimator{Playable = m_CustomMixerPlayable,Graph = m_Graph});
            /*     GameObject.Instantiate(animator.gameObject); */
            
            }).Run();
            Entities
            .WithoutBurst()
            .ForEach((MyAnimator myAnimator,Render skinned,DynamicBuffer<Bone> bones) => {
                 var job = myAnimator.Playable.GetJobData<MixerJob>();
                job.weight = 0.0f;
                myAnimator.Playable.SetJobData(job);
                myAnimator.Graph.Play(); 

            }).Run(); 
             Entities
             .WithAll<MyAnimator>()
            .WithoutBurst()
            .ForEach((Entity e,Render r) => {
               var skinnedRendered = EntityManager.GetComponentObject<SkinnedMeshRenderer>(r.SkinnedMeshRenderer);
               var bones = EntityManager.GetBuffer<Bone>(e);
               var matrix = EntityManager.GetBuffer<SkinMatrix>(r.SkinnedMeshRenderer);
 
               for(var i = 0; i < bones.Length-1; ++i) {
                    Debug.Log("Update Matrix " + i);

                     var deform = EntityManager.GetComponentData<LocalToWorld>(bones[i+1].Value).Value;
                    var skinMat = math.mul(skinnedRendered.worldToLocalMatrix, deform);
                  /*   matrix[i] = new SkinMatrix
                        {
                            Value = new float3x4(skinMat.c0.xyz,
                                                skinMat.c1.xyz,
                                                skinMat.c2.xyz,
                                                skinMat.c3.xyz)
                        }; */
                  /*       matrix[i] = new SkinMatrix
                        {
                            Value = new float3x4(skinMat.c0.xyz,
                                                skinMat.c1.xyz,
                                                skinMat.c2.xyz,
                                                skinMat.c3.xyz)
                        }; */
                    /* var deform = EntityManager.GetComponentData<LocalToWorld>(bones[i].Value).Value;
                    var bindPose = math.mul(r.bones[i].worldToLocalMatrix , deform);
                    var skinMat = matrix[i].Value;
                   */
                    
               }
            }).Run();
             Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity e,Render skinned, RenderMesh r) => {
           
                var animator = EntityManager.GetComponentObject<Animator>(skinned.Animator);
                var skinnedRendered = EntityManager.GetComponentObject<SkinnedMeshRenderer>(skinned.SkinnedMeshRenderer);
                skinnedRendered.sharedMesh.MarkModified();
                skinnedRendered.sharedMesh.MarkDynamic();
                var newMesh = skinnedRendered.sharedMesh;
                animator.Update(time);
                skinnedRendered.BakeMesh(newMesh);
                newMesh.RecalculateBounds();
                var desc = new RenderMeshDescription(
                newMesh,
                skinnedRendered.sharedMaterial,
                shadowCastingMode: ShadowCastingMode.Off,
                receiveShadows: false);
                RenderMeshUtility.AddComponents(e,EntityManager,desc);
                EntityManager.AddComponentData(e,new NonUniformScale{Value = new float3(1f,1f,1f)});
            }).Run();
            return default;
        }
    }
}