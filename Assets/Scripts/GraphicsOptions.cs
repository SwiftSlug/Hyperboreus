using UnityEngine;
using UnityEngine.UI;

public class GraphicsOptions : MonoBehaviour
{
	Resolution[] resolutions;

	public Dropdown resolutionDropdown;
    public Dropdown aaDropdown;

	public Toggle windowToggle;
	public Toggle vsyncToggle;
    public Toggle anisoToggle;

	void Start()
	{
		resolutions = Screen.resolutions;
		AddResolutions();

		CheckFullscreen();
		CheckVSync();
        CheckAniso();
        CheckAA();
	}

	void CheckFullscreen()
	{
		if (Screen.fullScreen)
		{
			windowToggle.isOn = false;
		}
		else if (!Screen.fullScreen)
		{
			windowToggle.isOn = true;
		}
	}

	void CheckVSync()
	{
		if (QualitySettings.vSyncCount == 1 || QualitySettings.vSyncCount == 2)
		{
			vsyncToggle.isOn = true;
		}
		else
		{
			vsyncToggle.isOn = false;
		}
	}

    void CheckAniso()
    {
        if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable || QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable)
        {
            anisoToggle.isOn = true;
        }
        else
        {
            anisoToggle.isOn = false;
        }
    }

    void CheckAA()
    {
        if (QualitySettings.antiAliasing == 0)
        {
            aaDropdown.value = 0;
        }
        else if (QualitySettings.antiAliasing == 2)
        {
            aaDropdown.value = 1;
        }
        else if (QualitySettings.antiAliasing == 4)
        {
            aaDropdown.value = 2;
        }
        else if (QualitySettings.antiAliasing == 8)
        {
            aaDropdown.value = 3;
        }

        aaDropdown.RefreshShownValue();
    }

	public void ChangeFullscreen()
	{
		if(windowToggle.isOn)
		{
			Screen.fullScreen = false;
		}
		else
		{
			Screen.fullScreen = true;
		}
	}

	public void ChangeVSync()
	{
		if(vsyncToggle.isOn)
		{
			QualitySettings.vSyncCount = 1;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
	}

    public void ChangeAniso()
    {
        if(anisoToggle.isOn)
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        }
        else
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        }
    }

	void AddResolutions()
	{
		resolutionDropdown.ClearOptions();

		for (int i = 0; i < resolutions.Length; i++)
		{
			resolutionDropdown.options.Add(new Dropdown.OptionData(ConvertResolutionText(resolutions[i])));
		}

        resolutionDropdown.value = resolutions.Length - 1;

		resolutionDropdown.RefreshShownValue();
	}

	string ConvertResolutionText(Resolution resolution)
	{
		return resolution.width + " x " + resolution.height + "  " + resolution.refreshRate + "Hz";
	}

	public void ChangeResolution()
	{
		Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
	}

    public void ChangeAA()
    {
        if (aaDropdown.value == 0)
        {
            QualitySettings.antiAliasing = 0;
        }
        else if (aaDropdown.value == 1)
        {
            QualitySettings.antiAliasing = 2;
        }
        else if (aaDropdown.value == 2)
        {
            QualitySettings.antiAliasing = 4;
        }
        else if (aaDropdown.value == 3)
        {
            QualitySettings.antiAliasing = 8;
        }
    }
}