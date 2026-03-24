using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Résumé :
//     SceneUpdateCaller gère l'appel de certaines fonctions et autres utilités APRES qu'une scene soit chargée (et non plus pendant)
public class SceneUpdateCaller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneChanger.instance.SceneChangerUPDATE();
    }
}
