using UnityEngine;


[CreateAssetMenu(fileName = "Vector2Variable", menuName = "RPG/Vector2Variable", order = 0)]
public class Vector2Variable : Variable<Vector2>
{
    void Awake() { 
      Value = Vector2.zero;
    }
}