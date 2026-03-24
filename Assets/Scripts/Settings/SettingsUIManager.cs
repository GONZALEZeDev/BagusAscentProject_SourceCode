using TMPro;
using UnityEngine;
using UnityEngine.UI;
//
// Résumé :
//     SettingsUIManager gère la partie interface utilisateur des paramètres du jeu (changer les valeurs des éléments graphiques en fonction de celles du jeu)
public class SettingsUIManager : MonoBehaviour
{
    public static SettingsUIManager instance;
    public GameObject soundTab;
    public GameObject videoTab;
    public GameObject generalTab;

    public TMP_Dropdown colorsDropdown;
    public Toggle fullscreenToggle;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public TMP_Dropdown colorsPauseDropdown;
    public Toggle fullscreenPauseToggle;
    public Slider masterPauseSlider;
    public Slider musicPauseSlider;
    public Slider sfxPauseSlider;

    public Image soundTabBtnImage;
    public Image videoTabBtnImage;
    public Image generalTabBtnImage;

    public Sprite TabBtnNotSelected;
    public Sprite TabBtnSelected;



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

    public void ShowSoundTab()
    {
        soundTabBtnImage.sprite = TabBtnSelected;
        generalTabBtnImage.sprite = TabBtnNotSelected;
        videoTabBtnImage.sprite = TabBtnNotSelected;

        soundTab.SetActive(true);
        videoTab.SetActive(false);
        generalTab.SetActive(false);
    }

    public void SetMasterVolumeUI(float volume)
    {
        masterPauseSlider.value = volume;
        masterSlider.value = volume;
    }

    public void SetMusicVolumeUI(float volume)
    {
        musicPauseSlider.value = volume;
        musicSlider.value = volume;
    }

    public void SetSfxVolumeUI(float volume)
    {
        sfxPauseSlider.value = volume;
        sfxSlider.value = volume;
    }

    public void ShowVideoTab()
    {
        videoTabBtnImage.sprite = TabBtnSelected;
        generalTabBtnImage.sprite = TabBtnNotSelected;
        soundTabBtnImage.sprite = TabBtnNotSelected;

        videoTab.SetActive(true);
        generalTab.SetActive(false);
        soundTab.SetActive(false);
    }

    public void SetFullScreenUI(bool isFullScreen)
    {
        fullscreenPauseToggle.isOn = isFullScreen;
        fullscreenToggle.isOn = isFullScreen;
    }

    public void ShowGeneralTab()
    {
        generalTabBtnImage.sprite = TabBtnSelected;
        soundTabBtnImage.sprite = TabBtnNotSelected;
        videoTabBtnImage.sprite = TabBtnNotSelected;

        generalTab.SetActive(true);
        soundTab.SetActive(false);
        videoTab.SetActive(false);
    }

    public void SetColorsUI(int type)
    {
        colorsDropdown.value = type;
        colorsPauseDropdown.value = type;
    }
}
