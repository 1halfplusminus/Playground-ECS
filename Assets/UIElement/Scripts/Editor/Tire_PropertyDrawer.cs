
using System;
using Unity.Properties.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Tire))]

public class Tire_PropertyDrawer : PropertyDrawer {


    public VisualTreeAsset InspectorXML;
    public override  VisualElement CreatePropertyGUI(SerializedProperty property) {
        var container = new VisualElement();
        InspectorXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UIElement/Scripts/Editor/Tire.uxml");
        InspectorXML.CloneTree(container);
      
        return container;
    }
}