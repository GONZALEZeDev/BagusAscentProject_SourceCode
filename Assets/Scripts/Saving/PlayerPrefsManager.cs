
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager instance { get; set; }

    //NOTE : PlayerPrefs also stores the default values
    #region Default Player Prefs Values
    const bool isFullScreen = false;
    const float musicVolume = -10f;
    const float sfxVolume = -75f;
    const float masterVolume = 0f;
    const int colorType = 0;
    #endregion

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Start()
    {
        ApplyPlayerPrefs();
    }

    public void ApplyPlayerPrefs()
    {
        //set general values to playerprefs values
        if(PlayerPrefs.HasKey("isFullScreen")) SettingsManager.instance.SetFullScreen(PlayerPrefs.GetInt("isFullScreen"));
        else SettingsManager.instance.SetFullScreen(isFullScreen);

        if (PlayerPrefs.HasKey("masterVolume")) SettingsManager.instance.SetMasterVolume(PlayerPrefs.GetFloat("masterVolume"));
        else SettingsManager.instance.SetMasterVolume(masterVolume);

        if (PlayerPrefs.HasKey("musicVolume")) SettingsManager.instance.SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
        else SettingsManager.instance.SetMusicVolume(musicVolume);

        if (PlayerPrefs.HasKey("sfxVolume")) SettingsManager.instance.SetSfxVolume(PlayerPrefs.GetFloat("sfxVolume"));
        else SettingsManager.instance.SetSfxVolume(sfxVolume);

        if (PlayerPrefs.HasKey("colorType"))
        {
            SettingsManager.instance.SetColors(PlayerPrefs.GetInt("colorType"));
            SettingsUIManager.instance.SetColorsUI(PlayerPrefs.GetInt("colorType"));
        }
        else {
            SettingsManager.instance.SetColors(colorType);
            SettingsUIManager.instance.SetColorsUI(colorType);
        }
        
    }
}
