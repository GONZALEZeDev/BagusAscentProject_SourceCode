using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterController : MonoBehaviour
{
    void Update()
    {
        float distanceThisFrame = Vector3.Distance(EncounterManager.instance.player.transform.position, EncounterManager.instance.lastPosition);
        EncounterManager.instance.totalDistance += distanceThisFrame;
        EncounterManager.instance.lastPosition = EncounterManager.instance.player.transform.position;

        if (distanceThisFrame > 0.0f)
        {
            // Utilisez la courbe pour obtenir la nouvelle probabilité en fonction de la distance totale parcourue
            EncounterManager.instance.monsterSpawnProbability = EncounterManager.instance.probabilityCurve.Evaluate((EncounterManager.instance.stepCount) / 200);

            // Si la distance totale parcourue dépasse 1 unité, réinitialise la distance totale et la probabilité
            if (EncounterManager.instance.totalDistance >= EncounterManager.instance.stepLength)
            {
                EncounterManager.instance.stepCount++;
                if (EncounterManager.instance.stepCount % 2 == 0)
                {
                    switch (SceneManager.GetActiveScene().buildIndex)
                    {
                        case 7:
                            //Inside the cave
                            SfxManager.instance.PlayPlayerWalkSfx(1);
                            break;
                        case 8:
                            //Between cave and summit
                            SfxManager.instance.PlayPlayerWalkSfx(1);
                            break;
                        case 9:
                            //In the snowy summit
                            SfxManager.instance.PlayPlayerWalkSfx(2);
                            break;
                        default:
                            //In one of the two forests
                            SfxManager.instance.PlayPlayerWalkSfx(0);
                            break;
                    }
                }
                EncounterManager.instance.totalDistance = 0.0f;

                // Génère un nombre aléatoire entre 0 et 1
                float randomValue = Random.value;

                if (randomValue <= EncounterManager.instance.monsterSpawnProbability && !BattleTriggerManager.instance.inBattle && SceneManager.GetActiveScene().buildIndex > 1 && !PauseManager.instance.isPaused && EncounterManager.instance.isInSafeZone == false)
                {
                    Debug.Log("Starting ennemy fight...");
                    StartCoroutine(SceneChanger.instance.ChangeScene(2, false, false));
                    EncounterManager.instance.stepCount = 0;
                }
            }

        }
    }
}
