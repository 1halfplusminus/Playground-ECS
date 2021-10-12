

using UnityEngine;

public class Car: MonoBehaviour {
    [SerializeField] string Make = "Toyota";
    [SerializeField] int YearBuild = 1980;

    [SerializeField] Color Color = Color.black;

    public Tire[] Tires = new Tire[4];
}