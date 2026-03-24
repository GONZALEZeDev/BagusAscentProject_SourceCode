using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

//
// Résumé :
//     SettingsManager est une classe qui gère les fonctions pour le menu des paramètres, ainsi que plusieurs paramètres.
//     When changing settings values, they get saved on PlayerPrefs automatically
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    public AudioMixer MainMixer;
    public AudioSource loopedAudio;
    public AudioSource withIntroAudio;
    public AudioClip[] OST;
    public float overworldMusicTimestamp = 0;
    public float transitionTimer = 0;
    public bool isTransitioning;
    public float introFightDuration = 1.371f; //En seconde
    public float introVSKaosDuration = 1.371f; //En seconde
    public float transitionDuration = 2.0f; //En seconde
    public ColorBlindMode colorType;
    public Canvas canvas;

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

    public void Start()
    {
        SettingsUIManager.instance.ShowSoundTab();
        canvas.enabled = false;
    }

    public IEnumerator ChangeMusic(int index)
    {
        yield return null;
        /*
        if (loopedAudio.clip == OST[1]) overworldMusicTimestamp = loopedAudio.time;
        withIntroAudio.Stop();
        loopedAudio.Stop();
        switch (index)
        {
            case 0:
                //Menu Principal
                overworldMusicTimestamp = 0;
                loopedAudio.time = 0;
                withIntroAudio.clip = null;
                loopedAudio.clip = OST[0];
                loopedAudio.Play();
                yield return null;
                break;
            case 1:
                //Overworld
                withIntroAudio.clip = null;
                loopedAudio.clip = OST[1];
                if (overworldMusicTimestamp != 0)
                {
                    loopedAudio.time = overworldMusicTimestamp;
                    overworldMusicTimestamp = 0;
                }
                loopedAudio.Play();
                yield return null;
                break;
            case 2:
                //Fight intro
                withIntroAudio.clip = OST[2];
                loopedAudio.clip = OST[3];
                loopedAudio.time = 0;
                loopedAudio.volume = 0;
                withIntroAudio.Play();
                yield return new WaitForSecondsRealtime(introFightDuration);
                loopedAudio.Play();
                isTransitioning = true;
                break;
            case 3:
                //Vs Kaos Intro
                withIntroAudio.clip = OST[4];
                loopedAudio.clip = OST[5];
                loopedAudio.time = 0;
                loopedAudio.volume = 0;
                withIntroAudio.Play();
                yield return new WaitForSecondsRealtime(introVSKaosDuration);
                loopedAudio.Play();
                isTransitioning = true;
                break;
            default:
                //No music, for gameOver for example
                overworldMusicTimestamp = 0;
                withIntroAudio.clip = null;
                loopedAudio.clip = null;
                break;
        }*/
    }

    public void StartMusicTransition()
    {
        // Vérifiez si une transition est déjà en cours
        if (!isTransitioning)
        {
            isTransitioning = true;
            transitionTimer = 0.0f;
        }
    }

    public void SetMasterVolume(float volume)
    {
        SettingsUIManager.instance.SetMasterVolumeUI(volume);
        //Set general value master volume to "volume" (change Music Master value)
        MainMixer.SetFloat("MasterVolume", volume);
        //Set playerprefs Master volume value to "volume"
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        SettingsUIManager.instance.SetMusicVolumeUI(volume);
        //Set general value music volume to "volume" (change Music Master value)
        MainMixer.SetFloat("MusicVolume", volume);
        //Set playerprefs Music volume value to "volume"
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        SettingsUIManager.instance.SetSfxVolumeUI(volume);
        //Set general value sfx volume to "volume" (change Sfx Master value)
        MainMixer.SetFloat("SfxVolume", volume);
        //Set playerprefs sfx volume value to "volume"
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        SettingsUIManager.instance.SetFullScreenUI(isFullScreen);
        Screen.fullScreen = isFullScreen;
        if (isFullScreen) PlayerPrefs.SetInt("isFullScreen", 1);
        else PlayerPrefs.SetInt("isFullScreen", 0);

    }

    public void SetFullScreen(int isFullScreen)
    {
        if(isFullScreen == 1)
        {
            SettingsUIManager.instance.SetFullScreenUI(true);
            Screen.fullScreen = true;
        }
        else
        {
            SettingsUIManager.instance.SetFullScreenUI(false);
            Screen.fullScreen = false;
        }
        PlayerPrefs.SetInt("isFullScreen", isFullScreen);
    }

    public void GoToMainMenu()
    {
        //On est dans le menu principal
        //Disable simplement le canvas settings
        canvas.enabled = false;
    }

    

    public void SetColors(int type)
    {
        colorType = (ColorBlindMode)type;
        FindFirstObjectByType<ColorBlindFilter>().mode = colorType;
        PlayerPrefs.SetInt("colorType", type);
    }
}
