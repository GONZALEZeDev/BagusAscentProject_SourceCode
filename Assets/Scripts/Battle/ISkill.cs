using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Transformez Skill en interface
public interface ISkill
{
    int SkillId { get; }
    string SkillName { get; }
    int SkillCost { get; }
    int SkillPower { get; }

    int SkillDamageAfterEffect { get; set; }

    string SkillDesc { get; }
    string SkillType { get; }

    //est-ce que le skill a pour cible le camp des héros (true) ou celui des ennemis (false)
    bool SkillAllie { get; }

    public void ActivateAttackEffect(Character attackOwner, Character attackTarget)
    {
        if (SkillPower > 0)
        {
            attackOwner.currentMana -= SkillCost;
            int damageAttack = SkillDamageAfterEffect;

            if (attackTarget.isblocking == true)
            {
                if (attackTarget.isDefenseBuffed)
                {
                    if (attackOwner.CompareTag("heroe"))
                    {
                        damageAttack = (damageAttack + (attackOwner.damagePerLevel * (attackOwner.level - 1))) / 3;
                        TeamClass.instance.StoreAttackXP(attackOwner.characterName, attackTarget.level, damageAttack);
                    }
                    else damageAttack = damageAttack / 3;
                    attackTarget.currentHP -= damageAttack;
                    attackTarget.isDefenseBuffed = false;
                }
                else
                {
                    if (attackOwner.CompareTag("heroe"))
                    {
                        damageAttack = damageAttack + (attackOwner.damagePerLevel * (attackOwner.level - 1)) - (damageAttack + (attackOwner.damagePerLevel * (attackOwner.level - 1)) / 4);
                        TeamClass.instance.StoreAttackXP(attackOwner.characterName, attackTarget.level, damageAttack);
                    }
                    else damageAttack = damageAttack - (damageAttack / 4);
                    attackTarget.currentHP -= damageAttack;
                }

                // Restauration de mana pour l'ennemi ciblé
                if (attackTarget.maxMana - attackTarget.currentMana >= 15)
                {
                    attackTarget.currentMana += 15;
                }
                else
                {
                    attackTarget.currentMana = attackTarget.maxMana;
                }
            }
            else
            {
                if (attackOwner.CompareTag("heroe"))
                {
                    damageAttack += (attackOwner.damagePerLevel * (attackOwner.level - 1));
                    TeamClass.instance.StoreAttackXP(attackOwner.characterName, attackTarget.level, damageAttack);
                }
                attackTarget.currentHP -= damageAttack;
            }

            attackTarget.isblocking = false;
        }
    }
}

public class BaseAttack : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }


    public BaseAttack()
    {
        SkillId = 0;
        SkillName = "Strike";
        SkillCost = 0;
        SkillPower = 10;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "A basic attack.";
        SkillType = "Tous";
        SkillAllie = false;
    }
}

public class MagicAttack : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }


    public MagicAttack()
    {
        SkillId = 1;
        SkillName = "Magic";
        SkillCost = 15;
        SkillPower = 10;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "A magic attack.";
        SkillType = "Tous";
        SkillAllie = false;
    }

}


// Liste des différentes attaques de tout les ennemies classé par type d'ennemie

// Attaque type Slime 

public class SplashAttack : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public SplashAttack()
    {
        SkillId = 2;
        SkillName = "Splash";
        SkillCost = 10;
        SkillPower = 10;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Skill hitting a target and 2 adjacent allies";
        SkillType = "Slime";
        SkillAllie = false;
    }


    public void splashAttackEffect(Character attackOwner, List<Character> adjacentTargets)
    {
        int damageToAdjacentTargets = (SkillDamageAfterEffect + attackOwner.level * attackOwner.damagePerLevel) / 3; // Dégâts réduits pour les personnages adjacents
        // Appliquer des dégâts réduits aux deux personnages adjacent
        foreach (Character adjacentTarget in adjacentTargets)
        {
            adjacentTarget.currentHP -= damageToAdjacentTargets;
        }
    }
}

public class GluTrapAttack : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public GluTrapAttack()
    {
        SkillId = 3;
        SkillName = "Glu Trap";
        SkillCost = 20;
        SkillPower = 6;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Inflicts damage to the target and makes them skip their next turn.";
        SkillType = "Slime Glu";
        SkillAllie = false;
    }
}

