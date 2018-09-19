using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour
{
	[SerializeField] Button _btnDisconnect = null;

	void Awake()
	{
		_btnDisconnect.onClick.AddListener(OnDisconnect);
	}

	void OnDisconnect()
	{
		PlayerControl.Local.Disconnect();
	}
}
