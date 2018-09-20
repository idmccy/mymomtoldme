using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeSuggestion : MonoBehaviour
{
	[SerializeField] InputField _inputName = null;
	[SerializeField] InputField _inputDesc = null;
	[SerializeField] Button _btnSuggest = null;
	[SerializeField] Button _btnSkip = null;
	[SerializeField] GameObject _gobWarning = null;

	void Awake()
	{
		_btnSuggest.onClick.AddListener(() =>
		{
			if (_inputName.text != "" && _inputDesc.text != "")
			{
				PlayerControl.Local.SendSuggestion(_inputName.text, _inputDesc.text);
				gameObject.SetActive(false);
			}
			else
			{
				_gobWarning.SetActive(true);
			}
			PlayerControl.Local.CompleteChoice();
		});
		_btnSkip.onClick.AddListener(() =>
		{
			PlayerControl.Local.CompleteChoice();
		});
		gameObject.SetActive(false);
	}

}
