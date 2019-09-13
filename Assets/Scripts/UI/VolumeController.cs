using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour 
{
	[SerializeField]
	private Slider musicSlider;

	[SerializeField]
	private Slider sfxSlider;

	[SerializeField]
	private Image musicToggle;
	[SerializeField]
	private Image sfxToggle;
	[SerializeField]
	private Sprite ON;
	[SerializeField]
	private Sprite OFF;
	private void OnDestroy()
	{
		SoundManager.Save();
	}
	void Start () 
	{
		musicSlider.value=SoundManager.GetVolumeSNG();
		sfxSlider.value=SoundManager.GetVolumeSFX();
	}
	
	void Update () 
	{
		if(musicSlider.value > 0)
		{
			musicToggle.sprite = ON;
		}
		else
		{
			musicToggle.sprite = OFF;
		}
		if(sfxSlider.value > 0)
		{
			sfxToggle.sprite = ON;
		}
		else
		{
			sfxToggle.sprite = OFF;
		}
		
		SoundManager.VolumeMusic(musicSlider.value);
		SoundManager.VolumeSFX(sfxSlider.value);
	}

}
