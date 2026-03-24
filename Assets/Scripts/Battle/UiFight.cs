using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class UIFight : MonoBehaviour
{
    static public UIFight instance;

    //TextMesh des héros
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI currentCharacterHealthText;
    public TextMeshProUGUI currentCharacterManaText;
    public TextMeshProUGUI maxCharacterHealthText;
    public TextMeshProUGUI maxCharacterManaText;

    //TextMesh des ennemies
    public TextMeshProUGUI ennemieNameText;
    public TextMeshProUGUI currentEnnemieHealthText;
    public TextMeshProUGUI currentEnnemieManaText;
    public TextMeshProUGUI maxEnnemieHealthText;
    public TextMeshProUGUI maxEnnemieManaText;

    //Fenêtre d'inventaire
    public TextMeshProUGUI itemNameTxt;
    public TextMeshProUGUI itemDescTxt;
    public GameObject invWindows;
    public GameObject[] itemSlots = new GameObject[10];
    public TextMeshProUGUI[] itemQty = new TextMeshProUGUI[10];

    //Backgrounds
    public Image bgImage;
    public Sprite bg1;
    public Sprite bg2;
    public Sprite bg3;
    public Sprite bg4;
    public Sprite bgBoss;

    // Différentes instances de script ou ressources nécessaire
    public Material outlineMaterial;
    public Character ennemie;

    // Tout les games objects à instancier
    public GameObject parentHeroePanel;
    public GameObject parentEnnemiePanel;
    public GameObject annoucementGO;
    public GameObject annoucementLayer1GO;
    public GameObject annoucementLayer2GO;
    public GameObject nameAttack;
    public GameObject manaCostAttack;
    public GameObject descAttack;

    public int startTimer = 120; //Timer de quelques secondes permettant de décaler légèrement le début du combat lorsqu'on y entre.

    // Variables 
    public Character activeCharacter;
    public GameObject activeCharacterObject;
    public int activeCharacterIndex = 0;

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

    void Start()
    {
        BattleManager.instance.ReadyBattle();
        
        StartCoroutine(StartDelay());
        nameAttack.SetActive(false);
        manaCostAttack.SetActive(false);
        descAttack.SetActive(false);
        annoucementGO.SetActive(false);
        annoucementLayer1GO.SetActive(false);
        annoucementLayer2GO.SetActive(false);
        invWindows.SetActive(false);
        foreach (GameObject item in itemSlots) item.SetActive(false);
        itemDescTxt.gameObject.SetActive(false);
        itemNameTxt.gameObject.SetActive(false);
    }

    // Fonction servant à régler un problème de rapidité d'exécution cassant l'orderFight
    IEnumerator StartDelay()
    {
        // Attendre 1 seconde
        yield return new WaitForSeconds(0.1f);

        characterNameText.text = TeamClass.instance.GetAliveHeroes()[0].characterName;
        // Affiche la santé actuelle du personnage héro
        currentCharacterHealthText.text = "" + TeamClass.instance.GetAliveHeroes()[0].currentHP;
        // Affiche le mana actuel du personnage héro
        currentCharacterManaText.text = "" + TeamClass.instance.GetAliveHeroes()[0].currentMana;
        // Affiche la santé max du personnage héro
        maxCharacterHealthText.text = "" + TeamClass.instance.GetAliveHeroes()[0].maxHP;
        // Affiche le mana max du personnage héro
        maxCharacterManaText.text = "" + TeamClass.instance.GetAliveHeroes()[0].maxMana;

        UpdateUI();
        StartCoroutine(activeCharacter.SetAnimation(true));
    }


    // Update is called once per frame
    void Update()
    {
        if (startTimer >= 0)
        {
            startTimer--;
            if (startTimer == 0)
            {
                Debug.Log("Timer Finished");
            }
            return;
        }
        activeCharacter = OrderFight.instance.charactersOrder[activeCharacterIndex];
        activeCharacterObject = OrderFight.instance.charactersOrder[activeCharacterIndex].gameObject;

        if (activeCharacter.currentHP <= 0)
        {
            NextCharacter();
        }
        UpdateUI();
        // Pour cet exemple, nous changeons le personnage actif à chaque appui de touche "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextCharacter();
        }
    }

    // Méthode pour mettre à jour l'UI avec les informations du personnage actif
    public void UpdateUI()
    {
        activeCharacter = OrderFight.instance.charactersOrder[activeCharacterIndex];
        activeCharacterObject = OrderFight.instance.charactersOrder[activeCharacterIndex].gameObject;

        if (activeCharacterObject.CompareTag("heroe"))
        {
            PauseManager.instance.canTogglePause = true;
            // Affiche le nom du personnage héro actif
            characterNameText.text = activeCharacter.characterName;
            // Affiche la santé actuelle du personnage héro
            currentCharacterHealthText.text = "" + activeCharacter.currentHP;
            // Affiche le mana actuel du personnage héro
            currentCharacterManaText.text = "" + activeCharacter.currentMana;
            // Affiche la santé max du personnage héro
            maxCharacterHealthText.text = "" + activeCharacter.maxHP;
            // Affiche le mana max du personnage héro
            maxCharacterManaText.text = "" + activeCharacter.maxMana;

            // Affiche le nom du personnage ennemie actif
            ennemieNameText.text = ennemie.characterName;
            // Affiche la santé actuel du personnage ennemie
            currentEnnemieHealthText.text = "" + ennemie.currentHP;
            // Affiche le mana actuel du personnage ennemie
            currentEnnemieManaText.text = "" + ennemie.currentMana;
            // Affiche la santé max du personnage ennemie
            maxEnnemieHealthText.text = "" + ennemie.maxHP;
            // Affiche le mana max du personnage ennemie
            maxEnnemieManaText.text = "" + ennemie.maxMana;
        }
        else
        {
            // Affiche le nom du personnage héro actif
            characterNameText.text = TeamClass.instance.GetAliveHeroes()[0].characterName;
            // Affiche la santé actuelle du personnage héro
            currentCharacterHealthText.text = "" + TeamClass.instance.GetAliveHeroes()[0].currentHP;
            // Affiche le mana actuel du personnage héro
            currentCharacterManaText.text = "" + TeamClass.instance.GetAliveHeroes()[0].currentMana;
            // Affiche la santé max du personnage héro
            maxCharacterHealthText.text = "" + TeamClass.instance.GetAliveHeroes()[0].maxHP;
            // Affiche le mana max du personnage héro
            maxCharacterManaText.text = "" + TeamClass.instance.GetAliveHeroes()[0].maxMana;

            // Affiche le nom du personnage ennemie actif
            ennemieNameText.text = activeCharacter.characterName;
            // Affiche la santé actuel du personnage ennemie
            currentEnnemieHealthText.text = "" + activeCharacter.currentHP;
            // Affiche le mana actuel du personnage ennemie
            currentEnnemieManaText.text = "" + activeCharacter.currentMana;
            // Affiche la santé max du personnage ennemie
            maxEnnemieHealthText.text = "" + activeCharacter.maxHP;
            // Affiche le mana max du personnage ennemie
            maxEnnemieManaText.text = "" + activeCharacter.maxMana;
        }

        // Accède au composant SpriteRenderer du personnage actif
        SpriteRenderer spriteRenderer = activeCharacterObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = outlineMaterial;
    }

    // Affiche les informations de l'attaque ciblé par le curseur de la souris
    public void ShowDescAttack(int index)
    {
        ResizeChildrenScaleDescAttackPanel();
        Debug.Log(index);
        // Modifier le texte des composants TextMeshProUGUI
        nameAttack.GetComponent<TextMeshProUGUI>().text = activeCharacter.currentSkills[index].SkillName;
        manaCostAttack.GetComponent<TextMeshProUGUI>().text = activeCharacter.currentSkills[index].SkillCost + " MANA";
        descAttack.GetComponent<TextMeshProUGUI>().text = activeCharacter.currentSkills[index].SkillDesc;

        nameAttack.SetActive(true);
        manaCostAttack.SetActive(true);
        descAttack.SetActive(true);
    }

    public void ShowItemInfos(int itemID)
    {
        ResizeChildrenScaleDescAttackPanel();

        // Modifier le texte des composants TextMeshProUGUI
        itemNameTxt.text = PlayerInventory.instance.GetItemById(itemID).ItemName;
        itemDescTxt.text = PlayerInventory.instance.GetItemById(itemID).ItemDesc;

        itemNameTxt.gameObject.SetActive(true);
        itemDescTxt.gameObject.SetActive(true);
    }

    public void HideItemInfos()
    {
        itemNameTxt.gameObject.SetActive(false);
        itemDescTxt.gameObject.SetActive(false);

        ResizeChildrenScaleNormalEnnemiePanel();
    }

    // Cache les informations d'attaque
    public void HideDescAttack()
    {
        nameAttack.SetActive(false);
        manaCostAttack.SetActive(false);
        descAttack.SetActive(false);

        ResizeChildrenScaleNormalEnnemiePanel();
    }

    public void ResizeChildrenScaleSubMenu()
    {
        // Réduire de moitié la taille de l'enfant
        Vector3 newScale = new Vector3(0.7f, 0.7f, 0.7f);
        parentHeroePanel.transform.localScale = newScale;

        // Nouvelles coordonnées pour déplacer le parent
        //-43f, -22f
        Vector3 newParentPosition = new Vector3(-84f, -39f, 0f);
        // Déplacer le parent à de nouvelles coordonnées
        parentHeroePanel.transform.localPosition = newParentPosition;
    }

    public void ResizeChildrenScaleMainMenu()
    {
        // Réduire de moitié la taille de l'enfant
        Vector3 newScale = new Vector3(1f, 1f, 1f);
        parentHeroePanel.transform.localScale = newScale;

        // Nouvelles coordonnées pour déplacer le parent
        Vector3 newParentPosition = new Vector3(2f, 6f, 0f);
        // Déplacer le parent à de nouvelles coordonnées
        parentHeroePanel.transform.localPosition = newParentPosition;
    }

    public void ResizeChildrenScaleDescAttackPanel()
    {
        // Réduire de moitié la taille de l'enfant
        Vector3 newScale = new Vector3(0.7f, 0.7f, 0.7f);
        parentEnnemiePanel.transform.localScale = newScale;

        // Nouvelles coordonnées pour déplacer le parent
        //60f, -12f
        Vector3 newParentPosition = new Vector3(95f, -35f, 0f);
        // Déplacer le parent à de nouvelles coordonnées
        parentEnnemiePanel.transform.localPosition = newParentPosition;
    }

    public void ResizeChildrenScaleNormalEnnemiePanel()
    {
        // Réduire de moitié la taille de l'enfant
        Vector3 newScale = new Vector3(1f, 1f, 1f);
        parentEnnemiePanel.transform.localScale = newScale;

        // Nouvelles coordonnées pour déplacer le parent
        Vector3 newParentPosition = new Vector3(-14f, 0f, 0f);
        // Déplacer le parent à de nouvelles coordonnées
        parentEnnemiePanel.transform.localPosition = newParentPosition;
    }

    public void GoBack()
    {
        ButtonUIFight.instance.HideSubAttackMenu();
        ButtonUIFight.instance.HideActionPanel();

        ButtonUIFight.instance.isLockMode = false;

        SpriteRenderer spriteRenderer = BattleManager.instance.theEnnemies[ButtonUIFight.instance.targetedEnemyIndex].gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = BattleManager.instance.theEnnemies[ButtonUIFight.instance.targetedEnemyIndex].defaultMaterial;
        spriteRenderer = TeamClass.instance.heroTeam[ButtonUIFight.instance.targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = TeamClass.instance.heroTeam[ButtonUIFight.instance.targetedAllyIndex].defaultMaterial;
        annoucementGO.SetActive(false); 
        annoucementLayer1GO.SetActive(false); 
        annoucementLayer2GO.SetActive(false);

        // Redimensionne le panel des héros à sa position d'origine
        ResizeChildrenScaleMainMenu();
    }

    public void HighlightTargetEnemies(List<GameObject> targetEnemies)
    {
        // Assurez-vous que la liste d'ennemis n'est pas null
        if (targetEnemies != null && targetEnemies.Count > 0)
        {
            foreach (var targetEnemy in targetEnemies)
            {
                // Accédez au composant SpriteRenderer de l'ennemi ciblé
                SpriteRenderer enemySpriteRenderer = targetEnemy.GetComponent<SpriteRenderer>();

                // Appliquez le matériau d'outline
                enemySpriteRenderer.material = outlineMaterial;
            }
        }
    }


    // Méthode pour passer au personnage suivant
    public void NextCharacter()
    {
        StartCoroutine(activeCharacter.SetAnimation(false));
        if (activeCharacterObject.CompareTag("ennemie"))
        {
            activeCharacterObject.GetComponent<Ennemies>().isHisTurn = false;
            activeCharacterObject.GetComponent<Ennemies>().EndTurn();
        }
        ButtonUIFight.instance.HideSubAttackMenu();
        ButtonUIFight.instance.HideActionPanel();

        //On nettoie les EventTrigger du personnage actuel
        if (activeCharacterObject.CompareTag("heroe"))
        {
            for (int i = 1; i <= activeCharacter.currentSkills.Count; i++)
            {
                // Construire le nom de l'attaque en utilisant l'index + 1 (car les attaques commencent à 1)
                string attName = "att" + (i);

                // Récupérer le bouton avec le nom de l'attaque
                GameObject attackButtonObject = ButtonUIFight.instance.attackSubMenu.transform.Find(attName).gameObject;

                //Destruction de son EventTrigger pour en ajouter un frais au prochain tour
                Destroy(attackButtonObject.GetComponent<EventTrigger>());
            }
        }

        // Accède au composant SpriteRenderer du personnage actif
        SpriteRenderer spriteRenderer = OrderFight.instance.charactersOrder[activeCharacterIndex].GetComponent<SpriteRenderer>();
        // Change le matériau du SpriteRenderer
        spriteRenderer.material = OrderFight.instance.charactersOrder[activeCharacterIndex].defaultMaterial;

        // Passe au personnage suivant
        activeCharacterIndex = (activeCharacterIndex + 1) % OrderFight.instance.charactersOrder.Count;

        // Check de la liste d'effet du nouvel activeCharacter
        OrderFight.instance.charactersOrder[activeCharacterIndex].CheckEffectList();

        Debug.Log("Début tour " + OrderFight.instance.charactersOrder[activeCharacterIndex].name);
        if (OrderFight.instance.charactersOrder[activeCharacterIndex].gameObject.CompareTag("heroe"))
        {
            ButtonUIFight.instance.ActivateMainButtons();
        }
        else if (OrderFight.instance.charactersOrder[activeCharacterIndex].gameObject.CompareTag("ennemie"))
        {
            ButtonUIFight.instance.DesactivateMainButtons();
            PauseManager.instance.canTogglePause = false;
        }

        
        ResizeChildrenScaleNormalEnnemiePanel();
        HideDescAttack();

        // Met à jour l'UI avec les informations du nouveau personnage actif
        UpdateUI();
        StartCoroutine(activeCharacter.SetAnimation(true));
    }

    public void ShowInvWindow()
    {
        ResizeChildrenScaleSubMenu();

        int index = 0;
        foreach (var pair in PlayerInventory.instance.inventoryItems)
        {
            itemSlots[index].SetActive(false);
            itemSlots[index].GetComponent<EventTrigger>().triggers.Clear();
            itemSlots[index].GetComponent<Button>().onClick.RemoveAllListeners();

            if (pair.Value > 0)
            {
                Debug.Log(pair.Key.ItemName);
                //Lui attribuer un slot
                itemSlots[index].SetActive(true);
                itemSlots[index].GetComponent<Image>().sprite = PlayerInventory.instance.itemSprites[pair.Key.ItemID];
                itemQty[index].text = pair.Value.ToString();

                    //Ajoute les déclencheurs de l'objet pour quand on le survole
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerEnter; // Utilisez eventID au lieu de eventType
                    entry.callback.AddListener((data) => { ShowItemInfos(pair.Key.ItemID); });
                    itemSlots[index].GetComponent<EventTrigger>().triggers.Add(entry);


                // Ajouter un déclencheur d'événement pour quand la souris cesse de survoler le bouton
                    EventTrigger.Entry entryExit = new EventTrigger.Entry();
                    entryExit.eventID = EventTriggerType.PointerExit;
                    entryExit.callback.AddListener((data) => { HideItemInfos(); });
                    itemSlots[index].GetComponent<EventTrigger>().triggers.Add(entryExit);


                //Ajouter un listener avec le bon ItemID en paramètre d'entrée
                    itemSlots[index].GetComponent<Button>().onClick.AddListener(() => ButtonUIFight.instance.UseItem(pair.Key.ItemID));
                
                index++;
            }
        }

        invWindows.SetActive(true);
    }

}
