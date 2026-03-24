using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    static public WorldManager instance;
    public ChestScript[] chests;

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

    private void Start()
    {
        //Changed chests states based on those in WorldsManager
        for (int i = 0; i < chests.Length; i++)
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 3:
                    //World 1
                    if (WorldsManager.instance.world1ChestsOpened[i])
                    {
                        chests[i].animator.Play("OpenChestAnim2", -1, 1);
                        chests[i].animator.speed = 0; // Arrête l'animation
                        chests[i].selectEffect.SetActive(false);
                        chests[i].GetComponent<BoxCollider2D>().enabled = false;
                        chests[i].enabled= false;

                    }
                    else
                    {
                        chests[i].GetComponent<BoxCollider2D>().enabled = true;
                        chests[i].enabled = true;
                    }
                    break;
                case 5:
                    //World 2
                    if (WorldsManager.instance.world2ChestsOpened[i])
                    {
                        chests[i].animator.Play("OpenChestAnim2", -1, 1);
                        chests[i].animator.speed = 0; // Arrête l'animation
                        chests[i].selectEffect.SetActive(false);
                        chests[i].GetComponent<BoxCollider2D>().enabled = false;
                        chests[i].enabled = false;

                    }
                    else
                    {
                        chests[i].GetComponent<BoxCollider2D>().enabled = true;
                        chests[i].enabled = true;
                    }
                    break;
                case 7:
                    //World 3
                    if (WorldsManager.instance.world3ChestsOpened[i])
                    {
                        chests[i].animator.Play("OpenChestAnim2", -1, 1);
                        chests[i].animator.speed = 0; // Arrête l'animation
                        chests[i].selectEffect.SetActive(false);
                        chests[i].GetComponent<BoxCollider2D>().enabled = false;
                        chests[i].enabled = false;

                    }
                    else
                    {
                        chests[i].GetComponent<BoxCollider2D>().enabled = true;
                        chests[i].enabled = true;
                    }
                    break;
                case 9:
                    //World 4
                    if (WorldsManager.instance.world4ChestsOpened[i])
                    {
                        chests[i].animator.Play("OpenChestAnim2", -1, 1);
                        chests[i].animator.speed = 0; // Arrête l'animation
                        chests[i].selectEffect.SetActive(false);
                        chests[i].GetComponent<BoxCollider2D>().enabled = false;
                        chests[i].enabled = false;

                    }
                    else
                    {
                        chests[i].GetComponent<BoxCollider2D>().enabled = true;
                        chests[i].enabled = true;
                    }
                    break;
            }
            
        }
    }
    public void SaveState()
    {
        //Change chests states based on those in WorldsManager
        for (int i = 0; i < chests.Length; i++)
        {
            //On réactive le script pour accéder à ses variables
            chests[i].enabled = true;
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 3:
                    //World 1
                    //if(WorldsManager.instance.world1ChestsOpened.Length > i)
                    WorldsManager.instance.world1ChestsOpened[i] = chests[i].isOpened;
                    break;
                case 5:
                    //World 2
                    WorldsManager.instance.world2ChestsOpened[i] = chests[i].isOpened;
                    break;
                case 7:
                    //World 3
                    WorldsManager.instance.world3ChestsOpened[i] = chests[i].isOpened;
                    break;
                case 9:
                    //World 4
                    WorldsManager.instance.world4ChestsOpened[i] = chests[i].isOpened;
                    break;
            }
            if (chests[i].isOpened) chests[i].enabled = false;
        }
    }
}
