using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    static public BattleManager instance;

    public bool isFightFinished = false;

    //Ennemies Prefabs throughout the world
    public GameObject[] world1Ennemies;
    public GameObject[] world2Ennemies;
    public GameObject[] world3Ennemies;
    public GameObject[] world4Ennemies;

    public List<Ennemies> theEnnemies;
    List<Vector3> ennemiesPositions = new List<Vector3>() {
        new Vector3(3.71f, -0.23f,0),
        new Vector3(5.93f, -1.07f, 0),
        new Vector3(5.93f, 0.48f, 0),
        new Vector3(7.82f, -0.27f, 0)
    };

    List<Vector3> heroesPositions = new List<Vector3>() {
        new Vector3(-3.35f, -0.25f, 0),
        new Vector3(-5.15f, -1.19f, 0),
        new Vector3(-4.78f, 0.71f, 0),
        new Vector3(-6.53f, -0.11f, 0)
    };


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

    public void PickEnnemies()
    {
        //Choisis 1 à 4 ennemis parmis ceux apparaissant dans le monde acutel
        System.Random rnd = new System.Random();
        int nbrEnnemies;
        if(SceneManager.GetActiveScene().buildIndex == 3) nbrEnnemies = rnd.Next(1, 4);
        else nbrEnnemies = rnd.Next(1, 5);
        Debug.Log(nbrEnnemies);

        for (int i = 0; i < nbrEnnemies; i++)
        {
            //Prendre un ennemi au hasard dans le tableau listant tous les ennemis dispos pour ce monde (tous les ennemis ont les mêmes probabilités d'apparaitre en combat pour le moment)
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 3:
                    //World 1
                    theEnnemies.Add(Instantiate(world1Ennemies[rnd.Next(0, world1Ennemies.Length)], ennemiesPositions[i], new Quaternion()).GetComponent<Ennemies>());
                    theEnnemies[i].level = rnd.Next(1, 6);
                    break;
                case 5:
                    //World 2
                    theEnnemies.Add(Instantiate(world2Ennemies[rnd.Next(0, world2Ennemies.Length)], ennemiesPositions[i], new Quaternion()).GetComponent<Ennemies>());
                    theEnnemies[i].level = rnd.Next(6, 11);
                    break;
                case 7:
                    //World 3
                    theEnnemies.Add(Instantiate(world3Ennemies[rnd.Next(0, world3Ennemies.Length)], ennemiesPositions[i], new Quaternion()).GetComponent<Ennemies>());
                    theEnnemies[i].level = rnd.Next(11, 16);
                    break;
                default:
                    //World 4
                    theEnnemies.Add(Instantiate(world4Ennemies[rnd.Next(0, world4Ennemies.Length)], ennemiesPositions[i], new Quaternion()).GetComponent<Ennemies>());
                    theEnnemies[i].level = rnd.Next(16, 21);
                    break;
            }
            theEnnemies[i].InitializeBasicStats();
            if (i < 2) theEnnemies[i].gameObject.GetComponent<SpriteRenderer>().sortingOrder = i;
            else theEnnemies[i].gameObject.GetComponent<SpriteRenderer>().sortingOrder = i-2;
        }
        UIFight.instance.ennemie = theEnnemies[0];
    }

    public void ReadyBattle()
    {
        SceneChanger.instance.SetBattleBG();
        //players pos
        for (int i = 0; i < TeamClass.instance.heroTeam.Count; i++)
        {
            switch (i)
            {
                case 0:
                    //Hero pos 0
                    TeamClass.instance.heroTeam[i].GetComponent<PlayerController>().enabled = false;
                    TeamClass.instance.heroTeam[i].GetComponent<EncounterController>().enabled = false;
                    TeamClass.instance.heroTeam[i].GetComponent<CapsuleCollider2D>().enabled = false;
                    break;
                case 1:
                    //Hero pos 1
                    TeamClass.instance.heroTeam[i].GetComponent<playerFollower>().enabled = false;
                    break;
                case 2:
                    //Hero pos 2
                    TeamClass.instance.heroTeam[i].GetComponent<playerFollower>().enabled = false;
                    break;
                default:
                    //Hero pos 3
                    TeamClass.instance.heroTeam[i].GetComponent<playerFollower>().enabled = false;
                    break;
            }
            TeamClass.instance.heroTeam[i].beforeFightPos = TeamClass.instance.heroTeam[i].transform.position;
            TeamClass.instance.heroTeam[i].sp.sortingLayerName = "UI";
            TeamClass.instance.heroTeam[i].transform.position = heroesPositions[i];
            TeamClass.instance.heroTeam[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            TeamClass.instance.heroTeam[i].animator.SetBool("xSpeed", true);
            TeamClass.instance.heroTeam[i].sp.flipX = true;
            TeamClass.instance.heroTeam[i].animator.enabled = false;
            TeamClass.instance.heroTeam[i].sp.sprite = TeamClass.instance.heroTeam[i].sideSprite;
            TeamClass.instance.heroTeam[i].shadow.sortingLayerName = "UI";
        }

        //Chooses the ennemies in the fight
        PickEnnemies();

        //ennemies pos
        for (int i = 0; i < theEnnemies.Count; i++)
        {
            Vector3 vect = new Vector3();
            switch (i)
            {
                case 0:
                    //ennemi pos 0
                    vect = new Vector3(3.71f, -0.23f, 0.305f);
                    break;
                case 1:
                    //ennemi pos 1
                    vect = new Vector3(5.93f, -1.07f, 0.305f);
                    break;
                case 2:
                    //ennemi pos 2
                    vect = new Vector3(5.93f, 0.48f, 0.305f);
                    break;
                default:
                    //ennemi pos 3
                    vect = new Vector3(7.82f, -0.27f, 0.305f);
                    break;
            }
            theEnnemies[i].transform.position = vect;
        }
    }



    public void HandleVictory(string announcementText)
    {
        StartCoroutine(WaitingBeforeFinish(announcementText, false));
        Debug.Log("The heroes win !");
        // Autres actions éventuelles à exécuter immédiatement après le déclenchement de la coroutine
    }

    public void HandleDefeat(string announcementText)
    {
        StartCoroutine(WaitingBeforeFinish(announcementText, true));
        Debug.Log("The heroes lost !");
        // Autres actions éventuelles à exécuter immédiatement après le déclenchement de la coroutine
    }

    public IEnumerator WaitingBeforeFinish(string announcementText, bool GameOver)
    {
        // Attendre pendant 3 secondes
        yield return new WaitForSeconds(3f);

        // Actions à effectuer après l'attente
        // Dans ce cas, vous pouvez déclencher l'annonce
        StartCoroutine(ButtonUIFight.instance.DisplayAnnouncement(announcementText));
        if(!GameOver) SfxManager.instance.PlayOtherSfx(2);

        // Attendre 5 secondes après l'annonce
        yield return new WaitForSeconds(5f);
        StartCoroutine(SceneChanger.instance.UnloadBattleScene(false, GameOver));
    }

    public bool AreAllEnemiesDefeated()
    {
        // Vérifie si toutes les ennemies ont une currentHP de 0
        foreach (var enemy in theEnnemies)
        {
            if (enemy.currentHP > 0)
            {
                return false; // Au moins un ennemi a encore des points de vie
            }
        }
        return true; // Tous les ennemis ont une currentHP de 0
    }

    public bool AreAllHeroesDefeated()
    {
        // Vérifie si tous les héros ont une currentHP de 0
        foreach (var hero in TeamClass.instance.heroTeam)
        {
            if (hero.currentHP > 0)
            {
                return false; // Au moins un héros a encore des points de vie
            }
        }
        return true; // Tous les héros ont une currentHP de 0
    }

}
