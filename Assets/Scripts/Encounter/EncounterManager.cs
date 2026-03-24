using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager instance;
    public GameObject player;
    public Vector3 lastPosition;
    public bool isInSafeZone;
    public float totalDistance;
    public float stepLength = 1f;
    public float stepCount = 0f;
    public AnimationCurve probabilityCurve;
    public float monsterSpawnProbability;

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
            DontDestroyOnLoad(this.gameObject);
        }

    }

    private void Start()
    {
        player = TeamClass.instance.heroesGO[0];
        lastPosition = player.transform.position;
    }
}
