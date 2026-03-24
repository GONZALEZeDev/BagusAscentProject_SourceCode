using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonUIFight : MonoBehaviour
{
    static public ButtonUIFight instance;

    public GameObject actionPanel;
    public GameObject attackSubMenu;
    // Récupère le personnage actif
    public Character activeCharacter;

    // Ajoutez des références aux boutons "Attaque", "Bloquer" et "Action" dans l'Inspector de Unity
    public Button attackButton;
    public Button inventoryButton;
    public Button skipButton;
    public Button blockButton;
    public Button actionButton;
    public Button backButton;
    public Button lastHoveredAttackButton;

    // Elements pour le ciblage
    public int targetedEnemyIndex = 0;
    public int targetedAllyIndex = 0;
    public bool isLockMode = false;
    public Character target;
    public ISkill targetingSkill;

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
        HideSubAttackMenu();
        actionPanel.SetActive(false);
        activeCharacter = OrderFight.instance.charactersOrder[UIFight.instance.activeCharacterIndex];
        
        if (OrderFight.instance.charactersObjects[0].CompareTag("ennemie"))
        {
            DesactivateMainButtons();
        }

    }

    void Update()
    {
        if(UIFight.instance.startTimer >= 0)
        {
            return;
        }

        activeCharacter = OrderFight.instance.charactersOrder[UIFight.instance.activeCharacterIndex];

        if (isLockMode)
        {
            if (targetingSkill.SkillAllie == true)
            {

                if(target == null || !target.CompareTag("heroe") || target.characterName == activeCharacter.characterName)
                {
                    foreach(HeroClass hero in TeamClass.instance.heroTeam)
                    {
                        if(hero.characterName != activeCharacter.characterName && hero.currentHP > 0) target = hero;
                    }
                }

                if (targetingSkill.SkillName != "Mana Transfer")
                {
                    UIFight.instance.UpdateUI();
                    TargetingAlly(target);
                }
                else
                {
                    UIFight.instance.UpdateUI();
                    activeCharacter.GetComponent<Voleur>().TargettingManatransfert();
                }
            }
            else
            {
                if (target == null || !target.CompareTag("ennemie"))
                {
                    foreach (Ennemies ennemy in BattleManager.instance.theEnnemies)
                    {
                        if (ennemy.currentHP > 0) target = ennemy;
                    }
                }
                UIFight.instance.UpdateUI();
                Targeting(target);
            }
        }
    }

    public void ActivateMainButtons()
    {
        Debug.Log("Activating Main Buttons");
        // Réafficher les boutons "Attaque", "Bloquer" et "Action"
        if (attackButton != null)
        {
            attackButton.gameObject.SetActive(true);
        }
        if (blockButton != null)
        {
            blockButton.gameObject.SetActive(true);
        }
        if (actionButton != null)
        {
            actionButton.gameObject.SetActive(true);
        }

    }

    public void DesactivateMainButtons()
    {
        Debug.Log("Desactivating Main Buttons");
        // Masquer les boutons "Attaque", "Bloquer" et "Action"
        if (attackButton != null)
        {
            attackButton.gameObject.SetActive(false);
        }
        if (blockButton != null)
        {
            blockButton.gameObject.SetActive(false);
        }
        if (actionButton != null)
        {
            actionButton.gameObject.SetActive(false);
        }

    }

    // Affiche le sous menu des attaques
    public void ShowAttackMenu()
    {
        DesactivateMainButtons();
        UIFight.instance.ResizeChildrenScaleSubMenu();

        Debug.Log(activeCharacter.characterName);

        for (int i = 1; i <= activeCharacter.currentSkills.Count; i++)
        {
            int skillIndex = i - 1; // Stocke la valeur actuelle de i
            Debug.Log(activeCharacter.currentSkills[skillIndex].SkillName + " -> " + skillIndex);

            // Construire le nom de l'attaque en utilisant l'index + 1 (car les attaques commencent à 1)
            string attName = "att" + (i);

            // Récupérer le bouton avec le nom de l'attaque
            GameObject attackButtonObject = attackSubMenu.transform.Find(attName).gameObject;

            // Récupérer le composant Button du bouton
            Button attackButton = attackButtonObject.GetComponent<Button>();

            // Récupérer le composant TextMeshProUGUI du bouton
            Image[] buttonImg = attackButton.GetComponentsInChildren<Image>();

            // Activer le bouton
            attackButtonObject.SetActive(true);

            // Définir le texte du bouton avec le nom de la compétence
            if (buttonImg[1] != null)
            {
                if (activeCharacter.currentSkills[skillIndex].SkillName == "Strike") {
                    switch(activeCharacter.characterName)
                    {
                        case "Gus":
                            buttonImg[1].sprite = PlayerInventory.instance.ui.attackSpriteList[0];
                            break;
                        case "Mystos":
                            buttonImg[1].sprite = PlayerInventory.instance.ui.attackSpriteList[3];
                            break;
                        case "Kant":
                            buttonImg[1].sprite = PlayerInventory.instance.ui.attackSpriteList[1];
                            break;
                        default:
                            //Rovel
                            buttonImg[1].sprite = PlayerInventory.instance.ui.attackSpriteList[2];
                            break;
                    }
                }
                else buttonImg[1].sprite = PlayerInventory.instance.ui.attackSpriteList[activeCharacter.currentSkills[skillIndex].SkillId];
                buttonImg[1].gameObject.name = activeCharacter.currentSkills[skillIndex].SkillName;
            }

            // Ajouter ou mettre à jour le déclencheur PointerEnter
            EventTrigger eventTrigger = attackButtonObject.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = attackButtonObject.AddComponent<EventTrigger>();
            }

            // Ajouter un déclencheur d'événement pour le survol du bouton par la souris
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter; // Utilisez eventID au lieu de eventType
            entry.callback.AddListener((data) => { 
                lastHoveredAttackButton = attackButton; 
                UIFight.instance.ShowDescAttack(skillIndex);
                SfxManager.instance.PlayUISfx(0);
            });
            eventTrigger.triggers.Add(entry);

            // Ajouter un déclencheur d'événement pour quand la souris cesse de survoler le bouton
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { UIFight.instance.HideDescAttack(); });
            eventTrigger.triggers.Add(entryExit);

            // Ajout de déclencheur d'évènement pour le clique de l'attaque
            EventTrigger.Entry attClicked = new EventTrigger.Entry();
            attClicked.eventID = EventTriggerType.PointerClick;
            attClicked.callback.AddListener((data) =>
            {
                if (activeCharacter.characterClass == "DPS")
                {
                    activeCharacter.gameObject.GetComponent<DPS>().ActivateSelectedAttack();
                }
                else if (activeCharacter.characterClass == "Tank")
                {
                    activeCharacter.gameObject.GetComponent<Tank>().ActivateSelectedAttack();
                }
                else if (activeCharacter.characterClass == "Healer")
                {
                    activeCharacter.gameObject.GetComponent<Healer>().ActivateSelectedAttack();
                }
                else if (activeCharacter.characterClass == "Voleur")
                {
                    activeCharacter.gameObject.GetComponent<Voleur>().ActivateSelectedAttack();
                }
                SfxManager.instance.PlayUISfx(1);
            });
            eventTrigger.triggers.Add(attClicked);
        }

        backButton.gameObject.gameObject.SetActive(true);
    }

    // Affiche les boutons du sous menu d'action
    public void ShowActionPanel()
    {
        DesactivateMainButtons();
        actionPanel.SetActive(true);
        backButton.gameObject.gameObject.SetActive(true);
    }

    // Cache les boutons du sous menu attaques actif
    public void HideSubAttackMenu()
    {
        
        for (int i = 1; i <= 6; i++)
        {
            string attName = "att" + i;
            attackSubMenu.transform.Find(attName).gameObject.SetActive(false);
        }
        backButton.gameObject.gameObject.SetActive(false);
        UIFight.instance.ResizeChildrenScaleMainMenu();  
    }


    // Cache les boutons du panel d'actiion
    public void HideActionPanel()
    {
        actionPanel.SetActive(false);
        backButton.gameObject.gameObject.SetActive(false);
    }


    void Targeting(Character target)
    {
        SpriteRenderer spriteRenderer = TeamClass.instance.heroTeam[targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = TeamClass.instance.heroTeam[targetedAllyIndex].defaultMaterial;

        string announcement = "Choose a target with the arrows or Q and D !";
        UIFight.instance.annoucementGO.SetActive(true);
        UIFight.instance.annoucementLayer1GO.SetActive(true);
        UIFight.instance.annoucementLayer2GO.SetActive(true);

        UIFight.instance.annoucementGO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;
        UIFight.instance.annoucementLayer1GO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;
        UIFight.instance.annoucementLayer2GO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;
            // Vérifie si la précédente cible visé est toujours vivante, sinon on change de cible automatique
        while (BattleManager.instance.theEnnemies[targetedEnemyIndex].currentHP <= 0)
        {
            if (targetedEnemyIndex +1 >= BattleManager.instance.theEnnemies.Count)
            {
                targetedEnemyIndex = 0;
            }
            else
            {
                targetedEnemyIndex++;   
            }
        }
        UIFight.instance.UpdateUI();


        spriteRenderer = BattleManager.instance.theEnnemies[targetedEnemyIndex].gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = UIFight.instance.outlineMaterial;


        // Vérifie si la flèche gauche du pavé numérique à été pressée
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            spriteRenderer = BattleManager.instance.theEnnemies[targetedEnemyIndex].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.material = BattleManager.instance.theEnnemies[targetedEnemyIndex].defaultMaterial;

            // Calcul l'index du prochain ennemie à cibler
            targetedEnemyIndex--;
            if (targetedEnemyIndex < 0)
            {
                targetedEnemyIndex = BattleManager.instance.theEnnemies.Count - 1;
            }

            while (BattleManager.instance.theEnnemies[targetedEnemyIndex].currentHP <= 0)
            {
                targetedEnemyIndex--;
                if (targetedEnemyIndex < 0)
                {
                    targetedEnemyIndex = BattleManager.instance.theEnnemies.Count - 1;
                }
            }

                spriteRenderer = BattleManager.instance.theEnnemies[targetedEnemyIndex].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.material = UIFight.instance.outlineMaterial;
            UIFight.instance.ennemie = BattleManager.instance.theEnnemies[targetedEnemyIndex];
            UIFight.instance.UpdateUI();
        }

        // Vérifie si la flèche droite du pavé numérique à été pressée
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            spriteRenderer = BattleManager.instance.theEnnemies[targetedEnemyIndex].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.material = BattleManager.instance.theEnnemies[targetedEnemyIndex].defaultMaterial;

            // Calcul l'index du prochain ennemie à cibler
            targetedEnemyIndex++;
            if (targetedEnemyIndex >= BattleManager.instance.theEnnemies.Count)
            {
                targetedEnemyIndex = 0;
            }

            while (BattleManager.instance.theEnnemies[targetedEnemyIndex].currentHP <= 0)
            {
                targetedEnemyIndex++;
                if (targetedEnemyIndex >= BattleManager.instance.theEnnemies.Count)
                {
                    targetedEnemyIndex = 0;
                }
            }

            spriteRenderer = BattleManager.instance.theEnnemies[targetedEnemyIndex].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.material = UIFight.instance.outlineMaterial;
            UIFight.instance.ennemie = BattleManager.instance.theEnnemies[targetedEnemyIndex];
            UIFight.instance.UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            activeCharacter.isblocking = false;
            UIFight.instance.annoucementGO.SetActive(false);
            UIFight.instance.annoucementLayer1GO.SetActive(false);
            UIFight.instance.annoucementLayer2GO.SetActive(false);
            // Desactivation après sélection de la cible
            isLockMode = false;
            target = BattleManager.instance.theEnnemies[targetedEnemyIndex];

            spriteRenderer = BattleManager.instance.theEnnemies[targetedEnemyIndex].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.material = BattleManager.instance.theEnnemies[targetedEnemyIndex].defaultMaterial;

            // Appeler la fonction d'exécution de l'attaque
            targetingSkill.ActivateAttackEffect(activeCharacter, target);

            if(targetingSkill.SkillName == "Strike")
            {
                switch(activeCharacter.characterName)
                {
                    case "Mystos":
                        StartCoroutine(SfxManager.instance.PlayHeroAttackSfx(4));
                        break;
                    case "Gus":
                        StartCoroutine(SfxManager.instance.PlayHeroAttackSfx(1));
                        break;
                    case "Kant":
                        StartCoroutine(SfxManager.instance.PlayHeroAttackSfx(2));
                        break;
                    default:
                        //Rovel
                        StartCoroutine(SfxManager.instance.PlayHeroAttackSfx(3));
                        break;
                }
            }
            else StartCoroutine(SfxManager.instance.PlayHeroAttackSfx(targetingSkill));


            // Met à jour les statistiques
            UIFight.instance.UpdateUI();
            // Annonce de l'attaque et désactivation des boutons + remise à zéro du menu principale
            string textAnnoucement = activeCharacter.characterName + " uses their skill " + targetingSkill.SkillName + " !";
            HideSubAttackMenu();
            UIFight.instance.HideDescAttack();
            UIFight.instance.ResizeChildrenScaleMainMenu();
            UIFight.instance.ResizeChildrenScaleNormalEnnemiePanel();
            target = null;
            StartCoroutine(DisplayAnnouncement(textAnnoucement));
        }
    }

    void TargetingAlly(Character target)
    {
        SpriteRenderer spriteRenderer = BattleManager.instance.theEnnemies[targetedEnemyIndex].gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = BattleManager.instance.theEnnemies[targetedEnemyIndex].defaultMaterial;

        string announcement = "Choose an ally with the arrows or Q and D !";
        UIFight.instance.annoucementGO.SetActive(true);
        UIFight.instance.annoucementLayer1GO.SetActive(true);
        UIFight.instance.annoucementLayer2GO.SetActive(true);

        UIFight.instance.annoucementGO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;
        UIFight.instance.annoucementLayer1GO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;
        UIFight.instance.annoucementLayer2GO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;

        // Vérifie si la précédente cible visée est toujours vivante, sinon on change de cible automatique
        while (TeamClass.instance.heroTeam[targetedAllyIndex].currentHP <= 0 || TeamClass.instance.heroTeam[targetedAllyIndex].characterName == activeCharacter.characterName)
        {
            if (targetedAllyIndex + 1 >= TeamClass.instance.heroTeam.Count)
            {
                targetedAllyIndex = 0;
            }
            else
            {
                targetedAllyIndex++;
            }
        }

        spriteRenderer = TeamClass.instance.heroTeam[targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = UIFight.instance.outlineMaterial;

        // Vérifie si la flèche gauche du pavé numérique ou D a été pressée
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            spriteRenderer = TeamClass.instance.heroTeam[targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.material = TeamClass.instance.heroTeam[targetedAllyIndex].defaultMaterial;

            // Calcule l'index du prochain allié à cibler
            targetedAllyIndex--;
            if (targetedAllyIndex < 0)
            {
                targetedAllyIndex = TeamClass.instance.heroTeam.Count - 1;
            }


            while (TeamClass.instance.heroTeam[targetedAllyIndex].currentHP <= 0 || TeamClass.instance.heroTeam[targetedAllyIndex].characterName == activeCharacter.characterName)
            {
                targetedAllyIndex--;
                if (targetedAllyIndex < 0)
                {
                    targetedAllyIndex = TeamClass.instance.heroTeam.Count - 1;
                }
            }

            spriteRenderer = TeamClass.instance.heroTeam[targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.material = UIFight.instance.outlineMaterial;
            UIFight.instance.ennemie = TeamClass.instance.heroTeam[targetedAllyIndex];
        }

        // Vérifie si la flèche droite du pavé numérique ou Q a été pressée
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            spriteRenderer = TeamClass.instance.heroTeam[targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.material = TeamClass.instance.heroTeam[targetedAllyIndex].defaultMaterial;

            // Calcule l'index du prochain allié à cibler
            targetedAllyIndex++;
            Debug.Log(targetedAllyIndex);

            if (targetedAllyIndex > TeamClass.instance.heroTeam.Count - 1)
            {
                targetedAllyIndex = 0;
            }

            while (TeamClass.instance.heroTeam[targetedAllyIndex].currentHP <= 0 || TeamClass.instance.heroTeam[targetedAllyIndex].characterName == activeCharacter.characterName)
            {
                targetedAllyIndex++;
                if (targetedAllyIndex > TeamClass.instance.heroTeam.Count - 1)
                {
                    targetedAllyIndex = 0;
                }
            }


            spriteRenderer = TeamClass.instance.heroTeam[targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.material = UIFight.instance.outlineMaterial;
            UIFight.instance.ennemie = TeamClass.instance.heroTeam[targetedAllyIndex];
        }

        // Vérifie si la touche Entrée a été pressée
        if (Input.GetKeyDown(KeyCode.Return))
        {
            foreach (var enemy in BattleManager.instance.theEnnemies)
            {
                if (enemy.currentHP > 0)
                {
                    UIFight.instance.ennemie = enemy;
                    break;
                }
            }
            UIFight.instance.annoucementGO.SetActive(false);
            UIFight.instance.annoucementLayer1GO.SetActive(false);
            UIFight.instance.annoucementLayer2GO.SetActive(false);
            // Désactivation après sélection de la cible
            isLockMode = false;
            target = TeamClass.instance.heroTeam[targetedAllyIndex];

            spriteRenderer = TeamClass.instance.heroTeam[targetedAllyIndex].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.material = TeamClass.instance.heroTeam[targetedAllyIndex].defaultMaterial;
            // Appeler la fonction d'exécution de l'attaque
            targetingSkill.ActivateAttackEffect(activeCharacter, target);

            StartCoroutine(SfxManager.instance.PlayHeroAttackSfx(targetingSkill));

            // Met à jour les statistiques
            UIFight.instance.UpdateUI();
            // Annonce de l'attaque et désactivation des boutons + remise à zéro du menu principal
            string textAnnoucement = activeCharacter.characterName + " uses their skill " + targetingSkill.SkillName + " !";
            HideSubAttackMenu();
            UIFight.instance.HideDescAttack();
            UIFight.instance.ResizeChildrenScaleMainMenu();
            UIFight.instance.ResizeChildrenScaleNormalEnnemiePanel();
            target = null;
            StartCoroutine(DisplayAnnouncement(textAnnoucement));
        }
    }

    public void BlockingNextAttack()
    {
        StartCoroutine(SfxManager.instance.PlayHeroAttackSfx(0));
        activeCharacter.isblocking = true;
        // Annonce et déclenchement du blocage
        string announcementText = activeCharacter.characterName + " blocks the next attack !";
        DesactivateMainButtons();
        UIFight.instance.ResizeChildrenScaleMainMenu();
        StartCoroutine(DisplayAnnouncement(announcementText));
    }


    public IEnumerator DisplayAnnouncement(string announcement)
    {
        UIFight.instance.annoucementGO.SetActive(true);
        UIFight.instance.annoucementLayer1GO.SetActive(true);
        UIFight.instance.annoucementLayer2GO.SetActive(true);

        UIFight.instance.annoucementGO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;
        UIFight.instance.annoucementLayer1GO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;
        UIFight.instance.annoucementLayer2GO.GetComponentInChildren<TextMeshProUGUI>().text = announcement;

        // Attendre pendant 5 secondes
        yield return new WaitForSeconds(2.5f);

        // Désactiver l'annonce après l'attente
        UIFight.instance.annoucementGO.SetActive(false);
        UIFight.instance.annoucementLayer1GO.SetActive(false);
        UIFight.instance.annoucementLayer2GO.SetActive(false);
        if (!BattleManager.instance.isFightFinished)
        {
            UIFight.instance.NextCharacter();
        }
    }

    public void UseItem(int itemID)
    {
        // Annonce et activation de l'objet
        PlayerInventory.instance.UseItem(PlayerInventory.instance.GetItemById(itemID), activeCharacter);
        string announcementText = activeCharacter.characterName + " used a " + PlayerInventory.instance.GetItemById(itemID).ItemName + "!";
        DesactivateMainButtons();
        HideActionPanel();
        UIFight.instance.ResizeChildrenScaleMainMenu();
        StartCoroutine(DisplayAnnouncement(announcementText));
    }
}