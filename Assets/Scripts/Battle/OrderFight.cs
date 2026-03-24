using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class OrderFight : MonoBehaviour
{
    static public OrderFight instance;

    public List<Character> charactersOrder; // Liste des personnages trié
    public List<GameObject> charactersObjects; // Liste des objets des personnages trié

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

        // Combinaison des listes de héros et d'ennemis
        charactersOrder.AddRange(TeamClass.instance.heroTeam);
        charactersOrder.AddRange(BattleManager.instance.theEnnemies);

        // Tri de la liste des personnages en fonction de leur niveau
        charactersOrder.Sort((x, y) => y.level.CompareTo(x.level));

        // Remplir charactersObjects à partir des GameObjects associés aux Character dans charactersOrder
        foreach (var cha in charactersOrder)
        {
            GameObject characterObject = cha.gameObject; // Obtenez le GameObject associé à Character
            if (characterObject != null)
            {
                charactersObjects.Add(characterObject);
            }
            else
            {
                Debug.LogWarning("GameObject not found for: " + cha.characterName);
            }
        }

        // Affiche l'ordre d'attaque dans la console (à des fins de débogage)
        foreach (var character in charactersOrder)
        {
            Debug.Log("Character: " + character.characterName + ", Level: " + character.level);
        }

        // Vous pouvez maintenant utiliser la liste triée pour déterminer l'ordre d'attaque dans votre logique de combat
    }

}