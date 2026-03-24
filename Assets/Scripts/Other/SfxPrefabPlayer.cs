using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cette classe sert ) jouer des sfx sur des éléments qui originaires d'une scene différente du SfxManager
public class SfxPrefabPlayer : MonoBehaviour
{
    public void PLayUISfx(int index)
    {
        SfxManager.instance.PlayUISfx(index);
    }

    public void PlayHeroAttackSfx(int index)
    {
        SfxManager.instance.PlayHeroAttackSfx(index);
    }

    public void PlayEnnemiesAttackSfx(int index)
    {
        SfxManager.instance.PlayEnnemiesAttackSfx(index);
    }

    public void PlayOtherSfx(int index)
    {
        SfxManager.instance.PlayOtherSfx(index);
    }
}
