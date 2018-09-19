using System.Collections;
using System.Collections.Generic;

public struct EatLocation
{
	public string Name { get; private set; }
	public string Description { get; private set; }

	public EatLocation(string name, string desc)
	{
		Name = name;
		Description = desc;
	}


	// Static

	static EatLocation[] _presets =
	{
		new EatLocation("Arts", "Arts description"),
		new EatLocation("Business", "Arts description"),
		new EatLocation("Engine", "Arts description"),
		new EatLocation("PGP", "Arts description"),
		new EatLocation("U-Town", "Arts description"),
	};

	public static Dictionary<string, EatLocation> Presets = new Dictionary<string, EatLocation>();

	public static void Init()
	{
		foreach (var location in _presets)
		{
			Presets.Add(location.Name, location);
		}
	}
}
