using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Core : MonoBehaviour {
	private SpriteRenderer render;
	public Color color;
	public Color white=Color.white;
	private float value;
	public void Start()
	{
		render=gameObject.GetComponent<SpriteRenderer>();
		render.color=color;
	}
	public Core Set(Color w,Color c){
		white=w;
		color=c;
		render=GetComponent<SpriteRenderer>();
		render.color=c;
		return this;
	}
	public Core Set(Sprite s,Color c)
	{
		render=gameObject.AddComponent<SpriteRenderer>();
		render.sprite=s;
		render.color=c;
		color=c;
		return this;
	}
	public Core Flip()
	{
		render.flipX=true;
		return this;
	}
	public void Add(float f)
	{
		if(value<1)
		{
			value+=f;
			render.color=Color.Lerp(color,white,value);
		}
	}
	public void Min(float f)
	{
		if(value>0)
		{
			value-=f;
			render.color=Color.Lerp(color,white,value);
		}
	}
	public float Value(){
		return value;
	}
	public void Set(float f)
	{
		if(f!=value)
		{
			value=f;
			render.color=Color.Lerp(color,white,value);
		}
	}
}
