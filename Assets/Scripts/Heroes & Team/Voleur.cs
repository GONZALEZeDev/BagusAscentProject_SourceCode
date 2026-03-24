using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Voleur : HeroClass
{

    // Elements pour le ciblage
    public int targetedAllyIndex = 0;
    public List<Character> targetedAllies = new List<Character>();

    public void InitializeBasicData()
    {
        shadow = gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
        sp = GetComponent<SpriteRenderer>();
        sp.material = defaultMaterial;
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animController;
        characterName = "Rovel";
        characterClass = "Voleur";
        name = "Rovel";
        currentLvlCap = 0.5f * Mathf.Pow(level * 2, 3) + 6;
        healthPerLevel = 6;
        damagePerLevel = 4;
        maxHP = 85;
        currentHP = 85;
        baseHeroXP = 105;
        currentSkills.Add(new BaseAttack());
        currentSkills.Add(new ManaTransferSkill());
        currentSkills.Add(new StealingLifeSkill());
    }

    public void InitializeSkills(int[] lSkills)
    {

    }

    public void TargettingManatransfert()
    {
        string announcement = "Choose 2 allies with the arrows or Q and D ! (The one who sends mana and the one who receives it)";
        UIFight.instance.annoucementGO.SetActive(true);
        UIFight.instance.annoucementLayer1GO.SetActive(true);
        UIFight.instance.annoucementLayer2GO.SetActive(true);

        UIFight.instance.annoucementGO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;
        UIFight.instance.annoucementLayer1GO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;
        UIFight.instance.annoucementLayer2GO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;

        // Vérifie si la précédente cible visée est toujours vivante, sinon on change de cible automatique
        if (TeamClass.instance.heroTeam[targetedAllyIndex].currentHP <= 0)
        {
            if (targetedAllyIndex + 1 >= TeamClass.instance.heroTeam.Count)
            {
                targetedAllyIndex = 0;
            }
            else
            {
                targetedAllyIndex++;
            }
        }
        TeamClass.instance.heroTeam[targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>().material = UIFight.instance.outlineMaterial;

        // Vérifie si la flèche gauche du pavé numérique a été pressée
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeTargetedAlly(-1);
        }

        // Vérifie si la flèche droite du pavé numérique a été pressée
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeTargetedAlly(1);
        }

        // Vérifie si la touche Entrée a été pressée
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Vérifie si le nombre d'alliés déjà ciblés est inférieur à 2
            if (CountTargetedAllies() < 2)
            {
                // Vérifie si l'allié sélectionné n'est pas déjà ciblé et n'est pas le lanceur du sort
                if (!IsAllyAlreadyTargeted(TeamClass.instance.heroTeam[targetedAllyIndex]) &&
                    TeamClass.instance.heroTeam[targetedAllyIndex] != ButtonUIFight.instance.activeCharacter)
                {
                    // Ajoute l'allié à la liste des cibles
                    targetedAllies.Add(TeamClass.instance.heroTeam[targetedAllyIndex]);
                    // Met à jour le visuel de la cible sur l'allié sélectionné
                    UpdateTargetVisual(TeamClass.instance.heroTeam[targetedAllyIndex], true);
                }
            }

            // Si deux alliés ont été ciblés, désactive le mode de ciblage
            if (CountTargetedAllies() == 2)
            {
                UIFight.instance.annoucementGO.SetActive(false);
                UIFight.instance.annoucementLayer1GO.SetActive(false);
                UIFight.instance.annoucementLayer2GO.SetActive(false);
                ButtonUIFight.instance.isLockMode = false;

                // Appeler la fonction d'exécution de l'attaque
                ManaTransferSkill manaTransfer = new ManaTransferSkill();
                manaTransfer.ActiveManaTransfer(this);
                manaTransfer.Transfer(targetedAllies[0], targetedAllies[1], 10);

                // Annonce de l'attaque et désactivation des boutons + remise à zéro du menu principal
                string textAnnouncement = characterName + " uses " + ButtonUIFight.instance.targetingSkill.SkillName + " !";
                ButtonUIFight.instance.HideSubAttackMenu();
                UIFight.instance.HideDescAttack();
                UIFight.instance.ResizeChildrenScaleMainMenu();
                UIFight.instance.ResizeChildrenScaleNormalEnnemiePanel();
                foreach (var ally in targetedAllies)
                {
                    SpriteRenderer spriteRenderer = ally.gameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.material = ally.defaultMaterial;
                }
                StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(textAnnouncement));
                // Réinitialise la liste des alliés ciblés
                targetedAllies.Clear();
            }
        }
    }

    void ChangeTargetedAlly(int direction)
    {
        SpriteRenderer spriteRenderer = TeamClass.instance.heroTeam[targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = TeamClass.instance.heroTeam[targetedAllyIndex].defaultMaterial;

        // Calcule l'index du prochain allié à cibler
        targetedAllyIndex += direction;

        if (targetedAllyIndex < 0)
        {
            targetedAllyIndex = TeamClass.instance.heroTeam.Count - 1;
        }
        else if (targetedAllyIndex >= TeamClass.instance.heroTeam.Count)
        {
            targetedAllyIndex = 0;
        }

        // Si l'allié ciblé est le lanceur du sort, passe au suivant
        if (TeamClass.instance.heroTeam[targetedAllyIndex] == ButtonUIFight.instance.activeCharacter)
        {
            ChangeTargetedAlly(direction);
        }

        spriteRenderer = TeamClass.instance.heroTeam[targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = UIFight.instance.outlineMaterial;
        UIFight.instance.ennemie = TeamClass.instance.heroTeam[targetedAllyIndex];
    }

    bool IsAllyAlreadyTargeted(Character ally)
    {
        // Vérifie si l'allié est déjà dans la liste des alliés ciblés
        return targetedAllies.Contains(ally);
    }

    int CountTargetedAllies()
    {
        // Retourne le nombre d'alliés actuellement ciblés
        return targetedAllies.Count;
    }

    void UpdateTargetVisual(Character ally, bool isTargeted)
    {
        // Met à jour le visuel pour indiquer si l'allié est ciblé ou non
        SpriteRenderer spriteRenderer = ally.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = isTargeted ? UIFight.instance.outlineMaterial : ally.defaultMaterial;
    }
}
