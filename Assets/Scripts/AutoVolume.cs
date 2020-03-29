using UnityEngine;
[RequireComponent(typeof( AudioSource))]
public class AutoVolume : MonoBehaviour {
	AudioSource source;
	[SerializeField]
	float delay;
	void Start () {
		source=GetComponent<AudioSource>();
	}	
	void Update () {
		float volume=SoundManager.GetVolumeSNG();
		if(source.time<delay)volume=(source.time/delay)*volume;
		else if(source.time>source.clip.length-delay)volume=((source.clip.length-source.time)/delay)*volume;
		source.volume=volume;
	}
}
