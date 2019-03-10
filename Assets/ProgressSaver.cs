using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressSaver : MonoBehaviour
{

    public CharacterData characterData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void Save()
    {
        SaveCharacter(characterData, 0);

    }

    public void Load()
    {
        characterData = LoadCharacter(0);
    }
    
    public void AddFuel(int amount)
    {
        characterData.fuel_gathered += amount;
    }
    public void AddWood(int amount)
    {
        characterData.wood_gathered += amount;
    }
    public int TakeWood()
    {
        int amount = characterData.wood_gathered;
        characterData.wood_gathered = 0;
        return amount;
    }

    static void SaveCharacter (CharacterData data, int characterSlot)
    {
        PlayerPrefs.SetInt("wood_gathered_CharacterSlot" + characterSlot, data.wood_gathered);
        PlayerPrefs.SetInt("fuel_gathered_CharacterSlot" + characterSlot, data.fuel_gathered);
        PlayerPrefs.Save();
    }
    static CharacterData LoadCharacter (int characterSlot)
    {
        CharacterData loadedCharacter = new CharacterData();
        loadedCharacter.wood_gathered = PlayerPrefs.GetInt("wood_gathered_CharacterSlot" + characterSlot);
        loadedCharacter.fuel_gathered = PlayerPrefs.GetInt("fuel_gathered_CharacterSlot" + characterSlot);

        return loadedCharacter;
    }
}
