using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMenu : MonoBehaviour 
{
	private float offset;
	[SerializeField]
	private RectTransform toFollow;
	private RectTransform rect;

	// Use this for initialization
	void Start () 
	{
		rect = GetComponent<RectTransform>();
		offset = rect.localPosition.x -toFollow.localPosition.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 v = rect.localPosition;
		v.x = toFollow.localPosition.x + offset;
		rect.localPosition = v;
	}
}
