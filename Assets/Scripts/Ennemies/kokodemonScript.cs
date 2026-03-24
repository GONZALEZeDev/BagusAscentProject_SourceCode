using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kokodemonScript : Ennemies
{
    // Start is called before the first frame update
    void Start()
    {
        Groaaaaarrr groar = new Groaaaaarrr();
        LavaBall lavaBall = new LavaBall();
        BitingSkill bitingAttack = new BitingSkill();
        currentSkills.Add(groar);
        currentSkills.Add(lavaBall);
        currentSkills.Add(bitingAttack);
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

        ISkill skillSelected = currentSkills[Random.Range(0, currentSkills.Count)];

        //On vérifie s'il a assez de mana, si non on reprend un autre skill au hasard
        while (skillSelected.SkillCost > currentMana)
        {
            skillSelected = currentSkills[Random.Range(0, currentSkills.Count)];
        }

        // Affichage de l'annonce de lancer d'attaque et passage du tour
        string announcementText;

        if (skillSelected.SkillName == "Groar !")
        {
            skillSelected.ActivateAttackEffect(this, mainTarget);
            announcementText = this.characterName + " uses their skill " + skillSelected.SkillName +  " to buff themself !";
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }
        else if (skillSelected.SkillName == "Lava ball") 
        {
            List<Character> adjacentTargets = GetAdjacentTargets(mainTarget);
            LavaBall lavaball = new LavaBall();
            lavaball.lavaBallAttackEffect(adjacentTargets);
            skillSelected.ActivateAttackEffect(this, mainTarget);
            announcementText = this.characterName + " uses their skill " + skillSelected.SkillName + " on " + mainTarget.characterName + " !";
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }
        else if (skillSelected.SkillName == "Biting")
        {
            skillSelected.ActivateAttackEffect(this, mainTarget);
            announcementText = this.characterName + " uses their skill " + skillSelected.SkillName + " on " + mainTarget.characterName + " !";
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }

        StartCoroutine(SfxManager.instance.PlayEnnemiesAttackSfx(skillSelected));


        // Marquer que l'ennemi a attaqué pendant ce tour
        hasAttacked = true;
        this.isHisTurn = false;
    }
}