public class KaboomAttack : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public KaboomAttack()
    {
        SkillId = 4;
        SkillName = "Kaboom";
        SkillCost = 20;
        SkillPower = 10;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Inflicts damage to every opponent and applies a Burn effect during 3 turns.";
        SkillType = "Slime Glu Nyama";
        SkillAllie = false;
    }

    public void ActivateAttackEffect(Character attackOwner, List<HeroClass> attackTargets)
    {
        attackOwner.currentMana -= SkillCost;
        int damageAttack = SkillDamageAfterEffect + attackOwner.level * attackOwner.damagePerLevel;
        foreach (var hero in attackTargets)
        {
            if (hero.isblocking == true)
            {
                if (hero.isDefenseBuffed)
                {
                    hero.currentHP -= (damageAttack / 3);
                    hero.isDefenseBuffed = false;
                }
                else
                {
                    hero.currentHP -= (damageAttack - (damageAttack / 4));
                    // Restauration de mana pour l'ennemi ciblé
                    if (hero.maxMana - hero.currentMana >= 15)
                    {
                        hero.currentMana += 15;
                    }
                    else
                    {
                        hero.currentMana = hero.maxMana;
                    }
                }
            }
            else
            {
                hero.currentHP -= damageAttack;
            }

            hero.isblocking = false;
            hero.effectsList.Add(new HeatingEffect());
        }
    }
}


// Attaque du Kokodemon

