using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField]
	private AudioClip menu;
	[SerializeField]
	private AudioClip preloaded;
	[SerializeField]
	private AudioClip[] _sounds;
	[SerializeField]
	private string[] _names;
	[SerializeField]
	private string[] fx_names;

	private AudioSource soundPlayer;
	private AudioSource songPlayer;

	private static SoundManager _soundManager;
	private static ResourceRequest request;
	private static ResourceRequest autorequest;
	private static float weight;
	private static float volumeSNG = 1;
	private static float volumeSFX = 1;
	private static int loadId=1;
	private void Update()
	{
		if(weight>0)weight-=Time.deltaTime;
		if(request!=null)
		{
			if(request.isDone){
				preloaded =request.asset as AudioClip;
				request=null;
			}
		}
		if(loadId<_sounds.Length && autorequest.isDone)
		{
			_sounds[loadId]=autorequest.asset as AudioClip;
			if(++loadId<_sounds.Length) autorequest=Resources.LoadAsync<AudioClip>("Audio/"+ fx_names[loadId]);
			else autorequest=null;
		}
		if(Ship.paused)
		{
			songPlayer.volume -= Time.deltaTime;
		}
		else if(_soundManager.songPlayer.volume<volumeSNG)
		{
			_soundManager.songPlayer.volume+=Time.deltaTime/3f;
			if(_soundManager.songPlayer.volume>volumeSNG) _soundManager.songPlayer.volume=volumeSNG;
		}
	}
	void Awake() 
	{
		if(_soundManager)
		{
			Destroy(gameObject);
			return;
		}
		songPlayer = gameObject.AddComponent<AudioSource>();
		soundPlayer = gameObject.AddComponent<AudioSource>();
		songPlayer.loop = true;
		DontDestroyOnLoad(gameObject);
		_soundManager = this;
		
		autorequest=Resources.LoadAsync<AudioClip>("Audio/"+ fx_names[loadId]);
	}

	public static void Play (int i)
	{
		if(_soundManager == null)
		{
			Debug.LogWarning("SoundManager nao inicializado");
			return;
		}
		if(i==0)
		{
			_soundManager.songPlayer.clip =_soundManager.menu;
		}
		else
			_soundManager.songPlayer.clip =_soundManager.preloaded;

		_soundManager.songPlayer.volume=0f;
		_soundManager.songPlayer.Play();
		request=Resources.LoadAsync<AudioClip>("Audio/"+ _soundManager._names[(i+1)%_soundManager._names.Length]);
	}

	public static void PlayEffects (int i)
	{
		PlayEffects(i,0,0);
	}
	public static void PlayEffects (int i,float w, float l)
	{
		if(_soundManager == null)
		{
			Debug.LogWarning("SoundEffects nao inicializado");
			return;
		}
		if(l==0 || weight<l){
			_soundManager.soundPlayer.PlayOneShot(_soundManager._sounds[i],volumeSFX);
			weight+=w;
		}

	}

	public static void VolumeMusic(float i)
	{
		volumeSNG = i;
		_soundManager.songPlayer.volume=i;
	}

	public static void VolumeSFX(float i)
	{

		volumeSFX = i;
	}
	public static float GetVolumeSFX()
	{
		return volumeSFX;
	}

	public static float GetVolumeSNG()
	{
		return volumeSNG;
	}
	public static void Save()
	{
		PlayerPrefs.SetFloat("volumeSFX",volumeSFX);
		PlayerPrefs.SetFloat("volumeSNG",volumeSNG);
	}
	public static void Load()
	{
		if(PlayerPrefs.HasKey("volumeSFX"))volumeSFX =PlayerPrefs.GetFloat("volumeSFX");
		if(PlayerPrefs.HasKey("volumeSNG")) volumeSNG=PlayerPrefs.GetFloat("volumeSNG");
	}
}
