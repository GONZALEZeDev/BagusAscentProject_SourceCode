using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("heroe"))
        {
            TeamClass.instance.moveKeybinds.SetActive(false);
        }
    }
}
