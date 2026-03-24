using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIManager : MonoBehaviour
{
    public static PauseUIManager instance;

    public Button invTabButton;
    public Image invTabIcon;
    public Sprite invTabSpriteEnabled;
    public Sprite invTabSpriteDisabled;
    public Sprite defaultSkillIcon;

    public GameObject[] teamPosBtnGO;

    public GameObject invTabGO;
    public GameObject teamTabGO;
    public GameObject settingsTabGO;

    public GameObject exitCombatGO;
    public GameObject saveGO;

    public GameObject soundTabGO;
    public GameObject videoTabGO;
    public GameObject generalTabGO;

    public Image[] heroesPauseImage = new Image[4];
    public TextMeshProUGUI[] heroesHP = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] heroesMana = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] heroesNames = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] heroesLevels = new TextMeshProUGUI[4];
    public GameObject[] heroesSkillsIconsGO = new GameObject[4];
    public Image[] heroesXPBars = new Image[4];

    public PlayerInventoryUI invUI;

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
        }
    }

    public void ToggleInvTabButton()
    {
        invTabButton.interactable = !invTabButton.interactable;
        if (invTabButton.interactable) invTabIcon.sprite = invTabSpriteEnabled;
        else invTabIcon.sprite = invTabSpriteDisabled;
    }

    public void ToggleTeamManagement()
    {
        foreach (GameObject buttonGO in teamPosBtnGO)
        {
            buttonGO.SetActive(!buttonGO.activeSelf);
        }
    }

    public void ShowTeamTab()
    {
        UpdateTeamUI();
        teamTabGO.SetActive(true);
        invTabGO.SetActive(false);
        settingsTabGO.SetActive(false);
    }
    public void ShowInvTab()
    {
        UpdateInvHeroesUI();
        UpdateInvItemsUI();
        invTabGO.SetActive(true);
        teamTabGO.SetActive(false);
        settingsTabGO.SetActive(false);
    }
    public void ShowSettingsTab()
    {
        ShowSoundTab();
        settingsTabGO.SetActive(true);
        teamTabGO.SetActive(false);
        invTabGO.SetActive(false);
    }

    public void ShowSoundTab()
    {
        soundTabGO.SetActive(true);
        videoTabGO.SetActive(false);
        generalTabGO.SetActive(false);
    }

    public void ShowVideoTab()
    {
        videoTabGO.SetActive(true);
        soundTabGO.SetActive(false);
        generalTabGO.SetActive(false);
    }

    public void ShowGeneralTab()
    {
        generalTabGO.SetActive(true);
        videoTabGO.SetActive(false);
        soundTabGO.SetActive(false);
    }

    public void SettingsBtnPointerEnter(GameObject btn)
    {
        Debug.Log(btn.name +" : Enter");
        TextMeshProUGUI tempTxt = btn.GetComponentInChildren<TextMeshProUGUI>();
        Image[] tempImg = btn.GetComponentsInChildren<Image>(true);

        tempTxt.color = new Color32(245, 255, 110, 255);

        for (int i = 0; i < tempImg.Length; i++)
        {
            switch (i)
            {
                case 0:
                    //Left symbol
                    tempImg[i].color = new Color32(249, 255, 186, 255);
                    break;
                case 1:
                    //Right symbol
                    tempImg[i].color = new Color32(249, 255, 186, 255);
                    break;
                default:
                    //Underline
                    break;
            }
        }
    }

    public void SettingsBtnPointerExit(GameObject btn)
    {
        Debug.Log(btn.name + " : Exit");
        TextMeshProUGUI tempTxt = btn.GetComponentInChildren<TextMeshProUGUI>();
        Image[] tempImg = btn.GetComponentsInChildren<Image>(true);

        //Si le tab en question n'est pas celui séléctionné
        if (!tempImg[2].gameObject.activeSelf)
        {
            tempTxt.color = new Color32(86, 92, 66, 255);

            for (int i = 0; i < tempImg.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        //Left symbol
                        tempImg[i].color = new Color32(157, 162, 139, 255);
                        break;
                    case 1:
                        //Right symbol
                        tempImg[i].color = new Color32(157, 162, 139, 255);
                        break;
                    default:
                        //Underline
                        break;
                }
            }
        }
    }

    public void SettingsBtnPointerDown(GameObject btn)
    {
        Debug.Log(btn.name + " : Down");
        TextMeshProUGUI tempTxt = btn.GetComponentInChildren<TextMeshProUGUI>();
        Image[] tempImg = btn.GetComponentsInChildren<Image>(true);

        tempTxt.color = new Color32(159, 161, 119, 255);

        for (int i = 0; i < tempImg.Length; i++)
        {
            switch (i)
            {
                case 0:
                    //Left symbol
                    tempImg[i].color = new Color32(207, 209, 195, 255);
                    break;
                case 1:
                    //Right symbol
                    tempImg[i].color = new Color32(207, 209, 195, 255);
                    break;
                default:
                    //Underline
                    break;
            }
        }
    }

    public void UpdateTeamUI()
    {
        Debug.Log("Updating Team UI");
        //Updates the fields
        for (int i = 0; i < 4; i++)
        {
            Image[] tempIcons = heroesSkillsIconsGO[i].GetComponentsInChildren<Image>();
            heroesHP[i].text = $"HP\r\n{TeamClass.instance.heroTeam[i].currentHP}/{TeamClass.instance.heroTeam[i].maxHP}";
            heroesMana[i].text = $"Mana\r\n{TeamClass.instance.heroTeam[i].currentMana}/{TeamClass.instance.heroTeam[i].maxMana}";
            heroesNames[i].text = TeamClass.instance.heroTeam[i].characterName;
            heroesLevels[i].text = TeamClass.instance.heroTeam[i].level.ToString();
            heroesXPBars[i].fillAmount = (TeamClass.instance.heroTeam[i].currentXp / TeamClass.instance.heroTeam[i].currentLvlCap);
            if (TeamClass.instance.heroTeam[i].currentHP <= 0)
            {
                heroesPauseImage[i].sprite = TeamClass.instance.heroTeam[i].deadSprite;
            }
            else
            {
                heroesPauseImage[i].sprite = TeamClass.instance.heroTeam[i].faceSprite;
            }
            for (int j = 0; j < tempIcons.Length; j++)
            {
                if(j % 2 == 1)
                {
                    if (j / 2 < TeamClass.instance.heroTeam[i].currentSkills.Count)
                    {
                        if(TeamClass.instance.heroTeam[i].currentSkills[j / 2].SkillName == "Strike")
                        {
                            switch (TeamClass.instance.heroTeam[i].characterName)
                            {
                                case "Gus":
                                    tempIcons[j].sprite = PlayerInventory.instance.ui.attackSpriteList[0];
                                    break;
                                case "Mystos":
                                    tempIcons[j].sprite = PlayerInventory.instance.ui.attackSpriteList[3];
                                    break;
                                case "Kant":
                                    tempIcons[j].sprite = PlayerInventory.instance.ui.attackSpriteList[1];
                                    break;
                                default:
                                    //Rovel
                                    tempIcons[j].sprite = PlayerInventory.instance.ui.attackSpriteList[2];
                                    break;
                            }
                        }
                        else tempIcons[j].sprite = PlayerInventory.instance.ui.attackSpriteList[TeamClass.instance.heroTeam[i].currentSkills[j / 2].SkillId];
                    }
                    else tempIcons[j].sprite = defaultSkillIcon;
                }
            }

        }
    }

    public void UpdateInvHeroesUI()
    {
        //Changes the sprites of the buttons to the correct order of the heroes in the team
        for (int i = 0; i < invUI.heroesItemTargetSprites.Count; i++)
        {
            invUI.heroesItemTargetSprites[i].sprite = TeamClass.instance.heroTeam[i].faceSprite;
            invUI.heroesItemTargetMANATexts[i].text = TeamClass.instance.heroTeam[i].currentMana + "/" + TeamClass.instance.heroTeam[i].maxMana;
            invUI.heroesItemTargetHPTexts[i].text = TeamClass.instance.heroTeam[i].currentHP + "/" + TeamClass.instance.heroTeam[i].maxHP;
        }
    }

    public void UpdateInvItemsUI()
    {
        //Displays only items with 1 or more of them in inventory
        invUI.UpdateItems();
    }

    public void ShowExitCombatButton()
    {
        exitCombatGO.SetActive(true);
        saveGO.SetActive(false);
    }

    public void ShowSaveButton()
    {
        saveGO.SetActive(true);
        exitCombatGO.SetActive(false);
    }

}
