using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DPS : HeroClass
{

    public void InitializeBasicData()
    {
        shadow = gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
        sp = GetComponent<SpriteRenderer>();
        sp.material = defaultMaterial;
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animController;
        characterName = "Gus";
        characterClass = "DPS";
        name = "Gus";
        currentLvlCap = 0.5f * Mathf.Pow(level * 2, 3) + 6;
        healthPerLevel = 6;
        damagePerLevel = 5;
        maxHP = 100;
        currentHP = 100;
        baseHeroXP = 80;
        currentSkills.Add(new BaseAttack());
        currentSkills.Add(new SharpenedWeaponSkill());
        currentSkills.Add(new HeatingWeaponSkill());
    }

    public void InitializeSkills(int[] lSkills)
    {

    }

}
