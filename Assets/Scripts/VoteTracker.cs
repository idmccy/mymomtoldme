using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VoteTracker : NetworkBehaviour
{
	[SyncVar]
	string voteString = "";

	Dictionary<string, int> _votes = new Dictionary<string, int>();

	void Awake()
	{
		_instance = this;
	}

	public override void OnStartClient()
	{
		if (voteString.Length > 0)
		{
			string[] entries = voteString.Split(DELIMITER_ENTRY);
			if (entries.Length > 0)
			{
				foreach (var entry in entries)
				{
					string[] kvp = entry.Split(DELIMITER_KEY);
					int voteCount = -1;
					int.TryParse(kvp[1], out voteCount);
					_votes.Add(kvp[0], voteCount);
				}
			}
		}
		VotesWindow.singleton.RefreshVotes();
	}

	void OnDestroy()
	{
		_instance = null;
	}

	static VoteTracker _instance;

	public static Dictionary<string, int> Votes { get { return _instance._votes; } }

	public static void AddVote(EatLocation location)
	{
		if (_instance._votes.ContainsKey(location.Name))
		{
			++_instance._votes[location.Name];
		}
		else
		{
			_instance._votes.Add(location.Name, 1);
		}
		UpdateVoteString();
	}

	public static void RemoveVote(EatLocation location)
	{
		if (_instance._votes.ContainsKey(location.Name))
		{
			--_instance._votes[location.Name];
		}
		else
		{
			Debug.LogError("No such location: " + location.Name);
		}
		UpdateVoteString();
	}

	const char DELIMITER_KEY = '`';
	const char DELIMITER_ENTRY = '|';
	static void UpdateVoteString()
	{
		string finalString = "";
		foreach (var kvp in _instance._votes)
		{
			finalString += kvp.Key + DELIMITER_KEY + kvp.Value + DELIMITER_ENTRY;
		}
		if (finalString != "")
		{
			finalString = finalString.Substring(0, finalString.Length - 1); // remove last delimiter
		}
		if (_instance.voteString != finalString)
		{
			_instance.voteString = finalString;
			VotesWindow.singleton.RefreshVotes();
			print(finalString);
		}
	}
}