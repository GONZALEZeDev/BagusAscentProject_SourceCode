using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager instance;

    //Refer to the inspector for the detailed lists
    public AudioClip[] uiSfxClips;
    public AudioClip[] heroesAttackSfxClips;
    public AudioClip[] ennemiesAttackSfxClips;
    public AudioClip[] playerMoveSfxClips;
    public AudioClip[] otherSfxClips;

    public AudioSource uiSfxSource;
    public AudioSource playerSfxSource;
    public AudioSource otherSfxSource;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

    }

    public void PlayUISfx(int index)
    {
        uiSfxSource.Stop();
        uiSfxSource.clip = uiSfxClips[index];
        uiSfxSource.Play();
    }

    public IEnumerator PlayHeroAttackSfx(int index, float delay = 0f)
    {
        otherSfxSource.Stop();
        otherSfxSource.clip = heroesAttackSfxClips[index];
        //Petit temps pour que le son ne se joue pas pile au moment où l'attaque est choisie
        yield return new WaitForSecondsRealtime(delay);
        otherSfxSource.Play();
    }

    public IEnumerator PlayHeroAttackSfx(ISkill skill, float delay = 0f)
    {
        otherSfxSource.Stop();
        switch (skill.SkillName)
        {
            case "Sharpened Weapon":
                otherSfxSource.clip = heroesAttackSfxClips[5];
                break;
            case "Heating Weapon":
                otherSfxSource.clip = heroesAttackSfxClips[6];
                break;
            case "Aggro":
                otherSfxSource.clip = heroesAttackSfxClips[7];
                break;
            case "Mana Transfer":
                otherSfxSource.clip = heroesAttackSfxClips[8];
                break;
            case "Absorb":
                otherSfxSource.clip = heroesAttackSfxClips[9];
                break;
            case "Heal":
                otherSfxSource.clip = heroesAttackSfxClips[10];
                break;
            case "Defense Buff":
                otherSfxSource.clip = heroesAttackSfxClips[11];
                break;
            case "Attack Buff":
                otherSfxSource.clip = heroesAttackSfxClips[12];
                break;
            default:
                break;
        }
        //Petit temps pour que le son ne se joue pas pile au moment où l'attaque est choisie
        yield return new WaitForSecondsRealtime(delay);
        otherSfxSource.Play();
    }

    public IEnumerator PlayEnnemiesAttackSfx(int index, float delay = 0f)
    {
        otherSfxSource.Stop();
        otherSfxSource.clip = ennemiesAttackSfxClips[index];
        //Petit temps pour que le son ne se joue pas pile au moment où l'attaque est choisie
        yield return new WaitForSecondsRealtime(delay);
        otherSfxSource.Play();
    }

    public IEnumerator PlayEnnemiesAttackSfx(ISkill skill, float delay = 0f)
    {
        otherSfxSource.Stop();
        switch (skill.SkillName)
        {
            case "Strike":
                otherSfxSource.clip = ennemiesAttackSfxClips[1];
                break;
            case "Splash":
                otherSfxSource.clip = ennemiesAttackSfxClips[2];
                break;
            case "Kaboom":
                otherSfxSource.clip = ennemiesAttackSfxClips[3];
                break;
            case "Glu Trap":
                otherSfxSource.clip = ennemiesAttackSfxClips[4];
                break;
            case "Lil Buff":
                otherSfxSource.clip = ennemiesAttackSfxClips[5];
                break;
            case "Smash":
                otherSfxSource.clip = ennemiesAttackSfxClips[6];
                break;
            case "No No No !!!":
                otherSfxSource.clip = ennemiesAttackSfxClips[7];
                break;
            case "Biting":
                otherSfxSource.clip = ennemiesAttackSfxClips[8];
                break;
            case "Lava ball":
                otherSfxSource.clip = ennemiesAttackSfxClips[9];
                break;
            case "Groar !":
                otherSfxSource.clip = ennemiesAttackSfxClips[10];
                break;
            case "Rolling":
                otherSfxSource.clip = ennemiesAttackSfxClips[11];
                break;
            case "Lawn Mower":
                otherSfxSource.clip = ennemiesAttackSfxClips[12];
                break;
            case "Sweet Perfum":
                otherSfxSource.clip = ennemiesAttackSfxClips[13];
                break;
            default:
                break;
        }
        //Petit temps pour que le son ne se joue pas pile au moment où l'attaque est choisie
        yield return new WaitForSecondsRealtime(delay);
        otherSfxSource.Play();
    }

    public void PlayPlayerWalkSfx(int index)
    {
        playerSfxSource.Stop();
        playerSfxSource.clip = playerMoveSfxClips[index];
        playerSfxSource.Play();
    }

    public void PlayOtherSfx(int index)
    {
        otherSfxSource.Stop();
        otherSfxSource.clip = otherSfxClips[index];
        otherSfxSource.Play();
    }
}