public class Groaaaaarrr : ISkill
{
    public string SkillName { get; private set; }
    public int SkillId { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public Groaaaaarrr()
    {
        SkillId = 0;
        SkillName = "Groar !";
        SkillCost = 15;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Buffs the attacks of kokodemon";
        SkillType = "Kokodemon";
        SkillAllie = true;
    }

    public void ActivateAttackEffect(Character attackOwner)
    {
        attackOwner.currentMana -= SkillCost;
        attackOwner.effectsList.Add(new BuffATTEffect());
    }
}

public class LavaBall : ISkill
{
    public string SkillName { get; private set; }
    public int SkillId { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public LavaBall()
    {
        SkillId = 0;
        SkillName = "Lava ball";
        SkillCost = 30;
        SkillPower = 20;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Throws a lava ball to the main target and burns adjacent targets";
        SkillType = "Kokodemon";
        SkillAllie = false;
    }
    public void ActivateAttackEffect(Character attackOwner, Character attackTarget)
    {
        attackOwner.currentMana -= SkillCost;
        int damageAttack = SkillDamageAfterEffect + attackOwner.level * attackOwner.damagePerLevel;

        if (attackTarget.isblocking == true)
        {
            if (attackTarget.isDefenseBuffed)
            {
                attackTarget.currentHP -= (damageAttack / 3);
                attackTarget.isDefenseBuffed = false;
            }
            else
            {
                attackTarget.currentHP -= (damageAttack - (damageAttack / 4));
                // Restauration de mana pour l'ennemi ciblé
                if (attackTarget.maxMana - attackTarget.currentMana >= 15)
                {
                    attackTarget.currentMana += 15;
                }
                else
                {
                    attackTarget.currentMana = attackTarget.maxMana;
                }
            }
        }
        else
        {
            attackTarget.currentHP -= damageAttack;
        }

        attackTarget.isblocking = false;
    }

    public void lavaBallAttackEffect(List<Character> adjacentTargets)
    {
        // Appliquer l'effet de brûlure aux deux personnages adjacents
        foreach (Character adjacentTarget in adjacentTargets)
        {
            HeatingEffect heat = new HeatingEffect();
            adjacentTarget.effectsList.Add(heat);
        }
    }
}

public class BitingSkill : ISkill
{
    public string SkillName { get; private set; }
    public int SkillId { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public BitingSkill()
    {
        SkillId = 0;
        SkillName = "Biting";
        SkillCost = 20;
        SkillPower = 20;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Bites the main target.";
        SkillType = "Kokodemon";
        SkillAllie = false;
    }
}

// Attaque du Lil Thing 


public class SmashSkill : ISkill
{
    public string SkillName { get; private set; }
    public int SkillId { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public SmashSkill()
    {
        SkillId = 0;
        SkillName = "Smash";
        SkillCost = 15;
        SkillPower = 10;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Does 10 damage to the main target and 5 damage to adjacents targets";
        SkillType = "lilthing";
        SkillAllie = false;
    }


    public void smashAttackEffect(Character attackOwner, List<Character> adjacentTargets)
    {
        int damageToAdjacentTargets = (SkillDamageAfterEffect + attackOwner.level * attackOwner.damagePerLevel)/2;
        // Appliquer des dégâts réduits aux deux personnages adjacent
        foreach (Character adjacentTarget in adjacentTargets)
        {
            adjacentTarget.currentHP -= damageToAdjacentTargets;
        }
    }
}


public class NoNoNoSkill : ISkill
{
    public string SkillName { get; private set; }
    public int SkillId { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public NoNoNoSkill()
    {
        SkillId = 0;
        SkillName = "No No No !!!";
        SkillCost = 20;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Debuffs every hero next attack";
        SkillType = "lilthing";
        SkillAllie = true;
    }

    public void ActivateAttackEffect(Character attackOwner, List<HeroClass> attackTargets)
    {
        attackOwner.currentMana -= SkillCost;
        foreach (var hero in attackTargets)
        {
            DebuffAttackEffect debuff = new DebuffAttackEffect();
            hero.effectsList.Add(debuff);
        }
    }

}


public class LilBuffSkill : ISkill
{
    public string SkillName { get; private set; }
    public int SkillId { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public LilBuffSkill()
    {
        SkillId = 0;
        SkillName = "Lil Buff";
        SkillCost = 20;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Buffs an ally's attack for the next 3 rounds !";
        SkillType = "lilthing";
        SkillAllie = true;
    }

    public void ActivateAttackEffect(Character attackOwner, Character attackTarget)
    {
        attackOwner.currentMana -= SkillCost;
        BuffATTEffect buffAtt = new BuffATTEffect();
        attackTarget.effectsList.Add(buffAtt);
    }

}

public class FriendshipSkill : ISkill
{
    public string SkillName { get; private set; }
    public int SkillId { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public FriendshipSkill()
    {
        SkillId = 0;
        SkillName = "Friendship";
        SkillCost = 30;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Can't be under 50% of max HP until all his allies are alive !";
        SkillType = "lilthing";
        SkillAllie = true;
    }

    public void ActivateAttackEffect(Character attackOwner)
    {
        attackOwner.currentMana -= SkillCost;

    }

}

//Plantula attaques
public class LawnMower : ISkill
{
    public string SkillName { get; private set; }
    public int SkillId { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public LawnMower()
    {
        SkillId = 0;
        SkillName = "Lawn Mower";
        SkillCost = 20;
        SkillPower = 15;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Does 15 damage to every hero !";
        SkillType = "plantula";
        SkillAllie = false;
    }

    public void ActivateAttackEffect(Character attackOwner, List<HeroClass> attackTarget)
    {
        attackOwner.currentMana -= SkillCost;
        int damageAttack = SkillDamageAfterEffect + attackOwner.level * attackOwner.damagePerLevel;
        foreach (var hero in attackTarget)
        {
            if (hero.isblocking == true)
            {
                if (hero.isDefenseBuffed)
                {
                    hero.currentHP -= (damageAttack / 3);
                    hero.isDefenseBuffed = false;
                }
                else
                {
                    hero.currentHP -= (damageAttack - (damageAttack / 4));
                    // Restauration de mana pour l'ennemi ciblé
                    if (hero.maxMana - hero.currentMana >= 15)
                    {
                        hero.currentMana += 15;
                    }
                    else
                    {
                        hero.currentMana = hero.maxMana;
                    }
                }
            }
            else
            {
                hero.currentHP -= damageAttack;
            }

            hero.isblocking = false;
        }

    }
}

public class Rolling : ISkill
{
    public string SkillName { get; private set; }
    public int SkillId { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public Rolling()
    {
        SkillId = 0;
        SkillName = "Rolling";
        SkillCost = 10;
        SkillPower = 10;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Does 10 damages to the main target.";
        SkillType = "plantula";
        SkillAllie = false;
    }

}


public class SweetPerfum : ISkill
{
    public string SkillName { get; private set; }
    public int SkillId { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public SweetPerfum()
    {
        SkillId = 0;
        SkillName = "Sweet Perfum";
        SkillCost = 25;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Debuffs the attack of the main target !";
        SkillType = "plantula";
        SkillAllie = true;
    }

    public void ActivateAttackEffect(Character attackOwner, Character attackTarget)
    {
        attackOwner.currentMana -= SkillCost;
        DebuffAttackEffect debuff = new DebuffAttackEffect();
        attackTarget.effectsList.Add(debuff);
    }
}

// Attaques type Mage 

public class HealSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public HealSkill()
    {
        SkillId = 4;
        SkillName = "Heal";
        SkillCost = 15;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "A basic healing spell to use on your allies.";
        SkillType = "Base Attack";
        SkillAllie = true;
    }

