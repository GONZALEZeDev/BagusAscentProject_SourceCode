using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMusicController : MonoBehaviour
{
    void Update()
    {
        if (SettingsManager.instance.isTransitioning)
        {
            // Mettez à jour le timer de transition
            SettingsManager.instance.transitionTimer += Time.deltaTime;

            // Calculez le volume pour la transition
            float introVolume = Mathf.Lerp(1.0f, 0.0f, SettingsManager.instance.transitionTimer / SettingsManager.instance.transitionDuration);
            float loopVolume = Mathf.Lerp(0.0f, 1.0f, SettingsManager.instance.transitionTimer / SettingsManager.instance.transitionDuration);

            // Ajustez les volumes des AudioSource
            SettingsManager.instance.withIntroAudio.volume = introVolume;
            SettingsManager.instance.loopedAudio.volume = loopVolume;

            // Vérifiez si la transition est terminée
            if (SettingsManager.instance.transitionTimer >= SettingsManager.instance.transitionDuration)
            {
                SettingsManager.instance.isTransitioning = false;

                // Arrêtez la version avec l'intro et mettez en boucle la version bouclée
                SettingsManager.instance.withIntroAudio.Stop();
                SettingsManager.instance.withIntroAudio.volume = 100;
            }
        }
    }
}
