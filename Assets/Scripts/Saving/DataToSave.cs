using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataToSave
{
    //On encapsule les données de chaque héros dans cette liste
    public List<HeroSaveData> heroesData;

    //L'index de la scene à laquelle on a sauvegarde
    public int sceneIndex;

    //Etat des coffres
    public bool[] world1Chests;
    public bool[] world2Chests;
    public bool[] world3Chests;
    public bool[] world4Chests;

    //Les objets dans l'inventaire du joueur
    public int[] invKeysID;
    public int[] invValues;

    public DataToSave()
    {
        heroesData = new List<HeroSaveData>();
    }
}