    public void ActivateAttackEffect(Character attackOwner, Character attackTarget)
    {
        Debug.Log(attackTarget.maxHP + attackTarget.currentHP + attackOwner.damagePerLevel + attackTarget.level);
        attackOwner.currentMana -= SkillCost;
        if (attackTarget.maxHP - attackTarget.currentHP >= (20 * ((20+attackOwner.damagePerLevel*attackTarget.level)/20)))
        {
            if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreHealingXP(attackOwner.characterName, (20 * ((20 + attackOwner.damagePerLevel * attackTarget.level) / 20)), attackTarget.level);
            attackTarget.currentHP += (20 * ((20 + attackOwner.damagePerLevel * attackTarget.level) / 20));
        }
        else
        {
            if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreHealingXP(attackOwner.characterName, (attackTarget.maxHP - attackTarget.currentHP) * (((attackTarget.maxHP - attackTarget.currentHP) + attackOwner.damagePerLevel * attackTarget.level) / 20), attackTarget.maxHP);
            attackTarget.currentHP = attackTarget.maxHP;
        }
    }
}

public class ATQBuffSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public ATQBuffSkill()
    {
        SkillId = 5;
        SkillName = "Attack Buff";
        SkillCost = 20;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Buffs the attack of one of your allies. 50% attack buff for the next attack.";
        SkillType = "Mage Tier1";
        SkillAllie = true;
    }

    public void ActivateAttackEffect(Character attackOwner, Character attackTarget)
    {
        attackOwner.currentMana -= SkillCost;
        if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreOtherSkillXP(attackOwner.characterName, attackTarget.level, SkillCost);
        attackTarget.effectsList.Add(new BuffATTEffect());
    }

}

public class DefBufSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public DefBufSkill()
    {
        SkillId = 6;
        SkillName = "Defense Buff";
        SkillCost = 20;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Buffs the defense of one of your allies. 50% defense buff for the 3 next rounds.";
        SkillType = "Mage Tier1";
        SkillAllie = true;
    }

    public void ActivateAttackEffect(Character attackOwner, Character attackTarget)
    {
        attackOwner.currentMana -= SkillCost;
        attackTarget.isblocking = false;
        bool isTargetAlreadyDefenseBuff = false;
        foreach (var effect in attackTarget.effectsList)
        {
            if (effect.EffectName == "Defense Buff")
            {
                isTargetAlreadyDefenseBuff = true;
                break;
            }
        }
        if (!isTargetAlreadyDefenseBuff)
        {
            if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreOtherSkillXP(attackOwner.characterName, attackTarget.level, SkillCost);
            attackTarget.effectsList.Add(new BuffDefenseEffect());
        }

    }

}

public class FireBallSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public FireBallSkill()
    {
        SkillId = 7;
        SkillName = "Fire Ball";
        SkillCost = 25;
        SkillPower = 25;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Throw a fire ball to an ennemy";
        SkillType = "Mage Tier2";
        SkillAllie = false;
    }

}


// Attaques type DPS


public class SharpenedWeaponSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public SharpenedWeaponSkill()
    {
        SkillId = 9;
        SkillName = "Sharpened Weapon";
        SkillCost = 17;
        SkillPower = 25;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Deals a strong slash to the opponent.";
        SkillType = "Base Attack";
        SkillAllie = false;
    }

}

public class AutoATQBuffSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public AutoATQBuffSkill()
    {
        SkillId = 10;
        SkillName = "Auto ATT Buff";
        SkillCost = 20;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Applies a +50% attack buff to yourself.";
        SkillType = "DPS Tier1";
        SkillAllie = false;
    }

