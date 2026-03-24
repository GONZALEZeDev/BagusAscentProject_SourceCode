using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class KaosHealer : Ennemies
{

    void Start()
    {
        currentSkills.Add(new BaseAttack());
        currentSkills.Add(new HealSkill());
    }

    // Update is called once per frame
    void Update()
    {
        // Condition de défense tout les entre X et X attaque 
        if (currentMana < maxMana/2 && !isblocking && !hasBlocked)
        {
            Blocking();
        }
        else if (isHisTurn && !hasAttacked)  // Vérifiez si c'est le tour de l'ennemi et s'il n'a pas encore attaqué
        {
            Attacking();
        }
    }

    void Attacking()
    {
        isblocking = false;
        // Récupère la cible principale et les cibles adjacentes
        Character mainTarget = GetMainTarget();

        List<ISkill> availableSkills = SkillsAvailable();
        ISkill skillSelected = availableSkills[Random.Range(0, availableSkills.Count)];

        string announcementText;

        if (skillSelected.SkillName == "Strike")
        {
            announcementText = this.characterName + " uses " + skillSelected.SkillName + " on " + mainTarget.characterName + " !";
            skillSelected.ActivateAttackEffect(this, mainTarget);
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }
        else if (skillSelected.SkillName == "Heal")
        {
            bool allyNeededHealFound = false;
            foreach (var ally in BattleManager.instance.theEnnemies)
            {
                if (ally.characterName != this.characterName)
                {
                    if (ally.currentHP < ally.maxHP/2)
                    {
                        skillSelected.ActivateAttackEffect(this, mainTarget);
                        announcementText = this.characterName + " uses " + skillSelected.SkillName + " on " + mainTarget.characterName + " !";
                        StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
                        allyNeededHealFound = true;
                        break;
                    }
                }
            }
            if (!allyNeededHealFound)
            {
                Attacking();
            }
        }

        // Marquer que l'ennemi a attaqué pendant ce tour
        hasAttacked = true;
        this.isHisTurn = false;
    }

    List<ISkill> SkillsAvailable()
    {
        // Liste les skills que le personnage peut lancer avec son mana actuel
        List<ISkill> availableSkills = new List<ISkill>();

        foreach (ISkill skill in currentSkills)
        {
            if (skill.SkillCost <= currentMana)
            {
                availableSkills.Add(skill);
            }
        }

            return availableSkills;
    }


}
