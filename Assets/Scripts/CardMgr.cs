using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMgr : MonoBehaviour
{
	const int TOTAL_CARDS = 0;
	[SerializeField] List<CardDecision> _listCard = new List<CardDecision>();

	Stack<CardDecision> _stackCard = new Stack<CardDecision>();

	int _countNo = 0;

	void Awake()
	{
		EatLocation.Init();
	}

	void Start()
	{
		_listCard[0].Setup(EatLocation.Presets["Arts"]);
		_listCard[1].Setup(EatLocation.Presets["Engine"]);
		_listCard[2].Setup(EatLocation.Presets["PGP"]);

		_stackCard.Push(_listCard[2]);
		_stackCard.Push(_listCard[1]);
		_stackCard.Push(_listCard[0]);
	}

	public void SwipeAgree()
	{
		if (_stackCard.Count > 0)
		{
			print("AGREE");
			var card = _stackCard.Pop();
			if (_listCard.Contains(card))
			{
				_listCard.Remove(card);
			}
			PlayerControl.Local.SendVote(card.Location.Name);
			StartCoroutine(AnimateRight(card.transform));
		}
	}

	public void SwipeNoPreference()
	{
		if (_stackCard.Count > 0)
		{
			print("NO PREFERENCE");
			var card = _stackCard.Pop();
			if (_listCard.Contains(card))
			{
				++_countNo;
				_listCard.Remove(card);
			}
			//PlayerControl.Local.SendVote(card.Location.Name, false);
			StartCoroutine(AnimateLeft(card.transform));
		}
	}

	public void SwipeYouDecide()
	{
		if (_stackCard.Count > 0)
		{
			print("YOU DECIDE");
			while (_stackCard.Count > 0)
			{
				var card = _stackCard.Pop();
				if (_listCard.Contains(card))
				{
					_listCard.Remove(card);
				}
				StartCoroutine(AnimateDown(card.transform));
			}
			PlayerControl.Local.SendAbstain();
		}
	}

	const float SPEED = 5000;
	const float OFFSET = 2000;
	IEnumerator AnimateLeft(Transform trfCard)
	{
		float moved = 0;
		while (moved < OFFSET)
		{
			var delta = Time.deltaTime * SPEED;
			moved += delta;
			var pos = trfCard.position;
			pos.x -= delta;
			trfCard.position = pos;

			yield return null;
		}
		CloseIfLastCard();
	}

	IEnumerator AnimateRight(Transform trfCard)
	{
		float moved = 0;
		while (moved < OFFSET)
		{
			var delta = Time.deltaTime * SPEED;
			moved += delta;
			var pos = trfCard.position;
			pos.x += delta;
			trfCard.position = pos;

			yield return null;
		}
		CloseIfLastCard();
	}

	IEnumerator AnimateDown(Transform trfCard)
	{
		float moved = 0;
		while (moved < OFFSET)
		{
			var delta = Time.deltaTime * SPEED;
			moved += delta;
			var pos = trfCard.position;
			pos.y -= delta;
			trfCard.position = pos;

			yield return null;
		}
		CloseIfLastCard();
	}

	void CloseIfLastCard()
	{
		if (_stackCard.Count == 0)
		{
			gameObject.SetActive(false);
		}

		if (_listCard.Count == 0)
		{
			if (_countNo == TOTAL_CARDS)
			{
				// SUGGEST
			}
			else
			{
				PlayerControl.Local.CompleteChoice();
			}
		}
	}
}
