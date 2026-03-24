using UnityEngine;
using UnityEngine.SceneManagement;


//
// Résumé :
//     PauseController est une classe contrôlant si le joueur appuie sur Escape ou pas pour quitter/entrer dans le menu pause du moment qu'il n'est pas dans le menu settings
public class PauseController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PauseManager.instance.canTogglePause && SceneManager.GetActiveScene().buildIndex > 1)
        {
            PauseManager.instance.togglePauseGame();
        }
    }
}
