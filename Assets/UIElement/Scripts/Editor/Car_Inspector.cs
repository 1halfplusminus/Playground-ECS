
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Car))]
public class Car_Inspector: Editor {

    [SerializeField]
    VisualTreeAsset InspectorXML;
    public override VisualElement CreateInspectorGUI()
    {
       VisualElement myInspector = new VisualElement();
       myInspector.Add(new Label("This is a custom inspector"));

       InspectorXML.CloneTree(myInspector);
       return myInspector;
    }   
}