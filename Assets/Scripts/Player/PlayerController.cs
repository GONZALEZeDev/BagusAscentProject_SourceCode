using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    int placement;
    void FixedUpdate()
    {

        //Fais en sorte que les alliés soient affichés devant/derrière le leader selon leurs positions;
        if (gameObject.activeSelf && !BattleTriggerManager.instance.inBattle)
        {
            placement = 0;
            foreach (HeroClass hero in TeamClass.instance.heroTeam)
                if (hero != GetComponent<HeroClass>() && hero.gameObject.transform.position.y > transform.position.y) placement++;
            GetComponent<HeroClass>().sp.sortingOrder = placement + 4;
        }

        //Player Movement

        // Input for horizontal and vertical movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        GetComponent<HeroClass>().animator.SetFloat("ySpeed", moveY);


        // Checking for diagonal movement
        if (moveX != 0)
        {
            moveY = 0;
            GetComponent<HeroClass>().animator.SetBool("xSpeed", true);
        }
        else
        {
            moveX = 0;
            GetComponent<HeroClass>().animator.SetBool("xSpeed", false);
        }

        if (moveX > 0)
        {
            GetComponent<HeroClass>().sp.flipX = true;
        }
        if (moveX < 0)
        {
            GetComponent<HeroClass>().sp.flipX = false;
        }

        Vector2 movement = new Vector2(moveX, moveY);

        PlayerManager.instance.playerRigidbody.velocity = gameObject.GetComponent<HeroClass>().walkSpeed * movement;


        //Following stuff
        if (transform.position != PlayerManager.instance.lastMovement) {
            PlayerManager.instance.lastMovement = transform.position;
            TeamClass.instance.leaderTrail.Enqueue(transform.position);
        }
        
        if(transform.position.y >= PlayerManager.instance.yMaxWorld1 && SceneManager.GetActiveScene().buildIndex == 3 && !EncounterManager.instance.isInSafeZone)
        {
            //Change to between 1 and 2
            EncounterManager.instance.isInSafeZone = true;
            StartCoroutine(SceneChanger.instance.ChangeScene(4, false));
        }
        if (transform.position.y <= PlayerManager.instance.yMinBtwn12 && SceneManager.GetActiveScene().buildIndex == 4 && EncounterManager.instance.isInSafeZone)
        {
            //Change to world 1
            StartCoroutine(SceneChanger.instance.ChangeScene(3, false));
        }
        if (transform.position.y >= PlayerManager.instance.yMaxBtwn12 && SceneManager.GetActiveScene().buildIndex == 4 && EncounterManager.instance.isInSafeZone)
        {
            //Change to world 2
            StartCoroutine(SceneChanger.instance.ChangeScene(5, false));
        }
        if (transform.position.y <= PlayerManager.instance.yMinWorld2 && SceneManager.GetActiveScene().buildIndex == 5 && !EncounterManager.instance.isInSafeZone)
        {
            //Change to between 1 and 2
            EncounterManager.instance.isInSafeZone = true;
            StartCoroutine(SceneChanger.instance.ChangeScene(4, false));
        }
        if (transform.position.y >= PlayerManager.instance.yMaxWorld2 && SceneManager.GetActiveScene().buildIndex == 5 && !EncounterManager.instance.isInSafeZone)
        {
            //Change to between 2 and 3
            EncounterManager.instance.isInSafeZone = true;
            StartCoroutine(SceneChanger.instance.ChangeScene(6, false));
        }
        if (transform.position.y <= PlayerManager.instance.yMinBtwn23 && SceneManager.GetActiveScene().buildIndex == 6 && EncounterManager.instance.isInSafeZone)
        {
            //Change to world 2
            StartCoroutine(SceneChanger.instance.ChangeScene(5, false));
        }
        if (transform.position.y >= PlayerManager.instance.yMaxBtwn23 && SceneManager.GetActiveScene().buildIndex == 6 && EncounterManager.instance.isInSafeZone)
        {
            //Change to world 3
            StartCoroutine(SceneChanger.instance.ChangeScene(7, false));
        }
        if (transform.position.y <= PlayerManager.instance.yMinWorld3 && SceneManager.GetActiveScene().buildIndex == 7 && !EncounterManager.instance.isInSafeZone)
        {
            //Change to between 2 and 3
            EncounterManager.instance.isInSafeZone = true;
            StartCoroutine(SceneChanger.instance.ChangeScene(6, false));
        }
        if (transform.position.y >= PlayerManager.instance.yMaxWorld3 && transform.position.x >= PlayerManager.instance.xMaxWorld3 && SceneManager.GetActiveScene().buildIndex == 7 && !EncounterManager.instance.isInSafeZone)
        {
            //Change to between 3 and 4
            EncounterManager.instance.isInSafeZone = true;
            StartCoroutine(SceneChanger.instance.ChangeScene(8, false));
        }
        if (transform.position.y <= PlayerManager.instance.yMinBtwn34 && SceneManager.GetActiveScene().buildIndex == 8 && EncounterManager.instance.isInSafeZone)
        {
            //Change to world 3
            StartCoroutine(SceneChanger.instance.ChangeScene(7, false));
        }
        if (transform.position.y >= PlayerManager.instance.yMaxBtwn34 && SceneManager.GetActiveScene().buildIndex == 8 && EncounterManager.instance.isInSafeZone)
        {
            //Change to world 4
            StartCoroutine(SceneChanger.instance.ChangeScene(9, false));
        }
        if (transform.position.y <= PlayerManager.instance.yMinWorld4 && SceneManager.GetActiveScene().buildIndex == 9 && !EncounterManager.instance.isInSafeZone)
        {
            //Change to between 3 and 4
            EncounterManager.instance.isInSafeZone = true;
            StartCoroutine(SceneChanger.instance.ChangeScene(8, false));
        }
    }
}
