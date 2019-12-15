using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaveEditor : EditorWindow
{
	WorldInfo world;
	List<Entry> entries=new List<Entry>();
	Entry currentEntry,subEntry;
	EnemyHelper currentEnemy,subBossHelper;
	int x,y,counter,fullY,tempY,lastH;
	Vector2 pos,pos2;
	double saveTime,lastTime;
	[MenuItem("Window/Wave Editor")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(WaveEditor));
	}
	void OnGUI()
	{
#region Setting
		WorldInfo w=world;
		world=EditorGUILayout.ObjectField("World",world,typeof(WorldInfo),false)as WorldInfo;
		if(w!=world && world!=null)Revert(world.wave);
		if(world==null)return;
		x=(int)position.width/10;
		y=(int)position.height/10;
		if(entries.Count==0 && !string.IsNullOrEmpty(world.wave))
			Revert(world.wave);
		if(currentEntry==null)
		{
			if(entries.Count==0) entries.Add(new Entry());
			currentEntry=entries[0];
		}
#endregion
#region EnemyList
		GUILayout.BeginArea(new Rect(x/10,EditorGUIUtility.singleLineHeight,x,y*5));
		pos2=GUILayout.BeginScrollView(pos2,false,false);
		counter=0;
		foreach(EnemyInfo en in world.enemies)
		{
			GUILayout.BeginHorizontal();
				ShowSprite(GUILayoutUtility.GetRect(x/5,y),en.sprites[0].rect,en.sprites[0].texture);
				GUILayout.BeginVertical();
					GUILayout.Space(y/2);
					if(GUILayout.Button("+",GUILayout.Width(y/2),GUILayout.Height(y/2)))
						Add(counter);
				GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.Label(en.name);
			counter++;
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
#endregion
#region Shortcuts
		if(GetKeyDown(KeyCode.Alpha1))Add(0);
		if(GetKeyDown(KeyCode.Alpha2))Add(1);
		if(GetKeyDown(KeyCode.Alpha3))Add(2);
		if(GetKeyDown(KeyCode.Alpha4))Add(3);
		if(GetKeyDown(KeyCode.Alpha5))Add(4);
		float f=pos.y;
		if(GetKey(KeyCode.UpArrow))pos.y-=10;
		if(GetKey(KeyCode.DownArrow))pos.y+=10;
		if(f!=pos.y)Repaint();
#endregion
#region SubBoss
		EnemyInfo sub=world.subBoss;
		if(sub){
		if(subBossHelper!=null)subBossHelper.position=10;
		GUILayout.BeginArea(new Rect(x/10,y*8,x,y*2));
			GUILayout.BeginHorizontal();
			ShowSprite(GUILayoutUtility.GetRect(x/5,y*2),sub.sprites[0].rect,sub.sprites[0].texture);
				GUILayout.BeginVertical();
					GUILayout.Space(y/2);
					if(GUILayout.Button("+",GUILayout.Width(y/2),GUILayout.Height(y/2)) || GetKeyDown(KeyCode.Alpha0))
					{
						if(subBossHelper==null){
							subBossHelper=new EnemyHelper();
							subBossHelper.id=-1;
							subBossHelper.sprite=sub.sprites[0];
						}
						if(subEntry!=null)subEntry.enemies.Remove(subBossHelper);
						currentEntry.enemies.Add(subBossHelper);
						subEntry=currentEntry;
						currentEnemy=subBossHelper;
						Repaint();
						saveTime=EditorApplication.timeSinceStartup+2;
					}
				GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		GUILayout.EndArea();
		}
#endregion
#region Drone
		GUILayout.BeginArea(new Rect(x/10,y*6,x,y));
			GUILayout.BeginHorizontal();
			ShowSprite(GUILayoutUtility.GetRect(x/5,y*2),world.drone.sprites[0].rect,world.drone.sprites[0].texture);
				GUILayout.BeginVertical();
					GUILayout.Space(y/2);
					if(GUILayout.Button("+",GUILayout.Width(y/2),GUILayout.Height(y/2)) || GetKeyDown(KeyCode.D))
					{
						EnemyHelper eh=new EnemyHelper();
						eh.id=-2;
						eh.sprite=world.drone.bullets[0];
						currentEntry.enemies.Add(eh);
						currentEnemy=eh;
						Repaint();
						saveTime=EditorApplication.timeSinceStartup+2;
					}
				GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		GUILayout.EndArea();
#endregion
#region Entry Options
	#region Time
		GUILayout.BeginArea(new Rect(x*8,EditorGUIUtility.singleLineHeight,x*2,y*3));
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("+0.1"))currentEntry.timer++;
				if(GUILayout.Button("+0.5"))currentEntry.timer+=5;
				if(GUILayout.Button("+1"))currentEntry.timer+=10;
				if(GUILayout.Button("+5"))currentEntry.timer+=50;
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("-0.1"))currentEntry.timer--;
				if(GUILayout.Button("-0.5"))currentEntry.timer-=5;
				if(GUILayout.Button("-1"))currentEntry.timer-=10;
				if(GUILayout.Button("-5"))currentEntry.timer-=50;
				if(currentEntry.timer<1) currentEntry.timer=1;
			GUILayout.EndHorizontal();
	#endregion
	#region  Add/Remove
		if(GUILayout.Button("Addd entry")){
			counter=entries.IndexOf(currentEntry)+1;
			currentEntry=new Entry();
			currentEntry.timer=5;
			currentEnemy=null;
			entries.Insert(counter,currentEntry);
			saveTime=EditorApplication.timeSinceStartup+2;
		}
		if(entries.Count>1 && GUILayout.Button("Remove entry")){
			entries.Remove(currentEntry);
			currentEntry=null;
			saveTime=EditorApplication.timeSinceStartup+2;
		}
		GUILayout.EndArea();
	#endregion
