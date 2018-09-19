using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct EatLocation
{
	public string Name;
	public string Description;
	public Sprite Sprite;

	public EatLocation(string name, string desc, Sprite sprite = null)
	{
		Name = name;
		Description = desc;
		Sprite = sprite;
	}


	// Static

	public static Dictionary<string, EatLocation> Presets = new Dictionary<string, EatLocation>();

	public static void Init(EatLocation[] presets)
	{
		foreach (var location in presets)
		{
			Presets.Add(location.Name, location);
		}
	}
}
