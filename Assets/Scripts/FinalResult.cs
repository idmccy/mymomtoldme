using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalResult : MonoBehaviour
{
	[SerializeField] Text _txt = null;

	static FinalResult _instance;

	void Awake()
	{
		_instance = this;
		gameObject.SetActive(false);
	}

	void OnDestroy()
	{
		_instance = null;
	}

	public static void SetLocation(string location)
	{
		_instance._txt.text = location;
		_instance.gameObject.SetActive(true);
		Ntwk.singleton.StopClient();
	}
}
