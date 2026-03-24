using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TeamClass : MonoBehaviour
{
    public static TeamClass instance;

    public List<GameObject> heroesGO;

    public List<HeroClass> heroTeam;

    public GameObject moveKeybinds;

    //File des alliés suivant le joueur
    public Queue<Vector3> leaderTrail;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        moveKeybinds.SetActive(false);
    }

    public void UpdateFollowersQueue()
    {
        /*
         * On attache un PlayerController à tout les membres, et on ne l'active que lorsque c'est le leader, same avec les followers
         */

        //Puis attribuer des playerFollower aux 3 autres
        for (int i = 0; i < GetAliveHeroes().Count; i++)
        {
            if (i == 0)
            {
                //Player
                GetAliveHeroes()[i].gameObject.GetComponent<playerFollower>().enabled = false;
                GetAliveHeroes()[i].gameObject.GetComponent<PlayerController>().enabled = true;
                GetAliveHeroes()[i].gameObject.GetComponent<EncounterController>().enabled = true;
                GetAliveHeroes()[i].gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            }
            else
            {
                //Follower
                GetAliveHeroes()[i].gameObject.GetComponent<playerFollower>().enabled = true;
                GetAliveHeroes()[i].gameObject.GetComponent<PlayerController>().enabled = false;
                GetAliveHeroes()[i].gameObject.GetComponent<EncounterController>().enabled = false;
                GetAliveHeroes()[i].gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            }
        }

        //Also update which is dead and so which is shown in the queue and updates their offset depending on that too
        PlayerManager.instance.Followers();
        CameraFollow.instance.LookForPlayer();
    }

    public void PositionIncrement(int beforePosition)
    {
        List<HeroClass> newHeroTeam = new List<HeroClass>();
        for (int i = 0; i < heroTeam.Count; i++)
        {
            if (i == beforePosition)
            {
                newHeroTeam.Add(heroTeam[i + 1]);
            }
            else if (i == beforePosition + 1)
            {
                newHeroTeam.Add(heroTeam[i - 1]);
            }
            else newHeroTeam.Add(heroTeam[i]);
        }
        heroTeam = newHeroTeam;

        PauseUIManager.instance.UpdateTeamUI();
        PauseUIManager.instance.UpdateInvHeroesUI();
        UpdateFollowersQueue();
    }

    public void PositionDecrement(int beforePosition)
    {
        List<HeroClass> newHeroTeam = new()
        {
            null,
            null,
            null,
            null
        };
        for (int i = heroTeam.Count - 1; i > -1; i--)
        {
            if (i == beforePosition)
            {
                newHeroTeam[i] = heroTeam[i - 1];
            }
            else if (i == beforePosition - 1)
            {
                newHeroTeam[i] = heroTeam[i + 1];
            }
            else newHeroTeam[i] = heroTeam[i];
        }
        heroTeam = newHeroTeam;

        PauseUIManager.instance.UpdateTeamUI();
        UpdateFollowersQueue();
    }

    //Appelé seulement après un combat, pas lorsque l'allié meurt
    public void UpdateDeathAlliesPosition()
    {
        //Repositionne l'équipe pour que l'allié ou les alliés mort(s) soi(en)t positionné(s) à la fin de l'équipe et ne puisse plus être changé de place tant qu'ils ne sont pas revive.
    }

    public List<HeroClass> GetAliveHeroes()
    {
        List<HeroClass> tempList = new List<HeroClass>();
        foreach (HeroClass hero in heroTeam)
        {
            if(hero.currentHP > 0) { tempList.Add(hero); }
        }
        return tempList;
    }

    public void StoreAttackXP(string heroName, int targetLevel, float L)
    {
        foreach (HeroClass hero in heroTeam)
        {
            if (hero.name == heroName)
            {
                //Calcule le nombre d'xp à donner à l'attaquant selon les dégâts calculés de l'attaque
                //Ca signifie que si le héros effectue une attaque qui termine la cible, ce ne sont pas les PV enlevés
                //par l'attaque qui seront pris en compte, mais bel et bien les dégâts que l'attaque était censée infliger
                //
                // L correspond à la quantité de dégâts calculés

                float xpReceived = (L / 10) * 1.5f * hero.level * targetLevel;
                hero.storedXp += xpReceived;
                Debug.Log("Storing Attack XP for " + heroName + " : " + xpReceived + "xp received for a " + hero.currentLvlCap + "xp lvl cap");
                return;
            }
        }

    }

    public void StoreHealingXP(string heroName, int HR, int targetLevel)
    {
        foreach (HeroClass hero in heroTeam)
        {
            if (hero.name == heroName)
            {
                //Calcule le nombre d'xp à donner au héros utilisant cette capacité lorsqu'elle soigne lui ou un autre héros.
                //
                // HR correspond à la quantité de PV soignés sur la cible

                float xpReceived = (HR / 10) * 1.5f * targetLevel;
                hero.storedXp += xpReceived * (hero.baseHeroXP/100);
                Debug.Log("Storing Healing XP for " + heroName + " : " + xpReceived + "xp received for a " + hero.currentLvlCap + "xp lvl cap");
                return;
            }
        }
    }

    public void StoreOtherSkillXP(string heroName, int targetLevel, int manaUsed)
    {
        foreach (HeroClass hero in heroTeam)
        {
            if (hero.name == heroName)
            {
                // Calcule le nombre d'xp à donner au héro lorsqu'il fait une capacité ni de dégâts, ni de soin.
                // Cela correspond aux capacités Buff, Debuff, à effet etc...
                //
                // La formule pour calculer cette quantité d'xp est : ((manaUsed / 1.5) / 100) * LvlCap
                // où manaUsed correspond à la quantité de mana dépensée pour utilisant cette capacité
                // et où LvlCap correspond à la quantité d'xp à obtenir pour le héros pour passer le niveau actuel
                
                float xpReceived = (manaUsed/10f)*1.5f*targetLevel*hero.level;
                hero.storedXp += xpReceived;
                Debug.Log("Storing Other Skill XP for " + heroName + " : " + xpReceived + "xp received for a " + hero.currentLvlCap + "xp lvl cap");
                return;
            }
        }
    }

    public void GainWinXpAndMana()
    {
        Debug.Log("Gaining XP and Mana");
        //Xp et Mana mis dans la même fonction pour éviter de parcourir deux fois la liste de héros.
        foreach (HeroClass hero in heroTeam)
        {
            //Give mana
            if (hero.maxMana - hero.currentMana <= 30) hero.currentMana = hero.maxMana;
            else hero.currentMana += 30;

            //Ajout de l'xp emmagasinée
            hero.totalXp += hero.storedXp;
            hero.currentXp += hero.storedXp;
            //Réinitialisation de storedXp pour le prochain combat
            hero.storedXp = 0;

            while (hero.currentXp >= hero.currentLvlCap)
            {
                
                //Augmentation de niveau
                hero.level++;
                hero.currentXp -= hero.currentLvlCap;
                hero.maxHP += hero.healthPerLevel;
                hero.currentHP += hero.healthPerLevel;
                hero.currentLvlCap = 0.5f * Mathf.Pow(hero.level * 2, 3) + 6;
                Debug.Log(hero.name + " : lvl" + hero.level + " atteint. Nouveau cap d'xp : " +  hero.currentLvlCap + "xp");
            }
        }
    }
}
