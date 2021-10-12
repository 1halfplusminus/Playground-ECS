using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterListController {
    private List<CharacterData> allCharacters;

    private VisualTreeAsset ListEntryTemplate;

    private ListView CharacterList;

    private Label CharacterClassLabel;

    private Label CharacterNameLabel;

    private VisualElement CharacterPortrait;

    private Button SelectCharacterButton;

    public void InitializeCharacterList(VisualElement root,VisualTreeAsset listElementTemplate) {
        EnumerateAllCharacters();

        ListEntryTemplate = listElementTemplate;

        CharacterList = root.Q<ListView>("CharList");

        CharacterClassLabel = root.Q<Label>("CharacterClass");

        CharacterNameLabel = root.Q<Label>("CharacterName");

        CharacterPortrait = root.Q<VisualElement>("CharacterPortrait");

        SelectCharacterButton = root.Q<Button>("SelectCharButton");       
    
        FillCharacterList();
    }
    private void FillCharacterList() {
   
        CharacterList.makeItem = () => {
            var newListEntry = ListEntryTemplate.Instantiate();

            var newListEntryLogic = new CharacterListEntryController();

            newListEntry.userData = newListEntryLogic;

            newListEntryLogic.SetVisualElement(newListEntry);

            return newListEntry;
        };
        CharacterList.bindItem = (item,index)=>{
            (item.userData as CharacterListEntryController).SetCharacterData(allCharacters[index]);
        };
        CharacterList.itemHeight = 55;
        CharacterList.itemsSource = allCharacters;
        CharacterList.onSelectionChange += OnCharacterSelected;
        CharacterList.onItemsChosen += OnCharacterSelected;
    }
    private void OnCharacterSelected(IEnumerable<object> selectedItems)
    {
        Debug.Log("Character selected");
        var selectedCharacter = CharacterList.selectedItem as CharacterData;
         if (selectedCharacter == null)
        {
           

            // Disable the select button
            SelectCharacterButton.SetEnabled(false);

            return;
        }
        CharacterNameLabel.text = selectedCharacter.CharacterName;
    }
    private void EnumerateAllCharacters() {
        allCharacters = new List<CharacterData>();
        allCharacters.AddRange(Resources.LoadAll<CharacterData>("Characters"));
    }
}