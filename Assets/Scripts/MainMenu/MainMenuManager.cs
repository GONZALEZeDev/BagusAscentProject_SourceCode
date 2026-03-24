
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    bool isPlayerNew;
    public RectTransform newGameButtonGO;
    public RectTransform continueButtonGO;
    public RectTransform settingsButtonGO;

    #region Heroes Visual Data
    //Pourquoi sont-ils référencés un par un comme ceci ?
    //On attribue ces valeurs au moment de rentrer dans une partie, donc en appuyant sur New Game ou Continue.
    //On ne peut pas renseigner les sprites par un chemin, car ça ne marcherait plus une fois le jeu build.
    //On ne peux pas dire à chaque classe des héros (DPS, Tank, Voleur, Healer) de prendre ces sprites spécifiquement par défaut. On doit leur donner durant l'éxécution.
    //On ne peut utiliser les Import Settings de Unity pour remplir des valeurs par défaut, car celles ci ne sont utilisables que dans l'éditeur, donc les variables correspondantes seront vides si j'instancie la classe.

    public RuntimeAnimatorController mageAnimator;
    public RuntimeAnimatorController dpsAnimator;
    public RuntimeAnimatorController tankAnimator;
    public RuntimeAnimatorController voleurAnimator;

    public Sprite mageSideSprite;
    public Sprite dpsSideSprite;
    public Sprite voleurSideSprite;
    public Sprite tankSideSprite;

    public Sprite mageDeadSprite;
    public Sprite dpsDeadSprite;
    public Sprite voleurDeadSprite;
    public Sprite tankDeadSprite;

    public Sprite magePauseIcon;
    public Sprite dpsPauseIcon;
    public Sprite voleurPauseIcon;
    public Sprite tankPauseIcon;

    public Material defaultMat;
    #endregion

    float startButtonYNew = -276.5f;
    float settingsButtonYNew = -398.9f;
    float startButtonYNotNew = -262.4801f;
    float settingsButtonYNotNew = -448.2f;
    float continueButtonY = -356f;

    // Start is called before the first frame update
    public void Start()
    {
        //Met isNew à true ou false selon si le fichier SaveFile est vide ou non. (fonction dans SaveManager)
        isPlayerNew = SaveManager.instance.isSaveFileEmpty();
        
        //Selon la valeur de isNew, le menu ne sera pas ordonné de la même manière.
        if (isPlayerNew)
        {
            //Afficher le layout où il n'y a pas le bouton continuer
            continueButtonGO.gameObject.SetActive(false);
            newGameButtonGO.localPosition = new Vector3(-15.009f, startButtonYNew, 0);
            settingsButtonGO.localPosition = new Vector3(-15.009f, settingsButtonYNew, 0);
        }
        else
        {
            //Afficher le layout où il y a le bouton continuer
            continueButtonGO.gameObject.SetActive(true);
            newGameButtonGO.localPosition = new Vector3(-15.009f, startButtonYNotNew, 0);
            settingsButtonGO.localPosition = new Vector3(-15.009f, settingsButtonYNotNew, 0);
        }
        continueButtonGO.localPosition = new Vector3(-15.009f, continueButtonY, 0);

        PlayerInventory.instance.inventoryItems.Clear();
        TeamClass.instance.heroTeam = new List<HeroClass>();

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
        StartCoroutine(SettingsManager.instance.ChangeMusic(0));
    }


    //Si l'on fait New Game, ça ne touche rien au fichier, ça ne fait que lancer une nouvelle partie.
    //Mais si l'on sauvegarde dans cette nouvelle partie, ça écrase les anciennes données.
    //Ca signifie que le fichier SaveFile n'est vide que si c la premiere fois que l'on joue.
    public void NewGame()
    {
        HeroClass tempHero;

        //Charger les données par défaut en commençant une nouvelle partie (1. Mage/Healer 2. Voleur 3. Tank 4. DPS)

        //DPS
        TeamClass.instance.heroesGO[0].AddComponent<DPS>();
        TeamClass.instance.moveKeybinds.SetActive(true);
        tempHero = TeamClass.instance.heroesGO[0].GetComponent<HeroClass>();
        tempHero.animController = dpsAnimator;
        tempHero.sideSprite = dpsSideSprite;
        tempHero.deadSprite = dpsDeadSprite;
        tempHero.faceSprite = dpsPauseIcon;
        tempHero.defaultMaterial = defaultMat;
        TeamClass.instance.heroesGO[0].GetComponent<DPS>().InitializeBasicData();
        TeamClass.instance.heroTeam.Add(TeamClass.instance.heroesGO[0].GetComponent<HeroClass>());

        //Tank
        TeamClass.instance.heroesGO[1].AddComponent<Tank>();
        tempHero = TeamClass.instance.heroesGO[1].GetComponent<HeroClass>();
        tempHero.animController = tankAnimator;
        tempHero.sideSprite = tankSideSprite;
        tempHero.deadSprite = tankDeadSprite;
        tempHero.faceSprite = tankPauseIcon;
        tempHero.defaultMaterial = defaultMat;
        TeamClass.instance.heroesGO[1].GetComponent<Tank>().InitializeBasicData();
        TeamClass.instance.heroTeam.Add(TeamClass.instance.heroesGO[1].GetComponent<HeroClass>());

        //Voleur
        TeamClass.instance.heroesGO[2].AddComponent<Voleur>();
        tempHero = TeamClass.instance.heroesGO[2].GetComponent<HeroClass>();
        tempHero.animController = voleurAnimator;
        tempHero.sideSprite = voleurSideSprite;
        tempHero.deadSprite = voleurDeadSprite;
        tempHero.faceSprite = voleurPauseIcon;
        tempHero.defaultMaterial = defaultMat;
        TeamClass.instance.heroesGO[2].GetComponent<Voleur>().InitializeBasicData();
        TeamClass.instance.heroTeam.Add(TeamClass.instance.heroesGO[2].GetComponent<HeroClass>());

        //Mage/Healer
        TeamClass.instance.heroesGO[3].AddComponent<Healer>();
        tempHero = TeamClass.instance.heroesGO[3].GetComponent<HeroClass>();
        tempHero.animController = mageAnimator;
        tempHero.sideSprite = mageSideSprite;
        tempHero.deadSprite = mageDeadSprite;
        tempHero.faceSprite = magePauseIcon;
        tempHero.defaultMaterial = defaultMat;
        TeamClass.instance.heroesGO[3].GetComponent<Healer>().InitializeBasicData();
        TeamClass.instance.heroTeam.Add(TeamClass.instance.heroesGO[3].GetComponent<HeroClass>());

        TeamClass.instance.UpdateFollowersQueue();
        PauseUIManager.instance.UpdateInvHeroesUI();
        PauseUIManager.instance.UpdateTeamUI();

        //Charger la premiere scene
        StartCoroutine(SceneChanger.instance.ChangeScene(3, false));
    }

    public void Continue()
    {

        //Charger les données du saveFile
        DataToSave loadedData = SaveManager.instance.GetSave();

        for (int i = 0; i < loadedData.invValues.Count(); i++)
        {
            PlayerInventory.instance.AddItem(loadedData.invKeysID[i], loadedData.invValues[i]);
        }

        for (int i = 0; i < loadedData.heroesData.Count; i++)
        {
            HeroClass tempHero;
            switch (loadedData.heroesData[i].characterName)
            {
                case "Gus":
                    TeamClass.instance.heroesGO[i].AddComponent<DPS>();
                    tempHero = TeamClass.instance.heroesGO[i].GetComponent<HeroClass>();
                    tempHero.animController = dpsAnimator;
                    tempHero.sideSprite = dpsSideSprite;
                    tempHero.deadSprite = dpsDeadSprite;
                    tempHero.faceSprite = dpsPauseIcon;
                    tempHero.defaultMaterial = defaultMat;
                    TeamClass.instance.heroesGO[i].GetComponent<DPS>().InitializeBasicData();
                    TeamClass.instance.heroTeam.Add(TeamClass.instance.heroesGO[i].GetComponent<HeroClass>());
                    TeamClass.instance.heroTeam[i].ContinueHeroClass(loadedData.heroesData[i]);
                    TeamClass.instance.heroesGO[i].GetComponent<DPS>().InitializeSkills(loadedData.heroesData[i].learnedSkills);

                    break;
                case "Mystos":
                    TeamClass.instance.heroesGO[i].AddComponent<Healer>();
                    tempHero = TeamClass.instance.heroesGO[i].GetComponent<HeroClass>();
                    tempHero.animController = mageAnimator;
                    tempHero.sideSprite = mageSideSprite;
                    tempHero.deadSprite = mageDeadSprite;
                    tempHero.faceSprite = magePauseIcon;
                    tempHero.defaultMaterial = defaultMat;
                    TeamClass.instance.heroesGO[i].GetComponent<Healer>().InitializeBasicData();
                    TeamClass.instance.heroTeam.Add(TeamClass.instance.heroesGO[i].GetComponent<HeroClass>());
                    TeamClass.instance.heroTeam[i].ContinueHeroClass(loadedData.heroesData[i]);
                    TeamClass.instance.heroesGO[i].GetComponent<Healer>().InitializeSkills(loadedData.heroesData[i].learnedSkills);
                    break;
                case "Kant":
                    TeamClass.instance.heroesGO[i].AddComponent<Tank>();
                    tempHero = TeamClass.instance.heroesGO[i].GetComponent<HeroClass>();
                    tempHero.animController = tankAnimator;
                    tempHero.sideSprite = tankSideSprite;
                    tempHero.deadSprite = tankDeadSprite;
                    tempHero.faceSprite = tankPauseIcon;
                    tempHero.defaultMaterial = defaultMat;
                    TeamClass.instance.heroesGO[i].GetComponent<Tank>().InitializeBasicData();
                    TeamClass.instance.heroTeam.Add(TeamClass.instance.heroesGO[i].GetComponent<HeroClass>());
                    TeamClass.instance.heroTeam[i].ContinueHeroClass(loadedData.heroesData[i]);
                    TeamClass.instance.heroesGO[i].GetComponent<Tank>().InitializeSkills(loadedData.heroesData[i].learnedSkills);
                    break;
                default:
                    //Rovel
                    TeamClass.instance.heroesGO[i].AddComponent<Voleur>();
                    tempHero = TeamClass.instance.heroesGO[i].GetComponent<HeroClass>();
                    tempHero.animController = voleurAnimator;
                    tempHero.sideSprite = voleurSideSprite;
                    tempHero.deadSprite = voleurDeadSprite;
                    tempHero.faceSprite = voleurPauseIcon;
                    tempHero.defaultMaterial = defaultMat;
                    TeamClass.instance.heroesGO[i].GetComponent<Voleur>().InitializeBasicData();
                    TeamClass.instance.heroTeam.Add(TeamClass.instance.heroesGO[i].GetComponent<HeroClass>());
                    TeamClass.instance.heroTeam[i].ContinueHeroClass(loadedData.heroesData[i]);
                    TeamClass.instance.heroesGO[i].GetComponent<Voleur>().InitializeSkills(loadedData.heroesData[i].learnedSkills);
                    break;
            }
        }

        TeamClass.instance.UpdateFollowersQueue();

        WorldsManager.instance.world1ChestsOpened = loadedData.world1Chests;
        WorldsManager.instance.world2ChestsOpened = loadedData.world2Chests;
        WorldsManager.instance.world3ChestsOpened = loadedData.world3Chests;
        WorldsManager.instance.world4ChestsOpened = loadedData.world4Chests;

        //Charger la scene en mémoire sceneChanger.ChangeScene()
        PauseUIManager.instance.UpdateInvItemsUI();
        PauseUIManager.instance.UpdateInvHeroesUI();
        PauseUIManager.instance.UpdateTeamUI();
        StartCoroutine(SceneChanger.instance.ChangeScene(loadedData.sceneIndex, true));
    }
    public void GoToSettings()
    {
        SettingsManager.instance.canvas.enabled = true;
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameCoroutine());
    }

    public IEnumerator ExitGameCoroutine()
    {
        //Lance une animation de fade in...
        StartCoroutine(TransitionManager.instance.PlayTransition(TransitionManager.TransitionType.inBlackAnim));
        Debug.Log("Quitting the game...");
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
