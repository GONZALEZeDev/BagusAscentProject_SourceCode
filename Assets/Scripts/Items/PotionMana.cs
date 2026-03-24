using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PotionMana : IItem
{
    // Propriétés spécifiques à PotionSoin
    public int ItemID { get; } = 2;
    public string ItemName { get; } = "Mana Potion";
    public string ItemDesc { get; } = "Gives mana back to targetted character.";

    // Implémentation de la méthode Use spécifique à PotionSoin
    public void Use(Character itemTarget)
    {
        itemTarget.currentMana = itemTarget.maxMana;
    }
}
