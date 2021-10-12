using UnityEngine.UIElements;

public class CharacterListEntryController {
    private Label nameLabel;

    public void SetVisualElement(VisualElement visualElement) {
        nameLabel = visualElement.Q<Label>("CharacterName");
    }

    public void SetCharacterData(CharacterData data) {
        nameLabel.text = data.CharacterName;
    }
}