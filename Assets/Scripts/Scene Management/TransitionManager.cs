using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static TransitionManager;

public class TransitionManager : MonoBehaviour
{
    static public TransitionManager instance;

    public Animator anim;
    public SpriteRenderer sp1;
    public Sprite startSprite;

    public enum TransitionType
    {
        EnterMobBattle,
        EnterBossBattle,
        EnterCave1,
        EnterCave2,
        ExitCave1,
        ExitCave2,
        inBlackAnim,
        outBlackAnim,
        startBlackAnim
    }

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
        sp1.sprite = startSprite;
    }

    private void Start()
    {
        StartCoroutine(PlayTransition(TransitionType.startBlackAnim));
    }
    public IEnumerator PlayTransition(TransitionType vid)
    {
        yield return new WaitForEndOfFrame();

        switch (vid)
        {
            case TransitionType.EnterMobBattle:
                //Jouer l'animation d'entrée en combat simple
                anim.SetTrigger("EnterMobBattle");
                break;
            case TransitionType.EnterBossBattle:
                //Jouer l'animation d'entrée en combat de boss
                anim.SetTrigger("EnterBossBattle");
                break;
            case TransitionType.EnterCave1:
                //Jouer l'animation d'entrée 1 dans la cave (World 3)
                //(lorsqu'on rentre dedans mais que l'on est encore à l'éxtérieur)
                anim.SetBool("EnterCave", true);
                break;
            case TransitionType.EnterCave2:
                //Jouer l'animation d'entrée 2 dans la cave (World 3)
                //(lorsqu'on rentre dedans mais que l'on est arrivé à l'intérieur)
                anim.SetBool("EnterCave", false);
                break;
            case TransitionType.ExitCave1:
                //Jouer l'animation de sortie 1 de la cave (World 3) jusqu'au pic enneigé (World 4)
                //(lorsqu'on rentre dedans mais que l'on est encore dans la grotte)
                anim.SetBool("ExitCave", true);
                break;
            case TransitionType.ExitCave2:
                //Jouer l'animation de sortie 2 de la cave (World 3) jusqu'au pic enneigé (World 3)
                //(lorsqu'on rentre dedans mais que l'on est arrivé à l'éxtérieur)
                anim.SetBool("ExitCave",false);
                break;
            case TransitionType.inBlackAnim:
                //Jouer une simple animation de transition où l'écran devient noirs
                anim.SetBool("BlackAnim", true);
                break;
            case TransitionType.outBlackAnim:
                //Jouer une simple animation de transition où l'écran passe de noir à la normal
                anim.SetBool("BlackAnim", false);
                break;
            case TransitionType.startBlackAnim:
                anim.SetTrigger("StartAnim");
                break;
        }
    }

    public void ResetTrigger(string name)
    {
        anim.ResetTrigger(name);
    }

}


