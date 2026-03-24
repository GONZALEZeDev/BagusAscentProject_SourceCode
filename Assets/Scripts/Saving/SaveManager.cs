using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


//
// Résumé :
//     SaveManager est une classe gérant la sauvegarde et l'accès aux données importantes du jeu.
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; set; }
    string saveFolderPath;
    string pathToSaveFile;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
    }

    private void Start()
    {
        saveFolderPath = Path.Combine(Application.persistentDataPath, "SaveFolder");
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        pathToSaveFile = Path.Combine(saveFolderPath, "saveData.json");

        if (!File.Exists(pathToSaveFile))
        {
            // Le fichier n'existe pas encore, donc on le créé
            File.WriteAllText(pathToSaveFile, "{}"); // Crée un fichier JSON vide
        }
    }

    public DataToSave GetSave()
    {
        DataToSave dat = new DataToSave();
        StreamReader reader = new StreamReader(pathToSaveFile);
        string jsonString = reader.ReadToEnd();
        JsonUtility.FromJsonOverwrite(jsonString, dat);

        reader.Close();

        return dat;
    }

    public void SaveData()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 7 || SceneManager.GetActiveScene().buildIndex == 9)
        WorldManager.instance.SaveState();
        DataToSave saveDataList = new DataToSave();

        foreach (HeroClass hero in TeamClass.instance.heroTeam)
        {
            HeroSaveData heroData = new HeroSaveData(
                hero.storedXp,
                hero.baseHeroXP,
                hero.currentXp, 
                hero.totalXp,
                hero.transform.position, 
                hero.characterName,
                hero.level,
                hero.maxHP,
                hero.currentHP,
                hero.maxMana,
                hero.currentMana,
                hero.learnedSkillsIDs
                );
            saveDataList.heroesData.Add(heroData);
        }

        saveDataList.sceneIndex = SceneManager.GetActiveScene().buildIndex;

        saveDataList.invKeysID = new int[PlayerInventory.instance.inventoryItems.Count];
        saveDataList.invValues = new int[PlayerInventory.instance.inventoryItems.Count];

        saveDataList.world1Chests = WorldsManager.instance.world1ChestsOpened;
        saveDataList.world2Chests = WorldsManager.instance.world2ChestsOpened;
        saveDataList.world3Chests = WorldsManager.instance.world3ChestsOpened;
        saveDataList.world4Chests = WorldsManager.instance.world4ChestsOpened;

        int index = 0;
        foreach (var pair in PlayerInventory.instance.inventoryItems)
        {
            saveDataList.invKeysID[index] = pair.Key.ItemID;
            saveDataList.invValues[index] = pair.Value;
            index++;
        }

        Debug.Log("Saving data at " + pathToSaveFile);
        string jsonString = JsonUtility.ToJson(saveDataList);

        StreamWriter writer = new StreamWriter(pathToSaveFile);

        //Debug.Log(jsonString);

        writer.Write(jsonString);//Overwrite old save
        writer.Close();
    }

    public bool isSaveFileEmpty()
    {
        StreamReader reader = new StreamReader(pathToSaveFile);
        string jsonString = reader.ReadToEnd();
        if (jsonString == "{}") return true;
        else return false;
    }

    public void EmptySaveFile()
    {
        Debug.Log("Emptying data at " + pathToSaveFile);
        string jsonString = "";
        StreamWriter writer = new StreamWriter(pathToSaveFile);
        writer.Write(jsonString);//Overwrite old save
        writer.Close();
    }

}
