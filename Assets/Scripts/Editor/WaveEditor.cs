using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaveEditor : EditorWindow
{
	WorldInfo world;
	List<Entry> entries=new List<Entry>();
	Entry currentEntry;
	EnemyHelper currentEnemy;
	int x,y;
	int counter;
	int fullY;
	int tempY;
	int lastH;
	Vector2 pos;
	[MenuItem("Window/Wave Editor")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(WaveEditor));
	}
	void OnGUI()
	{
		WorldInfo w=world;
		world=EditorGUILayout.ObjectField("World",world,typeof(WorldInfo),false)as WorldInfo;
		if(w!=world && world!=null)Revert(world.wave);
		if(world==null)return;
		x=Screen.width/10;
		y=Screen.height/10;
		if(entries.Count==0 && !string.IsNullOrEmpty(world.wave))
			Revert(world.wave);
		if(currentEntry==null)
		{
			if(entries.Count==0) entries.Add(new Entry());
			currentEntry=entries[0];
		}
		GUILayout.BeginArea(new Rect(x/10,y/2,x/2,y*2*world.enemies.Length));
		counter=0;
		foreach(EnemyInfo en in world.enemies)
		{
			GUILayout.BeginHorizontal();
				ShowSprite(GUILayoutUtility.GetRect(x/5,y),en.sprites[0].rect,en.sprites[0].texture);
				GUILayout.BeginVertical();
					GUILayout.Space(y/2);
					if(GUILayout.Button("+",GUILayout.Width(y/2),GUILayout.Height(y/2)))
					{
						EnemyHelper eh=new EnemyHelper();
						eh.id=counter;
						eh.sprite=en.sprites[0];
						currentEntry.enemies.Add(eh);
						currentEnemy=eh;
					}
				GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.Label(en.name);
			counter++;
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(x*5.4f,y/2,x*1.2f,y*2f));
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("+0.1"))currentEntry.timer++;
				if(GUILayout.Button("+1"))currentEntry.timer+=10;
				if(GUILayout.Button("+5"))currentEntry.timer+=50;
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("-0.1"))currentEntry.timer--;
				if(GUILayout.Button("-1"))currentEntry.timer-=10;
				if(GUILayout.Button("-5"))currentEntry.timer-=50;
				if(currentEntry.timer<1) currentEntry.timer=1;
			GUILayout.EndHorizontal();

		if(GUILayout.Button("Addd entry")){
			counter=entries.IndexOf(currentEntry)+1;
			currentEntry=new Entry();
			currentEntry.timer=5;
			entries.Insert(counter,currentEntry);
		}
		if(entries.Count>1 && GUILayout.Button("Remove entry")){
			entries.Remove(currentEntry);
			currentEntry=null;
		}
		GUILayout.EndArea();
		lastH=(int)GUILayoutUtility.GetLastRect().height;
		fullY=FullSize();
		GUILayout.BeginArea(new Rect(x*0.8f,y/2,x*4.5f,y*5.5f));
			GUI.Box(new Rect(x*0.6f,0,x*3.7f,y*5.5f),"");
			pos=GUI.BeginScrollView(new Rect(0,0,x*4.5f,y*5.5f),pos,new Rect(new Rect(0,0,x*4f,fullY+lastH)),false,true);
				counter=0;
				tempY=0;
				entries.ForEach(DrawEntry);
			GUI.EndScrollView();
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(x*5.4f,y*5,x*1.2f,y));
			if(GUILayout.Button("Save"))world.wave=GetWave();
			if(GUILayout.Button("Load"))Revert(world.wave);
		GUILayout.EndArea();
		if(currentEnemy==null)return;
		GUILayout.BeginArea(new Rect(x*5.5f,y*3,x,y*1.5f));
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("<")){
					currentEnemy.position--;
					if(currentEnemy.position<0) currentEnemy.position=19;
				}
				if(GUILayout.Button(">")){
					currentEnemy.position++;
					if(currentEnemy.position>19) currentEnemy.position=0;
				}
			GUILayout.EndHorizontal();
			if(GUILayout.Button("X")){
				currentEntry.enemies.Remove(currentEnemy);
				currentEnemy=null;
			}
		GUILayout.EndArea();
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

	void DrawEntry(Entry en)
	{

		int w =x/2;
		int h = lastH;
		GUI.Label(new Rect(0,fullY-tempY,w,h),en.timer/10+"."+(en.timer%10));
		tempY+=en.timer*y/10;
		if(currentEntry==en) GUI.color=Color.green;
		if(GUI.Button(new Rect(0,fullY-tempY,w,h), "Entry "+counter++))
		{
			currentEntry=en;
			currentEnemy=null;
		}
		GUI.color=Color.white;
		foreach(EnemyHelper eh in en.enemies)
		{
			if(GUI.Button(new Rect(eh.position*x/5+x*0.6f,fullY-tempY,x/10,y/2),""))
			{
				currentEnemy=eh;
				currentEntry=en;
			}
			GUI.color=Color.cyan;
			if(eh==currentEnemy) GUI.Box(new Rect(eh.position*x/5+x*0.6f,fullY-tempY,x/10,y/2),"");
			GUI.color=Color.white;

			ShowSprite(new Rect(eh.position*x/5+x*0.6f,fullY-tempY,x/10,y/2),eh.sprite.rect,eh.sprite.texture);
		}
		tempY+=lastH;
		return;

		if(currentEntry==en) GUI.color=Color.green;
		if(GUILayout.Button("Entry "+counter++,GUILayout.Width(x/2))){
			currentEntry=en;
			currentEnemy=null;
		}
		GUI.color=Color.white;
		foreach(EnemyHelper eh in en.enemies)
		{
			if(GUI.Button(new Rect(eh.position*x/5+x*0.6f,GUILayoutUtility.GetLastRect().y,x/10,y/2),"")){
				currentEnemy=eh;
				currentEntry=en;
			}
			GUI.color=Color.cyan;
			if(eh==currentEnemy)GUI.Box(new Rect(eh.position*x/5+x*0.6f,GUILayoutUtility.GetLastRect().y,x/10,y/2),"");
			GUI.color=Color.white;

			ShowSprite(new Rect(eh.position*x/5+x*0.6f,GUILayoutUtility.GetLastRect().y,x/10,y/2),eh.sprite.rect,eh.sprite.texture);
		}
		GUILayout.Space(en.timer*y/10);
		GUILayout.Label(en.timer/10+"."+(en.timer%10));

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
				s+=enemy.id.ToString();
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
		int j;
		int i=0;
		do
		{
			Entry entry=new Entry();
			entries.Add(entry);
			while(i<s.Length && !int.TryParse(s.Substring(i,1),out j)){
				if(s[i]=='T')entry.timer+=int.Parse(s.Substring(i+1,1))*10;
				if(s[i]=='t')entry.timer+=int.Parse(s.Substring(i+1,1));
				i+=2;
			}
			while(i<s.Length && int.TryParse(s.Substring(i,1),out j)){
				EnemyHelper enemy=new EnemyHelper();
				entry.enemies.Add(enemy);
				enemy.sprite=world.enemies[j].sprites[0];
				enemy.position=s[i+1]-48;
				enemy.id=j;
				i+=2;
			}
		}
		while(i<s.Length);
	}
}
