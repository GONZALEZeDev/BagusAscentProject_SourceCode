using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lilThingScript : Ennemies
{
    public  bool isInvincible = false;
    // Start is called before the first frame update
    void Start()
    {
        SmashSkill smash = new SmashSkill();
        NoNoNoSkill nonono = new NoNoNoSkill();
        LilBuffSkill buff = new LilBuffSkill();
        FriendshipSkill friendship = new FriendshipSkill();

        currentSkills.Add(friendship);
        currentSkills.Add(buff);
        currentSkills.Add(smash);
        currentSkills.Add(nonono);
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

        if (isInvincible)
        {
            isInvincible = false;
            foreach (Ennemies enemy in BattleManager.instance.theEnnemies)
            {
                if (enemy != this)
                {
                    if (enemy.currentHP > 0)
                    {
                        isInvincible = true;
                        currentHP = maxHP/2;
                    }
                }
            }

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

        if (skillSelected.SkillName == "Smash")
        {
            List<Character> adjacentTargets = GetAdjacentTargets(mainTarget);
            SmashSkill smash = new SmashSkill();
            smash.smashAttackEffect(this, adjacentTargets);
            skillSelected.ActivateAttackEffect(this, mainTarget);
            announcementText = this.characterName + " uses " + skillSelected.SkillName + " on " + mainTarget.characterName + " !";
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }
        else if (skillSelected.SkillName == "No No No !!!") 
        {
            NoNoNoSkill nonono = new NoNoNoSkill();
            nonono.ActivateAttackEffect(this, TeamClass.instance.heroTeam);
            announcementText = this.characterName + " uses " + skillSelected.SkillName + " !";
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }
        else if (skillSelected.SkillName == "Lil Buff")
        {
            skillSelected.ActivateAttackEffect(this, mainTarget);
            announcementText = this.characterName + " uses " + skillSelected.SkillName + " on " + mainTarget.characterName + " !";
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }
        else if (skillSelected.SkillName == "Friendship")
        {
            FriendshipSkill friendship = new FriendshipSkill();
            friendship.ActivateAttackEffect(this);
            FriendshipEffect friendshipEffect = new FriendshipEffect();
            this.effectsList.Add(friendshipEffect);
            announcementText = this.characterName + " uses " + skillSelected.SkillName + " ! He is now invincible until you kill everey other ennemy !";
            StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        }

        StartCoroutine(SfxManager.instance.PlayEnnemiesAttackSfx(skillSelected));


        // Marquer que l'ennemi a attaqué pendant ce tour
        hasAttacked = true;
        this.isHisTurn = false;
    }
}
