
using UnityEngine;

public class Variable<T> : ScriptableObject, IVariable<T> {
    [SerializeField]
    private T value;
    public T Value
    {
        get { return value; }
        set { this.value = value; }
    }
}