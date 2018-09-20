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
	public List<string> MamaSays;

	public EatLocation(string name, string desc, Sprite sprite = null, List<string> mamaSays = null)
	{
		Name = name;
		Description = desc;
		Sprite = sprite;
		MamaSays = (mamaSays == null) ? new List<string>() : mamaSays;
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
