using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class BattleController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (BattleTriggerManager.instance.inBattle && UIFight.instance.startTimer < 0)
        {
            if (UIFight.instance.activeCharacterObject.CompareTag("ennemie"))
            {
                UIFight.instance.activeCharacterObject.GetComponent<Ennemies>().isHisTurn = true;
            }

            // Désactiver les héros dont la santé est à zéro
            foreach (var chara in OrderFight.instance.charactersOrder)
            {
                if (chara.currentHP <= 0)
                {
                    chara.currentHP = 0;
                    if (chara.gameObject.CompareTag("ennemie") && chara == UIFight.instance.ennemie)
                    {
                        foreach (var enemy in BattleManager.instance.theEnnemies)
                        {
                            if (enemy.currentHP > 0)
                            {
                                Debug.Log("Delegating turn to " + enemy.name);
                                UIFight.instance.ennemie = enemy;
                            }
                        }
                    }
                    chara.gameObject.SetActive(false);
                }
            }

            // Vérifier si tous les ennemis ont une currentHP de 0
            if (BattleManager.instance.AreAllEnemiesDefeated() && !BattleManager.instance.isFightFinished)
            {
                BattleManager.instance.isFightFinished = true;
                BattleManager.instance.HandleVictory("VICTORY !");
                return;
            }

            // Vérifier si tous les héros ont une currentHP de 0
            if (BattleManager.instance.AreAllHeroesDefeated()&& !BattleManager.instance.isFightFinished)
            {
                BattleManager.instance.isFightFinished = true;
                BattleManager.instance.HandleDefeat("DEFEAT !");
                return;
            }
        }
    }
        
        
}
