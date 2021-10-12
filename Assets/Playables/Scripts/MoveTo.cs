
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;



[RequireComponent(typeof (NavMeshAgent))]
public class MoveTo : MonoBehaviour {

      public NavMeshAgent meshAgent;
      public AssetReference reference;
      
      Vector3Variable variable;
      async void Start() {


          reference.LoadAssetAsync<Vector3Variable>().Completed += (v)=>{
              if(v.IsDone) {
                  variable = v.Result;
              }
          };

          await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
          {
                Debug.Log("Update() " + Time.frameCount);
          }
      }

      void Update() {
            Debug.Log("In move to update");
            meshAgent.SetDestination(variable.Value);
      }
}