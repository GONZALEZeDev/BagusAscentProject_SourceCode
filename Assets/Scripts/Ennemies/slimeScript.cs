using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : Ennemies
{

    // Start is called before the first frame update
    void Start()
    {
        // Ajoutez BaseAttack à la liste de compétences
        currentSkills.Add(new BaseAttack());
        currentSkills.Add(new SplashAttack());
    }

    void Update()
    {
        if (currentMana < maxMana/2 && !isblocking && !hasBlocked)
        {
            Blocking();
        }
        if (isHisTurn && !hasAttacked)  // Vérifiez si c'est le tour de l'ennemi et s'il n'a pas encore attaqué
        {
            Attacking();
        }
    }

    void Attacking()
    {
        hasBlocked = false;
        isblocking = false;
        // Récupère la cible principale et les cibles adjacentes
        Character mainTarget = GetMainTarget();

        // Sélectionne un skill au hasard dans la liste et l'active
        ISkill skillSelected = currentSkills[Random.Range(0, currentSkills.Count)];

        //On vérifie s'il a assez de mana, si non on reprend un autre skill au hasard
        while(skillSelected.SkillCost > currentMana)
        {
            skillSelected = currentSkills[Random.Range(0, currentSkills.Count)];
        }

        StartCoroutine(SfxManager.instance.PlayEnnemiesAttackSfx(skillSelected));

        skillSelected.ActivateAttackEffect(this, mainTarget);

        if (skillSelected.SkillName == "Splash")
        {
            List<Character> adjacentTargets = GetAdjacentTargets(mainTarget);
            SplashAttack splashAttack = new SplashAttack();
            splashAttack.splashAttackEffect(this, adjacentTargets);
        }

        // Marquer que l'ennemi a attaqué pendant ce tour
        hasAttacked = true;

        // Affichage de l'annonce de lancer d'attaque et passage du tour
        string announcementText = this.characterName + " uses " + skillSelected.SkillName + " on " + mainTarget.characterName + " !";
        StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        this.isHisTurn = false;
    }

}