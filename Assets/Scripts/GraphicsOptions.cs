using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GraphicsOptions : MonoBehaviour
{
	Resolution[] resolutions;

	public Dropdown resolutionDropdown;

	public Toggle windowToggle;
	public Toggle vsyncToggle;

	void Start()
	{
		resolutions = Screen.resolutions;
		AddResolutions();

		CheckFullscreen();
		CheckVSync();
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

	void AddResolutions()
	{
		resolutionDropdown.ClearOptions();

		for (int i = 0; i < resolutions.Length; i++)
		{
			resolutionDropdown.options.Add(new Dropdown.OptionData(ConvertResolutionText(resolutions[i])));
		}

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
}