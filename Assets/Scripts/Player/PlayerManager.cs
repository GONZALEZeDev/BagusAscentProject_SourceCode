using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerManager
{
    private static PlayerManager Instance;

    public static PlayerManager instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = new PlayerManager();
            }
            return Instance;
        }
    }

    public Rigidbody2D playerRigidbody;
    public int followersNbr = 3;

    //variable contenant le dernier mouvement du joueur pour l'enregistrer pour ses alliées
    public Vector3 lastMovement;

    public GameObject player;

    public UnityEngine.Vector2 posWorld1 = new Vector2(-11.22f, -20.68f);
    public UnityEngine.Vector2 posEndWorld1 = new Vector2(-23.07f, 61.12f);

    public UnityEngine.Vector2 posBtwn12 = new Vector2(-11.82904f, -18.91f);
    public UnityEngine.Vector2 posEndBtwn12 = new Vector2(-11.82904f, -5.36f);

    public UnityEngine.Vector2 posWorld2 = new Vector2(-11.28f, -15.04f);
    public UnityEngine.Vector2 posEndWorld2 = new Vector2(17.15f, 83.23f);

    public UnityEngine.Vector2 posBtwn23 = new Vector2(-12.01f, -18.2f);
    public UnityEngine.Vector2 posEndBtwn23 = new Vector2(-13.08f, -6.3f);

    public UnityEngine.Vector2 posWorld3 = new Vector2(-13.04f, -18.61f);
    public UnityEngine.Vector2 posEndWorld3 = new Vector2(55.9f, 61.68f);

    public UnityEngine.Vector2 posBtwn34 = new Vector2(-12.45f, -18.63f);
    public UnityEngine.Vector2 posEndBtwn34 = new Vector2(-12.07f, -5.27f);

    public UnityEngine.Vector2 posWorld4 = new Vector2(-12.4f, -16.99f);


    public float yMaxWorld1 = 67f;

    public float yMinBtwn12 = -23f;
    public float yMaxBtwn12 = -1f;

    public float yMinWorld2 = -23f;
    public float yMaxWorld2 = 90f;

    public float yMinBtwn23 = -23f;
    public float yMaxBtwn23 = -1f;

    public float yMinWorld3 = -24.3f;
    public float yMaxWorld3 = 69.05f;
    public float xMaxWorld3 = 53.37f;

    public float yMinBtwn34 = -23f;
    public float yMaxBtwn34 = 0f;

    public float yMinWorld4 = -20f;


    public PlayerManager()
    {
        Followers();
    }

    

    public void Followers()
    {
        player = TeamClass.instance.GetAliveHeroes()[0].gameObject;
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        TeamClass.instance.leaderTrail = new Queue<Vector3>();
        List<HeroClass> listAliveHeroes = new List<HeroClass>();

        foreach (HeroClass hero in TeamClass.instance.heroTeam)
        {
            if (hero.currentHP <= 0)
            {
                hero.gameObject.SetActive(false);
            }
            else
            {
                listAliveHeroes.Add(hero);
            }
        }

        for (int i = 1; i < listAliveHeroes.Count; i++)
        {

            listAliveHeroes[i].gameObject.GetComponent<playerFollower>().Offset = (i) * listAliveHeroes[i].gameObject.GetComponent<playerFollower>().DefOffset;
        }
    }
}
