
using UnityEngine;
using UnityEngine.SceneManagement;


//
// Résumé :
//     PauseManager est une classe gérant le menu pause. Le menu pause comporte le management d'équipe avec la fiche de chaque héros, l'inventaire, et les paramètres (chacun dans un onglet)
//     Si le joueur fait pause durant un combat, il ne peut le faire que lorsque c'est au tour d'un des héros, n'a pas accès à l'onglet de l'inventaire,
//     et ne peut pas modifier l'arrangement de l'équipe en plein combat. Il ne peut également pas sauvegarder en plein combat, mais pourra l'abandonner de force.
public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public bool isPaused = false;


    //
    // Résumé :
    //     Ce paramètre change en fonction de si une animation de transition se joue ou non, si c'est le tour du héros lors d'un combat, et autre.
    //     Il est par défaut à false
    public bool canTogglePause = true;

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
        GetComponent<Canvas>().enabled = isPaused;
        PauseUIManager.instance.ShowSettingsTab();
    }


    public void togglePauseGame()
    {
        PauseUIManager.instance.UpdateTeamUI();
        //Réagis différemment selon la scene où nous sommes
        isPaused = !isPaused;
        GetComponent<Canvas>().enabled = isPaused;
        if (isPaused)
        {
            //Pause the game
            Time.timeScale = 0.0f;
            SfxManager.instance.PlayUISfx(3);
        }
        else
        {
            //Unpause
            Time.timeScale = 1.0f;
            SfxManager.instance.PlayUISfx(4);
        }

        if (BattleTriggerManager.instance.inBattle)
        {
            //On ne peut pas sauvegarde, ni accéder à l'inventaire depuis le menu pause en combat, ni changer la position des héros dans l'équipe
            PauseUIManager.instance.ToggleInvTabButton();
            PauseUIManager.instance.ToggleTeamManagement();
            //Le bouton de fuite prend la place du bouton de sauvegarde
            PauseUIManager.instance.ShowExitCombatButton();
        }
        else
        {
            PauseUIManager.instance.ShowSaveButton();
        }


    }

    public void FleeCombat()
    {
        Debug.Log("Fleeing combat...");
        StartCoroutine(SceneChanger.instance.UnloadBattleScene(true, false));
    }
    public void GoBackToMainMenu()
    {
        for (int i = 0; i < TeamClass.instance.heroTeam.Count; i++)
        {
            //On remet à 1 hp pour ne plus les considérer comme morts.
            TeamClass.instance.heroTeam[i].currentHP = 1;
            TeamClass.instance.heroTeam[i].sp.sortingLayerName = "Default";
            TeamClass.instance.heroTeam[i].animator.enabled = true;
            TeamClass.instance.heroTeam[i].shadow.sortingLayerName = "Default";
            TeamClass.instance.heroTeam[i].sp.material = TeamClass.instance.heroTeam[i].defaultMaterial;
        }
        StartCoroutine(SceneChanger.instance.ChangeScene(1, false));
    }

}