#endregion
#region Wave preview
		lastH=(int)GUILayoutUtility.GetLastRect().height;
		fullY=FullSize();
		GUILayout.BeginArea(new Rect(x,EditorGUIUtility.singleLineHeight,x*7,y*9));
			GUI.Box(new Rect(x/2,0,x*6.5f,y*9),"");
			pos=GUI.BeginScrollView(new Rect(0,0,x*7,y*9f),pos,new Rect(new Rect(0,0,x*4f,fullY+lastH)),false,true);
				counter=0;
				tempY=0;
				entries.ForEach(DrawEntry);
			GUI.EndScrollView();
		GUILayout.EndArea();
#endregion
#region Wave Options
		GUILayout.BeginArea(new Rect(x*8,y*8,x*2,y*2));
			if(GUILayout.Button("Save") || GetKeyDown(KeyCode.S)){
				world.wave=GetWave();
				EditorUtility.SetDirty(world);
				saveTime=0;
			}
			if(GUILayout.Button("Load") || GetKeyDown(KeyCode.R)){
				Revert(world.wave);
				Repaint();
				saveTime=0;
			}
		GUILayout.EndArea();
#endregion
#region Enemy Options
		if(currentEnemy==null)return;
		GUILayout.BeginArea(new Rect(x*8,y*6,x*2,y*2));
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("<") || GetKeyDown(KeyCode.LeftArrow)){
					currentEnemy.position--;
					if(currentEnemy.position<0) currentEnemy.position=19;
					if(currentEnemy.id==-2)currentEnemy.sprite=world.drone.bullets[currentEnemy.position%3*2];
					Repaint();
					saveTime=EditorApplication.timeSinceStartup+2;
				}
				if(GUILayout.Button(">") || GetKeyDown(KeyCode.RightArrow)){
					currentEnemy.position++;
					if(currentEnemy.position>19) currentEnemy.position=0;
					if(currentEnemy.id==-2)currentEnemy.sprite=world.drone.bullets[currentEnemy.position%3*2];
					Repaint();
					saveTime=EditorApplication.timeSinceStartup+2;
				}
			GUILayout.EndHorizontal();
			if(GUILayout.Button("X") || GetKeyDown(KeyCode.X)){
				currentEntry.enemies.Remove(currentEnemy);
				currentEnemy=null;
				Repaint();
			}
		GUILayout.EndArea();
#endregion
#region AutoSave
		if(lastTime<saveTime && EditorApplication.timeSinceStartup>saveTime){
			world.wave=GetWave();
			EditorUtility.SetDirty(world);
		}
		lastTime=EditorApplication.timeSinceStartup;
