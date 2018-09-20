using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveSuggestion : MonoBehaviour
{
	[SerializeField] CardDecision _card = null;

	public static Queue<EatLocation> QueueLoc = new Queue<EatLocation>();
	static ReceiveSuggestion _instance;

	Vector3 _cardstartPos;

	void Awake()
	{
		_instance = this;
		_cardstartPos = _card.transform.position;
		gameObject.SetActive(false);
	}

	void OnDestroy()
	{
		_instance = null;
		QueueLoc.Clear();
	}

	void ResetCardPos()
	{
		_card.transform.position = _cardstartPos;
	}

	public static void ShowSuggestion(EatLocation location)
	{
		if (!_instance.gameObject.activeInHierarchy)
		{
			_instance._card.IsSuggestion = true;
			CardMgr.singleton.AddSuggestion(_instance._card);
			_instance.ResetCardPos();
			_instance._card.Setup(location);
			_instance.gameObject.SetActive(true);
		}
		else
		{
			QueueLoc.Enqueue(location);
		}
	}

}
