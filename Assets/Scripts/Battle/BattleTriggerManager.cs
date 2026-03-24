using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerManager : MonoBehaviour
{
    public static BattleTriggerManager instance;

    public bool inBattle = false;
    public bool isBossBattle = false;

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
            DontDestroyOnLoad(gameObject);
        }
    }
}
