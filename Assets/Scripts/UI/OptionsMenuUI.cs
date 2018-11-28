using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    public Dropdown resolutionsDropdown;
    public Toggle fullscreenToggle;

    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        ResolutionsDropdownSetup();
        FullscreenToggleSetup();
        AudioSlidersSetup();
    }

    void AudioSlidersSetup()
    {
        Debug.Log(PlayerPrefs.GetFloat("musicVolume"));
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        Debug.Log(musicVolumeSlider.value);
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("effectsVolume");
    }

    void ResolutionsDropdownSetup()
    {
        List<string> resolutionStrings = new List<string>();

        resolutionsDropdown.ClearOptions();

        int resIndex = 0;
        Resolution[] resolutions = Settings.resolutions; 
        for (int i = 0; i < resolutions.Length; i++)
        {
            string temp = resolutions[i].width + "x" + resolutions[i].height;
            resolutionStrings.Add(temp);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
                resIndex = i;
        }
        resolutionsDropdown.AddOptions(resolutionStrings);
        resolutionsDropdown.value = resIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    void FullscreenToggleSetup()
    {
        fullscreenToggle.isOn = (PlayerPrefs.GetInt("fullScreen") == 1) ? true : false;
    }

}
