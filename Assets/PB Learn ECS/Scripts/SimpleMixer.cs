using Unity.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;


public class SimpleMixer : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float weight;

    NativeArray<TransformStreamHandle> m_Handles;
    NativeArray<float> m_BoneWeights;

    PlayableGraph m_Graph;
    AnimationScriptPlayable m_CustomMixerPlayable;
    [SerializeField]
    AnimationClip clip;
    
    [SerializeField]
    Transform Armature;
    void OnEnable()
    {
        var animator = GetComponent<Animator>();
        var clips = animator.GetCurrentAnimatorClipInfo(0);
        // Load animation clips.
        var idleClip = clip;
        var romClip = clip;
        if (idleClip == null || romClip == null)
            return;


        // Get all the transforms in the hierarchy.
        var transforms = Armature.GetComponentsInChildren<Transform>();
        var numTransforms = transforms.Length - 1;

        // Fill native arrays (all the bones have a weight of 1.0).
        m_Handles = new NativeArray<TransformStreamHandle>(numTransforms, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        m_BoneWeights = new NativeArray<float>(numTransforms, Allocator.Persistent, NativeArrayOptions.ClearMemory);
        for (var i = 0; i < numTransforms; ++i)
        {
            m_Handles[i] = animator.BindStreamTransform(transforms[i + 1]);
            m_BoneWeights[i] = 1.0f;
        }

        // Create job.
        var job = new MixerJob()
        {
            handles = m_Handles,
            boneWeights = m_BoneWeights,
            weight = 0.0f
        };

        // Create graph with custom mixer.
        m_Graph = PlayableGraph.Create("SimpleMixer");
        m_Graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        m_CustomMixerPlayable = AnimationScriptPlayable.Create(m_Graph, job);
        m_CustomMixerPlayable.SetProcessInputs(false);
        m_CustomMixerPlayable.AddInput(AnimationClipPlayable.Create(m_Graph, idleClip), 0, 1.0f);
        m_CustomMixerPlayable.AddInput(AnimationClipPlayable.Create(m_Graph, romClip), 0, 1.0f);

        var output = AnimationPlayableOutput.Create(m_Graph, "output", animator);
        output.SetSourcePlayable(m_CustomMixerPlayable);

        m_Graph.Play();
        Debug.Log("Enable simple mixer");
    }

    void Update()
    {

        var job = m_CustomMixerPlayable.GetJobData<MixerJob>();

        job.weight = weight;

        m_CustomMixerPlayable.SetJobData(job);
        m_Graph.Play();
        Debug.Log("Update simple mixer");
    }

    void OnDisable()
    {
        Debug.Log("Disable simple mixer");
        m_Graph.Destroy();
        m_Handles.Dispose();
        m_BoneWeights.Dispose();
    }
}
