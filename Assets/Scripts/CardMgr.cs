using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMgr : MonoBehaviour
{
	public static CardMgr singleton { get; private set; }

	[SerializeField] List<CardDecision> _listCard = new List<CardDecision>();
	[SerializeField] Image _imgBg = null;
	[SerializeField] MakeSuggestion _makeSuggestion = null;
	[SerializeField] ReceiveSuggestion _receiveSuggestion = null;
	[SerializeField] Animator _animatorNotification = null;
	[SerializeField] Text _txtNotification = null;
	[SerializeField] Text _txtTimer = null;
	[SerializeField] Image _imgFill = null;

	Coroutine _coroutine;
	Stack<CardDecision> _stackCard = new Stack<CardDecision>();

	int _initialNumCards = 0;
	int _countNo = 0;
	bool _isAnimating = false;
	bool _hasInit = false;
	bool _done = false;

	void Awake()
	{
		singleton = this;
	}

	void OnDestroy()
	{
		singleton = null;
	}

	public static void Shuffle<T>(IList<T> list, int seed)
	{
		int n = list.Count;
		var rng = new System.Random(seed);
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public void InitCards(string strInit)
	{
		if (_hasInit) return;
		_hasInit = true;
		DebugText.Log("Init Cards " + strInit);
		int i = 0;
		var listLoc = new List<EatLocation>(EatLocation.Presets.Values);
		var listIndex = new List<int>();

		string[] tok = strInit.Split('|');
		foreach (string s in tok)
		{
			listIndex.Add(int.Parse(s));
		}

		foreach (var index in listIndex)
		{
			_listCard[i].Setup(listLoc[index]);
			++i;
			if (i >= _listCard.Count) break;
		}

		_stackCard.Push(_listCard[2]);
		_stackCard.Push(_listCard[1]);
		_stackCard.Push(_listCard[0]);

		_initialNumCards = _listCard.Count;

		_coroutine = StartCoroutine(DoTimer());
	}

	public void SwipeAgree()
	{
		if (_isAnimating) return;
		if (_stackCard.Count > 0)
		{
			print("AGREE");
			StopCoroutine(_coroutine);
			var card = _stackCard.Pop();
			if (_listCard.Contains(card))
			{
				_listCard.Remove(card);
			}
			PlayerControl.Local.SendVote(card.Location.Name, card.IsSuggestion);
			StartCoroutine(AnimateRight(card.transform));
		}
	}

	public void SwipeNoPreference()
	{
		if (_isAnimating) return;
		if (_stackCard.Count > 0)
		{
			print("NO PREFERENCE");
			StopCoroutine(_coroutine);
			var card = _stackCard.Pop();
			if (_listCard.Contains(card))
			{
				++_countNo;
				_listCard.Remove(card);
			}
			if (card.IsSuggestion)
			{
				PlayerControl.Local.IgnoreSuggestion();
			}
			StartCoroutine(AnimateLeft(card.transform));
		}
	}

	public void SwipeYouDecide()
	{
		if (_isAnimating) return;
		if (_listCard.Count > 0 && _stackCard.Count > 0)
		{
			while (_stackCard.Count > 0)
			{
				var card = _stackCard.Pop();
				if (card.IsSuggestion) return; // can't "skip" suggestion cards
				if (_listCard.Contains(card))
				{
					_listCard.Remove(card);
				}
				StartCoroutine(AnimateDown(card.transform));
			}
			print("YOU DECIDE");
			StopCoroutine(_coroutine);
			//PlayerControl.Local.SendAbstain();
		}
	}

	const float SPEED = 5000;
	const float OFFSET = 2000;
	IEnumerator AnimateLeft(Transform trfCard)
	{
		_isAnimating = true;
		_txtNotification.text = "No Preference";
		_animatorNotification.SetTrigger("FadeIn");
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
		yield return new WaitForSeconds(0.5f);
		_animatorNotification.SetTrigger("FadeOut");
		yield return new WaitForSeconds(0.5f);
		CloseIfLastCard();
		_isAnimating = false;

		_coroutine = StartCoroutine(DoTimer());
	}

	IEnumerator AnimateRight(Transform trfCard)
	{
		_isAnimating = true;
		_txtNotification.text = "I Agree!";
		_animatorNotification.SetTrigger("FadeIn");
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
		yield return new WaitForSeconds(0.5f);
		_animatorNotification.SetTrigger("FadeOut");
		yield return new WaitForSeconds(0.5f);
		CloseIfLastCard();
		_isAnimating = false;

		_coroutine = StartCoroutine(DoTimer());
	}

	IEnumerator AnimateDown(Transform trfCard)
	{
		_isAnimating = true;
		_txtNotification.text = "You Decide";
		_animatorNotification.SetTrigger("FadeIn");
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
		yield return new WaitForSeconds(0.5f);
		_animatorNotification.SetTrigger("FadeOut");
		yield return new WaitForSeconds(0.5f);
		CloseIfLastCard();
		_isAnimating = false;

		_coroutine = StartCoroutine(DoTimer());
	}

	void CloseIfLastCard()
	{
		_receiveSuggestion.gameObject.SetActive(false);
		if (_stackCard.Count == 0)
		{
			_imgBg.enabled = false;
			for (var i = 0; i < transform.childCount; ++i)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
		else
		{
			if (ReceiveSuggestion.QueueLoc.Count > 0)
			{
				_receiveSuggestion.gameObject.SetActive(false);
				ReceiveSuggestion.ShowSuggestion(ReceiveSuggestion.QueueLoc.Dequeue());
			}
		}

		if (_done) return;
		print(_listCard.Count);
		if (_listCard.Count == 0)
		{
			_done = true;
			print("_countNo " + _countNo);
			if (_countNo == _initialNumCards)
			{
				_makeSuggestion.gameObject.SetActive(true);
			}
			else
			{
				PlayerControl.Local.CompleteChoice();
			}
		}
	}

	public void AddSuggestion(CardDecision card)
	{
		_stackCard.Push(card);
	}

	IEnumerator DoTimer()
	{
		const float TIMEOUT = 20;
		float ctr = TIMEOUT;

		while (ctr > 0)
		{
			ctr -= Time.deltaTime;

			_txtTimer.text = Mathf.CeilToInt(ctr).ToString();
			_imgFill.fillAmount = (ctr / TIMEOUT);

			yield return null;
		}

		SwipeNoPreference();
	}
}
