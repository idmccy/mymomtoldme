using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDecision : MonoBehaviour
{
	[SerializeField] Image _img = null;
	[SerializeField] Text _txtName = null;
	[SerializeField] Text _txtDesc = null;

	public EatLocation Location { get; private set; }

	public void Setup(EatLocation location)
	{
		Location = location;
		_txtName.text = location.Name;
		_txtDesc.text = location.Description;
		if (location.Sprite != null)
		{
			_img.sprite = location.Sprite;
		}
	}
}
