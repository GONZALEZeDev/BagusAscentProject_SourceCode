using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    public GameObject sceneGO;

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

    public IEnumerator ChangeScene(int sceneIndex, bool isWithContinue, bool isBossBattle = false)
    {
        if (sceneIndex == 2)//Scene de combat
        {

            PauseManager.instance.canTogglePause = false;
            //Mettre la scene actuelle en pause avec Time.timeScale = 0;
            Time.timeScale = 0f;
            BattleTriggerManager.instance.isBossBattle = isBossBattle;
            if (isBossBattle)
            {
                //Lancer l'animation d'entrée en combat du boss (l'animation doit être en Unscaled Time pour ne pas être affecté par la pause)
                StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.EnterBossBattle));
            }
            else
            {
                //Lancer l'animation d'entrée en combat (l'animation doit être en Unscaled Time pour ne pas être affecté par la pause)
                StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.EnterMobBattle));
                StartCoroutine(SettingsManager.instance.ChangeMusic(2));
            }

            //Attendre suffisamment pour que l'écran soit noir (même temps pour les deux anim normallement)
            yield return new WaitForSecondsRealtime(1.12f);

            CameraFollow.instance.StopFollowPlayer();

            //Désactivez les gameobjetcs  de l'autre scene pour éviter de laisser des trucs chargés pour rien
            var sceneGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var item in sceneGameObjects)
            {
                if (item.CompareTag("SceneGO"))
                {
                    sceneGO = item;
                    break;
                }
            }
            sceneGO.SetActive(false);

            //Enelever la pause
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            BattleTriggerManager.instance.inBattle = true;

            yield return new WaitForEndOfFrame();
        }
        else
        {
            PauseManager.instance.canTogglePause = false;
            int beforeIndex = SceneManager.GetActiveScene().buildIndex;

            //Mettre la scene actuelle en pause avec Time.timeScale = 0;
            Time.timeScale = 0;

            //Sauvegarde les états des coffres du monde
            if (beforeIndex == 3 || beforeIndex == 5 || beforeIndex == 7 || beforeIndex == 9) WorldManager.instance.SaveState();

            //Lancer l'animation de transition d'entrée (l'animation doit être en Unscaled Time pour ne pas être affecté par la pause
            switch (sceneIndex)
            {
                case 7:
                    //To World 3 - Cave
                    StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.EnterCave1));
                    break;
                case 8:
                    //To Btwn 3-4
                    StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.EnterCave1));
                    break;
                case 9:
                    //To World 4 - Pic enneigé
                    StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.ExitCave1));
                    break;
                default:
                    //Tout autre monde
                    StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.inBlackAnim));
                    break;
            }

            //Attendre que l'animation se finisse (qu'elle passe à l'écran noir)

            yield return new WaitForSecondsRealtime(0.5f);

            //Remettre le temps en normal
            Time.timeScale = 1;

            GameOverUI.instance.GetComponent<Canvas>().enabled = false;

            if (sceneIndex != 1)
            {
                foreach (HeroClass hero in TeamClass.instance.heroTeam)
                {
                    hero.gameObject.SetActive(false);
                    if (!isWithContinue)
                    {
                        //Switch pour les faire apparaitre à un endroit prédéfini sur le prochain monde
                        switch (sceneIndex)
                        {
                            case 3:
                                //To world 1
                                if (beforeIndex == 4) hero.transform.position = PlayerManager.instance.posEndWorld1;
                                else hero.transform.position = PlayerManager.instance.posWorld1;
                                break;
                            case 4:
                                //To btwn 1-2
                                if (beforeIndex == 5) hero.transform.position = PlayerManager.instance.posEndBtwn12;
                                else hero.transform.position = PlayerManager.instance.posBtwn12;
                                break;
                            case 5:
                                //To World 2
                                if (beforeIndex == 6) hero.transform.position = PlayerManager.instance.posEndWorld2;
                                else hero.transform.position = PlayerManager.instance.posWorld2;
                                break;
                            case 6:
                                //To btwn 2-3
                                if (beforeIndex == 7) hero.transform.position = PlayerManager.instance.posEndBtwn23;
                                else hero.transform.position = PlayerManager.instance.posBtwn23;
                                break;
                            case 7:
                                //To world 3
                                if (beforeIndex == 8) hero.transform.position = PlayerManager.instance.posEndWorld3;
                                else hero.transform.position = PlayerManager.instance.posWorld3;
                                break;
                            case 8:
                                //To btwn 3-4
                                if (beforeIndex == 9) hero.transform.position = PlayerManager.instance.posEndBtwn34;
                                else hero.transform.position = PlayerManager.instance.posBtwn34;
                                break;
                            case 9:
                                //to world 4
                                hero.transform.position = PlayerManager.instance.posWorld4;
                                break;
                        }
                    }
                    hero.gameObject.SetActive(true);
                }
                TeamClass.instance.UpdateFollowersQueue();
            }
            

            EncounterManager.instance.totalDistance = 0;
            EncounterManager.instance.stepCount = 0;
            EncounterManager.instance.monsterSpawnProbability = 0.05f;

            SceneManager.LoadScene(sceneIndex);

            //yield return new WaitForEndOfFrame();

            Time.timeScale = 0;

            StartCoroutine(SettingsManager.instance.ChangeMusic(1));

            //Lancer l'animation de transition de sortie
            switch (sceneIndex)
            {
                case 7:
                    //To World 3 - Cave
                    StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.EnterCave2));
                    break;
                case 8:
                    //To Btwn 3-4
                    StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.EnterCave2));
                    break;
                case 9:
                    //To World 4 - Pic enneigé
                    StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.ExitCave2));
                    break;
                default:
                    //Tout autre monde
                    StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.outBlackAnim));
                    break;
            }

            BattleTriggerManager.instance.inBattle = false;
            PauseManager.instance.canTogglePause = true;
            Time.timeScale = 1;
        }
    }

    //Utilisé pour décharger la scene de combat
    public IEnumerator UnloadBattleScene(bool isAFlee, bool GameOver, bool bossBattle = false)
    {
        //Mettre la scene en pause
        Time.timeScale = 0;

        //Lancer l'animation de sortie en combat 1 
        StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.inBlackAnim));

        //Attendre pour que l'écran soit noir
        yield return new WaitForSecondsRealtime(0.5f);

        foreach (var ennemy in BattleManager.instance.theEnnemies) Destroy(ennemy.gameObject);

        yield return new WaitForEndOfFrame();

        SceneManager.UnloadSceneAsync(2);

        Time.timeScale = 1;
        sceneGO.SetActive(true);
        if (GameOver)
        {
            PauseManager.instance.canTogglePause = false;

            GameOverUI.instance.GetComponent<Canvas>().enabled = true;

            StartCoroutine(SettingsManager.instance.ChangeMusic(999));

            //Lancer l'animation de sortie en combat 2
            StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.outBlackAnim));

            //lancer musique game over

            //Réactivez et repositionner les héros dans la scene de vue Top Down
            for (int i = 0; i < TeamClass.instance.heroTeam.Count; i++)
            {

                TeamClass.instance.heroTeam[i].sp.sortingLayerName = "Default";
                TeamClass.instance.heroTeam[i].transform.position = TeamClass.instance.heroTeam[i].beforeFightPos;
                TeamClass.instance.heroTeam[i].animator.enabled = true;
                TeamClass.instance.heroTeam[i].shadow.sortingLayerName = "Default";
                TeamClass.instance.heroTeam[i].sp.material = TeamClass.instance.heroTeam[i].defaultMaterial;
            }

            PlayerInventory.instance.inventoryItems.Clear();
            TeamClass.instance.heroTeam = new List<HeroClass>();
            Debug.Log("continue from GO Screen : " + TeamClass.instance.heroTeam.Count);

            foreach (GameObject go in TeamClass.instance.heroesGO)
            {
                if (go.GetComponent<DPS>())
                {
                    Destroy(go.GetComponent<DPS>());
                }
                else if (go.GetComponent<Healer>())
                {
                    Destroy(go.GetComponent<Healer>());
                }
                else if (go.GetComponent<Tank>())
                {
                    Destroy(go.GetComponent<Tank>());
                }
                else if (go.GetComponent<Voleur>())
                {
                    Destroy(go.GetComponent<Voleur>());
                }
                go.SetActive(false);
            }
        }
        else
        {
            //Réactivez et repositionner les héros dans la scene de vue Top Down
            for (int i = 0; i < TeamClass.instance.heroTeam.Count; i++)
            {

                TeamClass.instance.heroTeam[i].sp.sortingLayerName = "Default";
                TeamClass.instance.heroTeam[i].transform.position = TeamClass.instance.heroTeam[i].beforeFightPos;
                TeamClass.instance.heroTeam[i].animator.enabled = true;
                TeamClass.instance.heroTeam[i].shadow.sortingLayerName = "Default";
                TeamClass.instance.heroTeam[i].sp.material = TeamClass.instance.heroTeam[i].defaultMaterial;
            }
            TeamClass.instance.UpdateFollowersQueue();

            if (!isAFlee)
            {
                TeamClass.instance.GainWinXpAndMana();
            }
            else
            {
                foreach (HeroClass hero in TeamClass.instance.heroTeam)
                {
                    hero.storedXp = 0;
                }
            }

            StartCoroutine(SettingsManager.instance.ChangeMusic(1));

            //Lancer l'animation de sortie en combat 2
            StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.outBlackAnim));

            EncounterManager.instance.isInSafeZone = false;
            PauseManager.instance.canTogglePause = true;


            //Reprendre musique monde

        }
        BattleTriggerManager.instance.inBattle = false;

    }

    public void SceneChangerUPDATE()
    {
        CameraFollow.instance.UpdateSceneCanvas();
        if (!BattleTriggerManager.instance.inBattle)
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    //Menu principal de départ
                    CameraFollow.instance.StopFollowPlayer();
                    EncounterManager.instance.isInSafeZone = true;
                    break;
                case 1:
                    //Menu principal
                    CameraFollow.instance.StopFollowPlayer();
                    EncounterManager.instance.isInSafeZone = true;
                    break;
                case 2:
                    //Combat
                    break;
                case 3:
                    //World 1
                    CameraFollow.instance.LookForPlayer();
                    TeamClass.instance.UpdateFollowersQueue();
                    EncounterManager.instance.isInSafeZone = false;
                    break;
                case 4:
                    //Btwn 1 & 2
                    CameraFollow.instance.LookForPlayer();
                    TeamClass.instance.UpdateFollowersQueue();
                    EncounterManager.instance.isInSafeZone = true;
                    break;
                case 5:
                    //World 2
                    CameraFollow.instance.LookForPlayer();
                    TeamClass.instance.UpdateFollowersQueue();
                    EncounterManager.instance.isInSafeZone = false;
                    break;
                case 6:
                    //Btwn 2 & 3
                    CameraFollow.instance.LookForPlayer();
                    TeamClass.instance.UpdateFollowersQueue();
                    EncounterManager.instance.isInSafeZone = true;
                    break;
                case 7:
                    //World 3
                    CameraFollow.instance.LookForPlayer();
                    TeamClass.instance.UpdateFollowersQueue();
                    EncounterManager.instance.isInSafeZone = false;
                    break;
                case 8:
                    //World 3-4
                    CameraFollow.instance.LookForPlayer();
                    TeamClass.instance.UpdateFollowersQueue();
                    EncounterManager.instance.isInSafeZone = true;
                    break;
                case 9:
                    //World 4
                    CameraFollow.instance.LookForPlayer();
                    TeamClass.instance.UpdateFollowersQueue();
                    EncounterManager.instance.isInSafeZone = false;
                    break;
            }
        }

    }

    public void SetBattleBG()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 5:
                //World 2
                UIFight.instance.bgImage.sprite = UIFight.instance.bg2;
                break;
            case 7:
                //World 3
                UIFight.instance.bgImage.sprite = UIFight.instance.bg3;
                break;
            case 9:
                //World 4
                if (BattleTriggerManager.instance.isBossBattle) UIFight.instance.bgImage.sprite = UIFight.instance.bgBoss;
                else UIFight.instance.bgImage.sprite = UIFight.instance.bg4;
                break;
            default:
                //World 1
                UIFight.instance.bgImage.sprite = UIFight.instance.bg1;
                break;
        }

    }
}
