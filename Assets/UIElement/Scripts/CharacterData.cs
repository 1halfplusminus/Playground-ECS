using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ECharacterClass{
    Knight,Ranger,Wizard
}
[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public ECharacterClass Class;
    public Sprite PortraitImage;
}
