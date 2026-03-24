using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
{
    // Informations sur le personnage
    public string characterName;
    public string characterClass;
    public int level = 1;//Commence au niveau 1
    public Sprite sideSprite;
    public int maxHP;
    public int currentHP;
    public int maxMana = 100;
    public int currentMana = 100;
    public bool isblocking = false;
    public bool isDefenseBuffed = false;
    public int damagePerLevel; // Valeur ajoutée aux dégats à chaque attaque directe du héros selon le niveau du héros
    public int healthPerLevel; // Valeur ajoutée à la vie du héros à chaque fois qu'il passe un niveau

    // Liste d'effets subit
    public List<IEffect> effectsList = new List<IEffect>();

    // Matérial par défault du personnage
    public Material defaultMaterial;

    // Liste de compétences du personnage
    public List<ISkill> currentSkills = new List<ISkill>();

    public void CheckEffectList()
    {
        // Utilisez une liste pour stocker les indices des effets à supprimer
        List<int> effectsToRemove = new List<int>();

        // Parcourez la liste des effets
        for (int i = 0; i < effectsList.Count; i++)
        {
            var effect = effectsList[i];

            if (effect.EffectUsed)
            {
                // Ajoutez l'index de l'effet à la liste des indices à supprimer
                effectsToRemove.Add(i);
                if (effect.EffectName == "Attack Buff")
                {
                    foreach (var skill in currentSkills)
                    {
                        skill.SkillDamageAfterEffect = skill.SkillPower;
                    }
                }
            }
            else
            {
                // Activez l'effet si non utilisé
                effect.ActivateEffect(this);
                Debug.Log(effect.EffectName + "is activated.");
            }
        }

        // Parcourez la liste des indices à supprimer en ordre inverse
        // pour éviter de modifier l'ordre des indices pendant la suppression
        for (int i = effectsToRemove.Count - 1; i >= 0; i--)
        {
            int indexToRemove = effectsToRemove[i];
            effectsList.RemoveAt(indexToRemove);
        }
    }
    public List<Character> GetAdjacentTargets(Character mainTarget)
    {
        List<Character> adjacentTargets = new List<Character>();

        // Récupérer l'index de la mainTarget dans la liste des héros
        int mainTargetIndex = TeamClass.instance.heroTeam.IndexOf(mainTarget.gameObject.GetComponent<HeroClass>());

        // Utiliser un switch pour déterminer les indices des deux personnages adjacents
        switch (mainTargetIndex)
        {
            case 0:
                // MainTarget est à l'index 0, les adjacents sont à l'index 1 et 3
                adjacentTargets.Add(TeamClass.instance.heroTeam[1]);
                adjacentTargets.Add(TeamClass.instance.heroTeam[2]);
                break;
            case 1:
                // MainTarget est à l'index 1, les adjacents sont à l'index 0 et 2
                adjacentTargets.Add(TeamClass.instance.heroTeam[0]);
                if (TeamClass.instance.heroTeam.Count == 4)
                {
                    adjacentTargets.Add(TeamClass.instance.heroTeam[3]);
                }

                break;
            case 2:
                // MainTarget est à l'index 2, les adjacents sont à l'index 1 et 3
                adjacentTargets.Add(TeamClass.instance.heroTeam[0]);
                if (TeamClass.instance.heroTeam.Count == 4)
                {
                    adjacentTargets.Add(TeamClass.instance.heroTeam[3]);
                }

                break;
            case 3:
                // MainTarget est à l'index 3, les adjacents sont à l'index 0 et 2
                adjacentTargets.Add(TeamClass.instance.heroTeam[2]);
                adjacentTargets.Add(TeamClass.instance.heroTeam[1]);
                break;
            default:
                break;
        }

        return adjacentTargets;
    }

    public IEnumerator SetAnimation(bool state)
    {
        Animator tempAnim = GetComponent<Animator>();
        if (state)
        {
            tempAnim.enabled = true;
        }
        else
        {
            tempAnim.enabled = false;
            GetComponent<SpriteRenderer>().sprite = sideSprite;
        }
        yield return null;
    }
}

