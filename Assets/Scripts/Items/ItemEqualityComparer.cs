using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEqualityComparer : IEqualityComparer<IItem>
{
    public bool Equals(IItem x, IItem y)
    {
        // Implémenter la logique de comparaison ici
        // Par exemple, comparer les noms des items
        return x.ItemName == y.ItemName;
    }

    public int GetHashCode(IItem obj)
    {
        // Retourner le code de hachage de l'objet
        // Utiliser une propriété unique de l'objet
        return obj.ItemName.GetHashCode();
    }
}
