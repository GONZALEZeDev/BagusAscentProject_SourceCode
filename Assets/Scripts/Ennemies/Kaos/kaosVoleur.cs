using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KaosVoleur : Ennemies
{

    // Elements pour le ciblage
    public int targetedAllyIndex = 0;
    public bool isLockMode = false;
    public Character target;
    public List<Character> targetedAllies = new List<Character>();

    void Start()
    {
        currentSkills.Add(new BaseAttack());
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
        if (isLockMode)
        {
            //TargettingManatransfert(target);
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
            Debug.Log("Kaos Attaque !");
            announcementText = this.characterName + " uses " + skillSelected.SkillName + " on " + mainTarget.characterName + " !";
            skillSelected.ActivateAttackEffect(this, mainTarget);
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
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
