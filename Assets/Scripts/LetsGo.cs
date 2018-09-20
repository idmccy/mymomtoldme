using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetsGo : MonoBehaviour
{
	[SerializeField] Button _btn = null;

	void Awake()
	{
		_btn.onClick.AddListener(() =>
		{
			Application.Quit();
		});
	}
}
