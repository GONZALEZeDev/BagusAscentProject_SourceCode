using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PotionSoin : IItem
{
    // Propriétés spécifiques à PotionSoin
    public int ItemID { get; } = 1;
    public string ItemName { get; } = "Heal Potion";
    public string ItemDesc { get; } = "Gives health points back to targetted character.";

    // Implémentation de la méthode Use spécifique à PotionSoin
    public void Use(Character itemTarget)
    {
        itemTarget.currentHP = itemTarget.maxHP;
    }
}