﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharLock : MonoBehaviour {

	[SerializeField]
	private int charId;

	private Button button;

	void Start()
	{
		button = GetComponent<Button>();
	}
	void Update()
	{
	}
}