    public void ActivateAttackEffect(Character attackOwner)
    {
        if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreOtherSkillXP(attackOwner.characterName, attackOwner.level, SkillCost);
        attackOwner.currentMana -= SkillCost;
    }

}


public class HeatingWeaponSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public HeatingWeaponSkill()
    {
        SkillId = 11;
        SkillName = "Heating Weapon";
        SkillCost = 22;
        SkillPower = 35;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "A powerfull attack that burns the target for 3 rounds";
        SkillType = "DPS Tier1";
        SkillAllie = false;
    }

    public void ActivateAttackEffect(Character attackOwner, Character attackTarget)
    {
        attackOwner.currentMana -= SkillCost;
        int damageAttack = SkillDamageAfterEffect;

        if (attackTarget.isblocking == true)
        {
            damageAttack = damageAttack + (attackOwner.damagePerLevel * (attackOwner.level-1)) - (damageAttack + (attackOwner.damagePerLevel * (attackOwner.level-1)) / 4);
            attackTarget.currentHP -= damageAttack;

            // Restauration de mana pour l'ennemi ciblé
            if (attackTarget.maxMana - attackTarget.currentMana >= 15)
            {
                attackTarget.currentMana += 15;
            }
            else
            {
                attackTarget.currentMana = attackTarget.maxMana;
            }
        }
        else
        {
            attackTarget.currentHP -= damageAttack + (attackOwner.damagePerLevel * (attackOwner.level - 1));
        }

        if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreAttackXP(attackOwner.characterName, attackTarget.level, damageAttack);

        attackTarget.isblocking = false;
        bool isTargetAlreadyBurning = false;
        foreach (var effect in attackTarget.effectsList)
        {
            if (effect.EffectName == "Heating")
            {
                isTargetAlreadyBurning = true;
                break;
            }
        }
        if (!isTargetAlreadyBurning)
        {
            if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreOtherSkillXP(attackOwner.characterName, attackTarget.level, SkillCost);
            attackTarget.effectsList.Add(new HeatingEffect());
        }

    }

}


public class DisarmSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public DisarmSkill()
    {
        SkillId = 12;
        SkillName = "Disarm";
        SkillCost = 30;
        SkillPower = 15;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Deals damage and disarms the target, making them skip their next turn.";
        SkillType = "DPS Tier2";
        SkillAllie = false;
    }

}



// Attques type TANK


public class AggroSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public AggroSkill()
    {
        SkillId = 14;
        SkillName = "Aggro";
        SkillCost = 10;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Redirects the next attack of the target on you.";
        SkillType = "Base Attack";
        SkillAllie = false;
    }

    public void ActivateAttackEffect(Character attackOwner, Ennemies attackTarget)
    {
        if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreOtherSkillXP(attackOwner.characterName, attackTarget.level, SkillCost);
        attackOwner.currentMana -= SkillCost;
        attackTarget.agroTarget = attackOwner;
    }

}


public class ProtectionSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public ProtectionSkill()
    {
        SkillId = 15;
        SkillName = "Protection";
        SkillCost = 15;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Protects you more than Block, but you have no mana recovery.";
        SkillType = "Tank Tier1";
        SkillAllie = false;
    }

    public void ActivateAttackEffect(Character attackOwner)
    {
        if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreOtherSkillXP(attackOwner.characterName, attackOwner.level, SkillCost);
        attackOwner.currentMana -= SkillCost;
    }

}


public class HammerShockSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public HammerShockSkill()
    {
        SkillId = 16;
        SkillName = "Hammer Shock";
        SkillCost = 17;
        SkillPower = 25;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "A strong hammer strike !";
        SkillType = "Tank Tier1";
        SkillAllie = false;
    }

}


