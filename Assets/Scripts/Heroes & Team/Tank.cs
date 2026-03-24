using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tank : HeroClass
{

    public void InitializeBasicData()
    {
        shadow = gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
        sp = GetComponent<SpriteRenderer>();
        sp.material = defaultMaterial;
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animController;
        characterName = "Kant";
        characterClass = "Tank";
        name = "Kant";
        currentLvlCap = 0.5f * Mathf.Pow(level * 2, 3) + 6;
        healthPerLevel = 9;
        damagePerLevel = 3;
        maxHP = 200;
        currentHP = 200;
        baseHeroXP = 120;
        currentSkills.Add(new BaseAttack());
        currentSkills.Add(new AggroSkill());
    }

    //Sert à remplir currentSkills des skills choisis en montant de niveaux.
    //Comme cette fonctionnalité n'est pas encore implémentée, la fonction est vide.
    public void InitializeSkills(int[] lSkills)
    {

    }
}
