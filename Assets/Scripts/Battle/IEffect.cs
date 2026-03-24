using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    string EffectName { get; }
    bool EffectUsed { get; }

    public void AddEffect(Character effectTarget)
    {
        effectTarget.effectsList.Add(this);
    }

    public void ActivateEffect(Character effectTarget)
    {
    }

}

public class BuffATTEffect : IEffect
{
    public string EffectName { get; private set; }
    public bool EffectUsed { get; private set; }

    public BuffATTEffect()
    {
        EffectName = "Attack Buff";
        EffectUsed = false;
    }

    public void ActivateEffect(Character effectTarget)
    {
        EffectUsed = true;
        foreach (var skill in effectTarget.currentSkills)
        {
            skill.SkillDamageAfterEffect = skill.SkillPower + (skill.SkillPower / 2);
        }
    }

}

public class BuffDefenseEffect : IEffect
{
    public string EffectName { get; private set; }
    public bool EffectUsed { get; private set; }
    public int counterBeforeUsed = 3;

    public BuffDefenseEffect()
    {
        EffectName = "Defense Buff";
        EffectUsed = false;
    }

    public void ActivateEffect(Character effectTarget)
    {
        //+50% d�fense sur 3 tours � voir comment int�grer
        if (!encounterActiveEffect())
        {
            effectTarget.isDefenseBuffed = true;
            counterBeforeUsed -= 1;
        }
    }

    bool encounterActiveEffect()
    {
        if (counterBeforeUsed == 0)
        {
            EffectUsed = true;
        }

        return EffectUsed;
    }

}

public class HeatingEffect : IEffect
{
    public string EffectName { get; private set; }
    public bool EffectUsed { get; private set; }
    public int counterBeforeUsed = 3;

    public HeatingEffect()
    {
        EffectName = "Heating";
        EffectUsed = false;
    }

    public void ActivateEffect(Character effectTarget)  // Every round 
    {
        if (!encounterActiveEffect())
        {
            effectTarget.currentHP -= 6;
            counterBeforeUsed -= 1;
        }

    }

    bool encounterActiveEffect()
    {
        if (counterBeforeUsed == 0)
        {
            EffectUsed = true;
        }

        return EffectUsed;
    }

}

public class AggrosEffect : IEffect
{
    public string EffectName { get; private set; }
    public bool EffectUsed { get; private set; }

    public AggrosEffect()
    {
        EffectName = "Aggro";
        EffectUsed = false;
    }

    public void ActivateEffect(Character effectTarget)
    {
        EffectUsed = true;
        // A voir comment l'int�grer
    }

}


public class ProtectionEffect : IEffect
{
    public string EffectName { get; private set; }
    public bool EffectUsed { get; private set; }

    public ProtectionEffect()
    {
        EffectName = "Protection";
        EffectUsed = false;
    }

    public void ActivateEffect(Character effectTarget)
    {
        EffectUsed = true;
        // A voir comment l'int�grer
    }

}

public class DebuffAttackEffect : IEffect
{
    public string EffectName { get; private set; }
    public bool EffectUsed { get; private set; }
    public int counterBeforeUsed = 3;
    public bool isDebuffed = false;

    public DebuffAttackEffect()
    {
        EffectName = "Debuff attck";
        EffectUsed = false;
    }

    public void ActivateEffect(Character effectTarget)
    {
        //-50% d'attaque sur 3 tours 
        if (!encounterActiveEffect())
        {
            if (!isDebuffed)
            {
                foreach (var skill in effectTarget.currentSkills)
                {
                    skill.SkillDamageAfterEffect = skill.SkillPower - (skill.SkillPower / 2);
                }
                isDebuffed = true;
            }
            counterBeforeUsed -= 1;
        }
    }

    bool encounterActiveEffect()
    {
        if (counterBeforeUsed == 0)
        {
            EffectUsed = true;
        }

        return EffectUsed;
    }

}

public class DebuffDefenseEffect : IEffect
{
    public string EffectName { get; private set; }
    public bool EffectUsed { get; private set; }

    public DebuffDefenseEffect()
    {
        EffectName = "Debuff defense";
        EffectUsed = false;
    }

    public void ActivateEffect(Character effectTarget)
    {
        //-50% de defense sur 3 tours 
        // A voir comment l'int�grer
    }

    /*bool encounterActiveEffect()
    {
        if (counterBeforeUsed == 0)
        {
            EffectUsed = true;
        }

        return EffectUsed;
    }*/

}


public class GluEffect : IEffect
{
    public string EffectName { get; private set; }
    public bool EffectUsed { get; private set; }

    public GluEffect()
    {
        EffectName = "Glued";
        EffectUsed = false;
    }

    public void ActivateEffect(Character effectTarget)
    {
        EffectUsed = true;
        UIFight.instance.NextCharacter();
    }
}

public class FriendshipEffect : IEffect
{
    public string EffectName { get; private set; }
    public bool EffectUsed { get; private set; }

    public FriendshipEffect()
    {
        EffectName = "Friendship";
        EffectUsed = false;
    }

    public void ActivateEffect(lilThingScript effectTarget)
    {
        if (effectTarget.isInvincible)
        {

        }
    }
}
