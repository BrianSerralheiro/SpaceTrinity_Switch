using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class EnemySpawner : MonoBehaviour {
	public static int[] points=new int[2];
	public static bool boss,freeze;
	private int counter;
	public float timer;
	public WorldInfo world1;
	public static WorldInfo world;
	public static Material bloomMaterial;
	[SerializeField]
	Material bloomSpriteMaterial;
	/*
	Level Design
	World 1: 
	Stage 1 ID0: V0T1U0T4A1A6T2B2T1B2T1B2T1B2T2C8S2T5A2A5A8A9T3B1S0T4B0T1B0T1B0T1B0T2A0A3T1B7T1B7T1B7T1B7T4C6A0A7T5A0A3T9C9S1T4
	Stage 2 ID1: U1S2T5B0B1B2B3B4B5B6B7B8B9S0T3C7A0A2T2A0A2T3A1A3A4A7A8A9T5B1B2B3B4T9B5B6B7B8B9S1T5B0B1B2B3B4B5T5B6B7B8B9S2T4C8C9A1A6T4
	Stage 3 ID2: U2S2T2B4B5B6B7B9T1A0A1A2T9T2C8C7T5B8B9B3T4A1A6A8T5B0B2B4B6A0A2B8C9T3A0A4A8B1B8T2C5A3A5B1B7T9T4V1T2D0T1T1

	World 2:
	Stage 1 ID3: U3S0T4F2F6T5E0E0T1E0T1E0T1E9T1E9T1E9T1E9T1S2T9F2F4G0G7T9H0H3H7T2E0T1E0T1E0T1E0T9E7T1E7T1E7T1E7S2T9H0H3F2F6E1E3E6S0S2T9T2G7E0E1E2T9
	Stage 2 ID4: U4S2T4F0F7T1F3T9E0E2E4E6E8F0F7T9T3S2G7E0E0E0E0E0T1E7E7E7E7H0S1T9T2F0F7E2E3H7S0T2T9H0H7E2E4E5E6E7T2E0E0E2E2E4E4E6E6E7E7T5T9
	Stage 3 ID5: U5S2T4G1F2F3F4T2H2E0E2E5S0T9T5E1E9T1F0T1F6T4S2G1E8E9E3T5H1H6F3S1T9T9V2T2I0T4

	World 3:
	Stage 1 ID6: U6S2L0J1T4a1a2J1J1J1J1T9J5T2K4T5L1L6T2J0T1J0T1J0T1J3T1J3T1J3T1J6T1J6T1L2J8J9T4J9J8J7J6J5J4J3J2J1J0T3J4a4T5J4T1J4T1J4a4T9S0S2T4K5T2K2K6T5T9T9T6
	Stage 2 ID7: U7S2T1S0T1J1T1J1a1a2T1J1T1J1T1J4T1J4T1J4T1J4T1J4T9L0L7T5L0L4T9T2J0J2J4J6J8T5S0T5K0K7T3T9S0T1S2T9J9J1J2J3J4J5J6J7J8J9T9J9a3a5J8J7J6J5J4J3J2J1J0S2T9T5a1a2LT42L3T4L4L5T9T9T2
	Stage 3 ID8: U8S2J1J2J3J4J5J6a1a2J7J8J9K0T9J0J0J2J2J4J4J6J6J8J8T3T9J9J8J7J6J5J4J3J2J1J0T1J1J1J1J4J4J4J4J8J8T9T5L1L2L3L4L5T9T4S0a1L0L4T9T2K5J0J2J4J6J8T2J1J2Ja33J4K0K4T5T9T4K2J1J2J4J5a4T9T9T2S0S2T5V3T1M0T9

	World 4:
	Stage 1 ID9  : U9S0T1S2T4N1T1N1T1N2N4T4O1O8T4P2T5P7T9T1Q0Q6T9T1S0T1N1T1N2T1N3T1N4T1N5T1N6T4O2O5T5P4T5
	Stage 2 ID10 : U:S2T4P0T1P4T7O1T1T4T1O5T5N1T1N2T1N3T1N4T1N5T1N6T1N7T1N8T1N9T6S0T4Q1T1Q4T7O2T1N2T1N4T1N6T1O6T1S0T4N1T1N2T1N3T1N4T1N5T1N6T5  
	Stage 3 ID11 : U;S0S2T4P0T1P8T1P9T9T4N1T1N4T1N4T1N1T1N7T1N7T4Q2T1Q3T1Q4T6O1T1O3T1O6T6S0S2T9V4T1R0T5 
	Stage 4 ID12 : U<S0S2T4P9T2P8T5P9T9N1T1N3T1N3T1N3T1N1T1N4T1N6T1N6T4S0T4O2T1O3T1O4T1O5T9C8C9F4T9J1T1J1T1J2T1J2T1J3T1J3T1J4T1J4T1J5T1J5T1J6T1J6T1T4T2N1T1N2T3Q2T1Q4T1Q6T9S0S2S1T5R0	
	Final Stage:
	 */
	void Start()
	{
		if(!world)world=world1;
		if(!world.Boss)enabled=false;
		DialogManager.open=world.begining;
		boss = false;
		freeze=false;
		FinalBoss.last=false;
		SoundManager.Play(1);
		bloomMaterial=bloomSpriteMaterial;
		points[0]=0;
		points[1]=0;
	}
	public static void AddPost(GameObject go){
		go.GetComponent<Renderer>().sharedMaterial=bloomMaterial;
		go.layer=9;
	}
	void OnDestroy()
	{
		Cash.totalCash += points[0]/200;
		Cash.totalCash += points[1]/200;
		Cash.Save();
	}

	void Update()
	{
		if(Ship.paused) return;
		do
		{
			if(timer<=0 && counter<world.wave.Length && !boss)
			{
				Chose(world.wave.Substring(counter,2));
				counter+=2;
			}
		}
		while(timer<=0 && counter<world.wave.Length && !boss);
		if(counter>=world.wave.Length && !boss){
			DialogManager.open=world.end;
			Spawn(world.Boss).Position(0);
		}
		if(timer>0 && !boss) timer-=Time.deltaTime;
	}

	public void Chose(string s)
	{
		int i;
		if(int.TryParse(s.Substring(0,1),out i)|| s[0]=='/'|| s[0]=='.'){
			EnemyBase en;
			if(s[0]=='/')en=Spawn(world.subBoss);
			else if(s[0]=='.')en=Spawn(world.drone);
			else en=Spawn(world.enemies[i]);
			if(en)
			{
				en.Position(s[1]-48);
			}
			else{
				Debug.LogError("No script found for "+world.enemies[i].name);
			}
		}else if(s[0]=='t')timer+=float.Parse(s.Substring(1,1))/10f;
			else if(s[0]=='T')timer+=int.Parse(s.Substring(1,1));
	}
	
	EnemyBase Spawn(EnemyInfo en){

		GameObject go = new GameObject("enemy");
		go.AddComponent<SpriteRenderer>().sprite=en.sprites[0];
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic=true;
		r.useFullKinematicContacts=true;
		EnemyBase e=go.AddComponent(en.GetScript())as EnemyBase;
		e.SetSprites(en);
		return e;
	}
}
