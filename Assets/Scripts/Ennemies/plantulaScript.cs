using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantulaScript : Ennemies
{
    // Start is called before the first frame update
    void Start()
    {
        LawnMower lawn = new LawnMower();
        Rolling roll = new Rolling();
        SweetPerfum sweetperfum = new SweetPerfum();
        currentSkills.Add(lawn);
        currentSkills.Add(roll);
        currentSkills.Add(sweetperfum);
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

        if (skillSelected.SkillName == "Lawn Mower")
        {
            LawnMower lawn = new LawnMower();
            lawn.ActivateAttackEffect(this, TeamClass.instance.heroTeam);
            announcementText = this.characterName + " uses their skill " + skillSelected.SkillName +  " on all heroes !";
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }
        else if (skillSelected.SkillName == "Rolling") 
        {
            skillSelected.ActivateAttackEffect(this, mainTarget);
            announcementText = this.characterName + " uses their skill " + skillSelected.SkillName + " on " + mainTarget.characterName + " !";
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }
        else if (skillSelected.SkillName == "Sweet Perfum")
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
