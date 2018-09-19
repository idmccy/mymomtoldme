using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoterCounter : MonoBehaviour
{
	[SerializeField] Text _txtCurr = null;
	[SerializeField] Text _txtMax = null;

	int _curr = 0;
	int _max = 0;

	static VoterCounter _instance;

	void Awake()
	{
		_instance = this;
		SetCurr(0);
		SetMax(0);
	}

	void OnDestroy()
	{
		_instance = null;
	}

	public static void ModifyCurr(int modify)
	{
		SetCurr(_instance._curr + modify);
	}

	public static void ModifyMax(int modify)
	{
		if (_instance == null) return;

		SetMax(_instance._max + modify);
	}

	public static void SetCurr(int curr)
	{
		_instance._curr = curr;
		_instance._txtCurr.text = _instance._curr.ToString();
	}

	public static void SetMax(int max)
	{
		_instance._max = max;
		_instance._txtMax.text = _instance._max.ToString();
	}
}