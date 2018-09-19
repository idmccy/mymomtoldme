using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confirm : MonoBehaviour
{
	[SerializeField] Button _btn = null;
	
	void Awake()
	{
		_btn.onClick.AddListener(() =>
		{
			PlayerControl.Local.CompleteChoice();
			gameObject.SetActive(false);
		});
	}
}
