using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotesWindow : MonoBehaviour
{
	public static VotesWindow singleton { get; private set; }

	[SerializeField] VoteEntry _voteEntryFirst = null;

	List<VoteEntry> _listEntry = new List<VoteEntry>();

	void Awake()
	{
		singleton = this;

		_voteEntryFirst.gameObject.SetActive(false);
		_listEntry.Add(_voteEntryFirst);
	}

	//public void AddVote(EatLocation location)
	//{
	//	VoteEntry entryToAdd;
	//	if (_dictEntry.TryGetValue(location.Name, out entryToAdd))
	//	{
	//	}
	//	else
	//	{
	//		if (_dictEntry.Count == 0)
	//		{
	//			entryToAdd = _voteEntryFirst;
	//		}
	//		else
	//		{
	//			entryToAdd = Instantiate(_voteEntryFirst, _voteEntryFirst.transform.parent);
	//		}
	//		_dictEntry.Add(location.Name, entryToAdd);
	//		entryToAdd.Init(location);
	//	}
	//	entryToAdd.ModifyCount(1);
	//	entryToAdd.gameObject.SetActive(true);
	//}

	//public void RemoveVote(EatLocation location)
	//{
	//	VoteEntry entryToAdd;
	//	if (_dictEntry.TryGetValue(location.Name, out entryToAdd))
	//	{
	//		entryToAdd.ModifyCount(-1);
	//	}
	//	else
	//	{
	//		Debug.LogError("No location of name " + location.Name);
	//	}
	//}

	public void RefreshVotes()
	{
		int i = 0;
		foreach (var kvp in VoteTracker.Votes)
		{
			VoteEntry entry;
			if (i < _listEntry.Count)
			{
				entry = _listEntry[i];
			}
			else
			{
				entry = Instantiate(_voteEntryFirst, _voteEntryFirst.transform.parent);
				_listEntry.Add(entry);
			}
			entry.SetLocation(kvp.Key);
			entry.UpdateCount(kvp.Value);
			entry.gameObject.SetActive(true);
			++i;
		}
		while (i < _listEntry.Count)
		{
			_listEntry[i].gameObject.SetActive(false);
			++i;
		}
	}
}
