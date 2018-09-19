using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
	[SerializeField] Text _txt = null;

	static DebugText singleton;

	void Awake()
	{
		singleton = this;
	}

	void OnDestroy()
	{
		singleton = null;
	}

	public static void Log(string text)
	{
		singleton._txt.text = text;
	}
}