public class CollectiveDefenseSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public CollectiveDefenseSkill()
    {
        SkillId = 17;
        SkillName = "Collective Defense";
        SkillCost = 22;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Applies a defense buff for everyone in your team.";
        SkillType = "Tank Tier2";
        SkillAllie = false;
    }

    public void ActivateAttackEffect(Character attackOwner)
    {
        if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreOtherSkillXP(attackOwner.characterName, attackOwner.level, SkillCost);
        attackOwner.currentMana -= SkillCost;
    }

}



// Attaques type Voleur


public class ManaTransferSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public ManaTransferSkill()
    {
        SkillId = 19;
        SkillName = "Mana Transfer";
        SkillCost = 10;
        SkillPower = 0;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Allows to transfer mana from an ally to another";
        SkillType = "Base Attack";
        SkillAllie = true;
    }

    public void ActiveManaTransfer(Character attackOwner)
    {
        if (attackOwner.CompareTag("heroe")) TeamClass.instance.StoreOtherSkillXP(attackOwner.characterName, attackOwner.level, SkillCost);
        attackOwner.currentMana -= SkillCost;
    }


    public void Transfer(Character fromAllie, Character toAllie, int quantity)
    {
        if (toAllie.currentMana + quantity > toAllie.maxMana)
        {
            quantity = toAllie.maxMana - toAllie.currentMana;
            Debug.Log("Amount = " + quantity);
            fromAllie.currentMana -= quantity;
            toAllie.currentMana += quantity;
        }
        else
        {
            fromAllie.currentMana -= quantity;
            toAllie.currentMana += quantity;
        }

    }

}



public class StealingLifeSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public StealingLifeSkill()
    {
        SkillId = 20;
        SkillName = "Absorb";
        SkillCost = 17;
        SkillPower = 18;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Steals life of the target to heal you.";
        SkillType = "Voleur Tier1";
        SkillAllie = false;
    }

    public void ActivateAttackEffect(Character attackOwner, Character attackTarget)
    {
        attackOwner.currentMana -= SkillCost;
        int damageAttack = SkillDamageAfterEffect + (attackOwner.damagePerLevel * (attackOwner.level - 1));

        if (attackTarget.isblocking == true)
        {
            damageAttack = SkillDamageAfterEffect + (attackOwner.damagePerLevel * (attackOwner.level - 1)) - (SkillDamageAfterEffect + (attackOwner.damagePerLevel * (attackOwner.level - 1)) / 4);
            // Restauration de mana pour l'ennemi ciblé
            if (attackTarget.maxMana - attackTarget.currentMana >= 15)
            {
                attackTarget.currentMana += 15;
            }
            else
            {
                attackTarget.currentMana = attackTarget.maxMana;
            }
        }
        attackTarget.currentHP -= damageAttack;
        attackOwner.currentHP += damageAttack / 3;
        if(attackOwner.currentHP > attackOwner.maxHP) attackOwner.currentHP = attackOwner.maxHP;

        if (attackOwner.CompareTag("heroe"))
        {
            TeamClass.instance.StoreAttackXP(attackOwner.characterName, attackTarget.level, damageAttack);
            TeamClass.instance.StoreHealingXP(attackOwner.characterName, damageAttack / 3, attackTarget.level);
        }

        attackTarget.isblocking = false;


    }

}


public class DebuffAttackSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public DebuffAttackSkill()
    {
        SkillId = 21;
        SkillName = "Debuff Attack";
        SkillCost = 17;
        SkillPower = 8;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Deals a few damage and makes the target's next attack weaker.";
        SkillType = "Voleur Tier1";
        SkillAllie = false;
    }

}


public class DebuffDefenseSkill : ISkill
{
    public int SkillId { get; private set; }
    public string SkillName { get; private set; }
    public int SkillCost { get; private set; }
    public int SkillPower { get; private set; }
    public int SkillDamageAfterEffect { get; set; }
    public string SkillDesc { get; private set; }
    public string SkillType { get; private set; }
    public bool SkillAllie { get; private set; }

    public DebuffDefenseSkill()
    {
        SkillId = 22;
        SkillName = "Debuff Defense";
        SkillCost = 17;
        SkillPower = 8;
        SkillDamageAfterEffect = SkillPower;
        SkillDesc = "Deals a few damage and makes the target more vulnerable to the next attack they receive.";
        SkillType = "Voleur Tier2";
        SkillAllie = false;
    }
}