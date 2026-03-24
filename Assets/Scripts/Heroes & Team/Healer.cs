using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Healer : HeroClass
{

    public void InitializeBasicData()
    {
        shadow = gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
        sp = GetComponent<SpriteRenderer>();
        sp.material = defaultMaterial;
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animController;
        characterName = "Mystos";
        characterClass = "Healer";
        name = "Mystos";
        healthPerLevel = 4;
        damagePerLevel = 2;
        maxHP = 75;
        currentHP = 75;
        baseHeroXP = 110;
        currentLvlCap = 0.5f * Mathf.Pow(level * 2, 3) + 6;
        currentSkills.Add(new BaseAttack());
        currentSkills.Add(new HealSkill());
        currentSkills.Add(new DefBufSkill());
        currentSkills.Add(new ATQBuffSkill());
    }

    public void InitializeSkills(int[] lSkills)
    {

    }

}
