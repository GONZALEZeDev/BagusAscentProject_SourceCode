using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HeroClass : Character
{
    public float totalXp; // Le total d'xp collecté jusqu'à maintenant
    public float currentLvlCap; // Total d'xp à collecter pour passer au niveau supérieur
    public float currentXp; // Le total d'xp collecté sur ce niveau. Se remet à 0 au prochain niveau
    public float storedXp = 0; // La quantité d'xp s'accumulant lors d'un combat. Se remet à 0 en sortant de combat.
    public float baseHeroXP = 100; // Valeur permettant de doser la quantité d'xp que reçoit un héros en particulier par rapport aux autres. Sert à l'équilibrage.

    //In Top Down view
    public SpriteRenderer sp;
    public RuntimeAnimatorController animController;
    public Animator animator;
    public float walkSpeed = 5f;
    public Sprite faceSprite;
    public Sprite deadSprite;
    //Ce tableau correspond aux skills que le héros a choisi en augmentant de niveau, mais sous forme sérialisable.
    //0 = Pas encore choisi ; 1 = 1ere attaque choisie ; 2 = 2e attaque choisie
    public int[] learnedSkillsIDs;

    public Vector3 beforeFightPos;

    public SpriteRenderer shadow;

    public void ContinueHeroClass(HeroSaveData data)
    {
        storedXp = data.storedXP;
        baseHeroXP = data.baseHeroXP;
        currentXp = data.currentXp;
        totalXp = data.totalXp;
        beforeFightPos = data.beforeFightPos;
        transform.position = beforeFightPos;
        level = data.level;
        maxHP = data.maxHP;
        currentHP = data.currentHP;
        maxMana = data.maxMana;
        currentMana = data.currentMana;
        currentLvlCap = 0.5f * Mathf.Pow(level * 2, 3) + 6;
    }

    public void ActivateSelectedAttack()
    {
        if (ButtonUIFight.instance.lastHoveredAttackButton != null)
        {
            string selectedAttackName = ButtonUIFight.instance.lastHoveredAttackButton.GetComponentsInChildren<Image>()[1].gameObject.name;

            // Comparez selectedAttackName avec les compétences du personnage actif
            foreach (ISkill skill in currentSkills)
            {
                if (skill.SkillName == selectedAttackName)
                {
                    if (currentMana < skill.SkillCost)
                    {
                        string announcementText = characterName + " doesn't have enough mana to use " + skill.SkillName + " !";
                        StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
                        break;
                    }
                    else
                    {
                        ButtonUIFight.instance.targetingSkill = skill;
                        // Effectuez les actions nécessaires ici
                        Debug.Log("Selected Attack: " + selectedAttackName);
                        ButtonUIFight.instance.isLockMode = true;
                        break;
                    }
                }
            }
        }
    }
}
