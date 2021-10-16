using UnityEngine;

public class TestAnimation : MonoBehaviour {
    Animator animator;
    SkinnedMeshRenderer skinnedMeshRenderer;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    GameObject armatureClone;

    float time;
    void Start()  {

        animator = GetComponentInChildren<Animator>();

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
     /*    skinnedMeshRenderer.enabled = false; */
        skinnedMeshRenderer.updateWhenOffscreen = true;
        skinnedMeshRenderer.enabled = false;
  
        meshFilter = skinnedMeshRenderer.gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = skinnedMeshRenderer.sharedMesh;
      /*   meshFilter.mesh = Instantiate(skinnedMeshRenderer.sharedMesh); */
        meshRenderer = skinnedMeshRenderer.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterials = skinnedMeshRenderer.sharedMaterials;
    /*     animator.enabled = false; */
  
        time = 0f;
    }

    void Update() {
        var newMesh = new Mesh();
        animator.Update(Time.deltaTime);  
        skinnedMeshRenderer.BakeMesh(newMesh);
        meshFilter.mesh = newMesh; 
    /*     skinnedMeshRenderer.rootBone = animator.GetComponentInChildren<Transform>(); 
        skinnedMeshRenderer.enabled = true; */
      /*   skinnedMeshRenderer.BakeMesh(newMesh);
        meshFilter.mesh = newMesh; 
 */
      /*   animator.Update(Time.deltaTime);  
        var state = animator.GetCurrentAnimatorStateInfo(0);
        var clips =  animator.GetCurrentAnimatorClipInfo(0);
        time += Time.deltaTime;
        for(int i= 0 ; i <  animator.GetCurrentAnimatorClipInfoCount(0) ; i++) {
             
             clips[i].clip.SampleAnimation(armatureClone,time);
             if(time >= clips[i].clip.length) {
                 time = 0.0f;
             }
        } */
       /*  var state = animator.GetCurrentAnimatorStateInfo(0);
        var clips =  animator.GetCurrentAnimatorClipInfo(0);
        Debug.Log(animator.playbackTime);
        for(int i= 0 ; i <  animator.GetCurrentAnimatorClipInfoCount(0) ; i++) {
            clips[i].clip.SampleAnimation(skinnedMeshRenderer.rootBone.gameObject,animator.playbackTime);
        }
        */
  
    }
}