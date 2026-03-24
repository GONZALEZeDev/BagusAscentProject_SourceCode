using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DivineStatue : MonoBehaviour
{

    bool playerInReach = false;
    public GameObject selectEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.CompareTag("heroe"))
            {
                selectEffect.SetActive(true);
                playerInReach = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("heroe"))
        {
            selectEffect.SetActive(false);
            playerInReach = false;
        }
    }
    private void Start()
    {
        selectEffect.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && playerInReach)
        {
            foreach(HeroClass hero in TeamClass.instance.heroTeam)
            {
                SfxManager.instance.PlayOtherSfx(1);
                hero.currentHP = hero.maxHP;
                hero.currentMana = hero.maxMana;
            }
            SaveManager.instance.SaveData();
        }
    }
}
