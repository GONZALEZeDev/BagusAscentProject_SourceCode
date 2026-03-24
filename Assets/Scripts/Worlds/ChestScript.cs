using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestScript : MonoBehaviour
{
    public int itemID;
    public bool isOpened = false;
    bool playerInReach = false;
    public Animator animator;
    public GameObject selectEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("heroe") && !isOpened)
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
        animator.SetBool("isOpened", isOpened);
        selectEffect.SetActive(false);
    }

    private void Update()
    {
        //Z = W pour nous en Azerty
        if (Input.GetKeyDown(KeyCode.Z) && playerInReach && !isOpened)
        {
            OpenChest();
        }
    }

    public void OpenChest()
    {
        isOpened = true;
        selectEffect.SetActive(false);
        SfxManager.instance.PlayOtherSfx(0);
        animator.SetBool("isOpened", isOpened);
        Debug.Log("Giving chest item...");
        PlayerInventory.instance.AddItem(itemID);

        //Désactive le script pour ne pas laisser le Update() tourner
        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
    }
}
