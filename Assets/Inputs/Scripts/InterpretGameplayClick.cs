using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InterpretGameplayClick", menuName = "RPG/InterpretGameplayClick", order = 0)]
public class InterpretGameplayClick : ScriptableObject {
    [SerializeField]
     Vector2Variable screenPosition;
   [SerializeField]
     Vector3Variable worldPosition;
    Entity player;

    public void SetPlayer(Entity player) {
        this.player = player;
    }
    public void Interpret(InputAction.CallbackContext context) {
        if(player == default(Entity)) {return;}
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        var position = Mouse.current.position.ReadValue();
        Debug.Log("Mouse position: " + position);
        screenPosition.Value = position;
        Ray ray = Camera.main.ScreenPointToRay(position);
        em.AddComponentData(player,new MouseClickMouvement() {
            ScreenPosition = screenPosition.Value,
            Ray = ray
        });
        Debug.Log("Interpet gameplay click screen position " + screenPosition.Value);

    }
}