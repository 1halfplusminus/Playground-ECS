
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;


public class MainView: MonoBehaviour ,IConvertGameObjectToEntity {
    [SerializeField] private VisualTreeAsset ListEntryTemplate;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        
        var uiDocument = GetComponent<UIDocument>();
        var buttons = uiDocument.rootVisualElement.Query<Button>().ToList();
        dstManager.AddComponentData(entity, new BlinkingButton());
    }

    void OnEnable() {
        var uiDocument = GetComponent<UIDocument>();

        var characterListController = new CharacterListController();

        characterListController.InitializeCharacterList(uiDocument.rootVisualElement, ListEntryTemplate);
    }
}