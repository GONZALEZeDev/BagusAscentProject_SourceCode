using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluSlimeScript : Ennemies
{

    // Variable de condition de défense

    // Start is called before the first frame update
    void Start()
    {
        // Ajoutez BaseAttack à la liste de compétences
        currentSkills.Add(new BaseAttack());
        currentSkills.Add(new SplashAttack());
        currentSkills.Add(new GluTrapAttack());
    }

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

        // Sélectionne un skill au hasard dans la liste et l'active
        ISkill skillSelected = currentSkills[Random.Range(0, currentSkills.Count)];

        while (skillSelected.SkillCost > currentMana)
        {
            skillSelected = currentSkills[Random.Range(0, currentSkills.Count)];
        }

        skillSelected.ActivateAttackEffect(this, mainTarget);
        string announcementText = this.characterName + " uses " + skillSelected.SkillName + " on " + mainTarget.characterName + " !";

        if (skillSelected.SkillName == "Strike")
        {
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }
        else if (skillSelected.SkillName == "Splash")
        {
            List<Character> adjacentTargets = GetAdjacentTargets(mainTarget);
            SplashAttack splashAttack = new SplashAttack();
            splashAttack.splashAttackEffect(this,adjacentTargets);
            // Affichage de l'annonce de lancer d'attaque et passage du tour
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }
        else if (skillSelected.SkillName == "Glu Trap")
        {
            GluEffect gluEffect = new GluEffect();
            mainTarget.effectsList.Add(gluEffect);
            // Affichage une description de l'effet
            announcementText = this.characterName + " uses " + skillSelected.SkillName + ", " + mainTarget.characterName + " skips their next turn !";
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }

        StartCoroutine(SfxManager.instance.PlayEnnemiesAttackSfx(skillSelected));


        // Marquer que l'ennemi a attaqué pendant ce tour
        hasAttacked = true;
        this.isHisTurn = false;
    }
}