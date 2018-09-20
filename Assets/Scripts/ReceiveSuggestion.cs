using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveSuggestion : MonoBehaviour
{
	[SerializeField] CardDecision _card = null;
	[SerializeField] CardMgr _cardMgr = null;

	static ReceiveSuggestion _instance;

	void Awake()
	{
		_instance = this;
		gameObject.SetActive(false);
	}

	void OnDestroy()
	{
		_instance = null;
	}

	public static void ShowSuggestion(EatLocation location)
	{
		_instance._card.Setup(location);
		_instance._card.IsSuggestion = true;
		_instance._cardMgr.AddSuggestion(_instance._card);
		_instance.gameObject.SetActive(true);
	}

}
