using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalResult : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI _txt = null;

	public void SetLocation(string location)
	{
		_txt.text = location;
	}
}
