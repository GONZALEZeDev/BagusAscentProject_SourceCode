using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;
using Object = System.Object;

public class playerFollower : MonoBehaviour
{
    public int DefOffset { get; private set; } = 15;
    public int Offset;
    int placement;

    private void Start()
    {
        Offset = DefOffset;
    }

    void FixedUpdate()
    {
        if(gameObject.activeSelf && !BattleTriggerManager.instance.inBattle)
        {
            placement = 0;
            foreach (HeroClass hero in TeamClass.instance.heroTeam) 
                if (hero != GetComponent<HeroClass>() && hero.gameObject.transform.position.y > transform.position.y) placement++;
            GetComponent<HeroClass>().sp.sortingOrder = placement+4;
        }
        

        if (TeamClass.instance.leaderTrail.Count > Offset)
        {
            Vector3 targetPosition = TeamClass.instance.leaderTrail.ToArray()[TeamClass.instance.leaderTrail.Count - 1 - Offset];
            Vector3 moveDirection = (targetPosition - transform.position);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, GetComponent<HeroClass>().walkSpeed * Time.deltaTime);

            GetComponent<HeroClass>().animator.SetFloat("ySpeed", moveDirection.y);

            if (moveDirection.x != 0)
            {
                GetComponent<HeroClass>().animator.SetBool("xSpeed", true);
            }
            else
            {
                GetComponent<HeroClass>().animator.SetBool("xSpeed", false);
            }

            if (moveDirection.x > 0)
            {
                GetComponent<HeroClass>().sp.flipX = true;
            }
            if (moveDirection.x < 0)
            {
                GetComponent<HeroClass>().sp.flipX = false;
            }

        }
    }
}