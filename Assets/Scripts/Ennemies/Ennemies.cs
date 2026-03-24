using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemies : Character
{
    //public int xpDropRange; // Un nombre aléatoire d'XP donnée au joueur une fois que ses héros ont battu l'ennemie, le nombre est choisit dans une plage délimité 
    public bool isHisTurn = false;
    public bool hasBlocked = false;
    public bool hasAttacked = false;  // Variable pour suivre si l'ennemi a déjà attaqué

    public Character agroTarget;

    public void InitializeBasicStats()
    {
        maxHP += level * healthPerLevel;
        currentHP = maxHP;
    }

    public void EndTurn()
    {
        // Si l'ennemie a attaqué et qu'il a une cible d'agro, alors réinitialiser la cible d'agro à null
        if (hasAttacked == true && agroTarget != null)
        {
            agroTarget = null;
        }
        // Réinitialiser la variable indiquant si l'ennemi a attaqué
        hasAttacked = false;

        UIFight.instance.UpdateUI();
    }

    public void Blocking()
    {
        StartCoroutine(SfxManager.instance.PlayEnnemiesAttackSfx(0));
        string announcementText = characterName + " blocks the next attack !";
        StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        isblocking = true;
        hasBlocked = true;
        hasAttacked = true;
    }

    public Character GetMainTarget()
    {
        // Si l'ennemie a été ciblé auparavant par une attaque d'agro, l'agroTarget devient d'office la cible d'attaque
        if (agroTarget != null)
        {
            return agroTarget;
        }
        else
        {
            // Récupère une cible au hasard parmi les personnages dans charactersOrder
            List<HeroClass> potentialTargets = TeamClass.instance.heroTeam;
            List<HeroClass> validTargets = new List<HeroClass>();

            // Vérifie la vie de chaque héros
            foreach (HeroClass potentialTarget in potentialTargets)
            {
                if (potentialTarget.currentHP > 0)
                {
                    validTargets.Add(potentialTarget);
                }
            }

            // Sélectionne une cible au hasard parmi les héros valides
            if (validTargets.Count == 4)
            {
                HeroClass target = validTargets[Random.Range(0, validTargets.Count - 1)];
                Debug.Log("3 Perso");
                return target;
            }
            else
            {
                HeroClass target = validTargets[Random.Range(0, validTargets.Count)];
                return target;
            }
        }


    }
}
