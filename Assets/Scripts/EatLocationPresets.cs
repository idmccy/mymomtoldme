using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatLocationPresets : MonoBehaviour
{
	[SerializeField] EatLocation[] _locations = null;

	void Awake()
	{
		EatLocation.Init(_locations);
	}
}