#endregion
	}

	void ShowSprite(Rect r,Rect c,Texture2D t)
	{
		Rect rect = c;
		rect.x/=t.width;
		rect.y/=t.height;
		rect.width/=t.width;
		rect.height/=t.height;
		GUI.DrawTextureWithTexCoords(r,t,rect);
	}
	bool GetKey(KeyCode key){
		return Event.current.keyCode==key;
	}
	bool GetKeyDown(KeyCode key){
		if(Event.current.type==EventType.KeyDown)return GetKey(key);
		return false;
	}
	void Add(int i){
		if(i>=world.enemies.Length)return;
		EnemyHelper eh=new EnemyHelper();
		eh.id=i;
		eh.sprite=world.enemies[i].sprites[0];
		currentEntry.enemies.Add(eh);
		currentEnemy=eh;
		Repaint();
		saveTime=EditorApplication.timeSinceStartup+2;
	}
	void DrawEntry(Entry en)
	{

		float x2 =x/2f;
		float x10 =x/10f;
		float y2=y/2;
		float xw=x*0.315f;
		GUI.Label(new Rect(0,fullY-tempY,x2,lastH),en.timer/10+"."+(en.timer%10));
		tempY+=en.timer*y/10;
		if(currentEntry==en) GUI.color=Color.green;
		if(GUI.Button(new Rect(0,fullY-tempY,x2,lastH), "Entry "+counter++))
		{
			currentEntry=en;
			currentEnemy=null;
		}
		GUI.color=Color.white;
		for (int i = 0; i < 20; i++)
		{
			GUI.Box(new Rect(i*xw+x2,fullY-tempY,xw,y2),"");
		}
		foreach(EnemyHelper eh in en.enemies)
		{
			GUI.Label(new Rect(eh.position*xw+x2,fullY-tempY+y2,xw,y2),eh.position.ToString());
			if(GUI.Button(new Rect(eh.position*xw+x2,fullY-tempY,xw,y2),""))
			{
				currentEnemy=eh;
				currentEntry=en;
			}
			GUI.color=Color.cyan;
			if(eh==currentEnemy) GUI.Box(new Rect(eh.position*xw+x2,fullY-tempY,xw,y2),"");
			GUI.color=Color.white;

			ShowSprite(new Rect(eh.position*xw+x2,fullY-tempY,xw,y2),eh.sprite.rect,eh.sprite.texture);
		}
		tempY+=lastH;
	}
	int FullSize()
	{
		int i=0;
		int j=lastH;
		foreach(Entry entry in entries)
		{
			i+=entry.timer*y/10+j;
		}
		return i;
	}
	string GetWave()
	{
		string s="";

		foreach(Entry entry in entries)
		{
			int i=entry.timer/90;
			for(; i>0; i--)
			 s+="T9";
			i=entry.timer%90;
			if(i/10>0)s+="T"+i/10;
			if(i%10>0)s+="t"+i%10;

			foreach(EnemyHelper enemy in entry.enemies)
			{
				s+=(char)(enemy.id+48);
				s+=(char)(enemy.position+48);
			}
		}
		return s;
	}
	void Revert(string s)
	{
		entries.Clear();
		currentEntry=null;
		currentEntry=null;
		subEntry=null;
		subBossHelper=null;
		int j;
		int i=0;
		do
		{
			Entry entry=new Entry();
			entries.Add(entry);
			while(i<s.Length && !int.TryParse(s.Substring(i,1),out j) && s[i]!='/' && s[i]!='.'){
				if(s[i]=='T')entry.timer+=int.Parse(s.Substring(i+1,1))*10;
				if(s[i]=='t')entry.timer+=int.Parse(s.Substring(i+1,1));
				i+=2;
			}
			while(i<s.Length && (int.TryParse(s.Substring(i,1),out j) || s[i]=='/' || s[i]=='.')){
				EnemyHelper enemy=new EnemyHelper();
				entry.enemies.Add(enemy);
				enemy.position=s[i+1]-48;
				if(s[i]=='/'){
					enemy.sprite=world.subBoss.sprites[0];
					enemy.id=-1;
					subBossHelper=enemy;
					subEntry=entry;
				}else if(s[i]=='.'){
					enemy.sprite=world.drone.bullets[enemy.position%3*2];
					enemy.id=-2;
				}else{
					enemy.sprite=world.enemies[j].sprites[0];
					enemy.id=j;
				}
				i+=2;
			}
		}
		while(i<s.Length);
	}
}
