using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;

    public RuntimeAnimatorController mageAnimator;
    public RuntimeAnimatorController dpsAnimator;
    public RuntimeAnimatorController tankAnimator;
    public RuntimeAnimatorController voleurAnimator;

    public Material defaultMat;

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
        GetComponent<Canvas>().enabled = false;
    }

    public void ContinueFromGOscreen()
    {
        Debug.Log("continue from GO Screen : " + TeamClass.instance.heroTeam.Count);

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
}
