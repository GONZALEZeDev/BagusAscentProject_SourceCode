using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroSaveData
{
    //Toutes les valeurs à enregistrer
    public float storedXP; 
    public float currentXp; 
    public float totalXp;// Le total d'xp collecté jusqu'à maintenant
    public float baseHeroXP;

    public Vector3 beforeFightPos;

    public string characterName;
    public int level;
    public int maxHP;
    public int currentHP;
    public int maxMana;
    public int currentMana;
    public int[] learnedSkills;

    // Liste de compétences du personnage

    public HeroSaveData(float STOXP, float bHEROXP, float CURXP, float TOTXP ,Vector3 BEFOREPOS, string NAME, int LVL, int MAXHP, int CURHP, int MAXMANA, int CURMANA, int[] skills) 
    {
        storedXP = STOXP;
        baseHeroXP = bHEROXP;
        currentXp = CURXP;
        totalXp = TOTXP;
        beforeFightPos = BEFOREPOS;
        characterName = NAME;
        level = LVL;
        maxHP = MAXHP;
        currentHP = CURHP;
        maxMana = MAXMANA;
        currentMana = CURMANA;
        learnedSkills = skills;
    }

}
