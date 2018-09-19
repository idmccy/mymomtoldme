using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
	public static PlayerControl Local { get; private set; }

	void Start()
	{
		VoterCounter.ModifyMax(1);
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		Local = this;
	}

	void OnDestroy()
	{
		if (isLocalPlayer)
		{
			Local = null;
		}
		else
		{
		}
		VoterCounter.ModifyMax(-1);
	}

	public void Disconnect()
	{
		Ntwk.singleton.StopClient();
	}

	public void SendVote(string s)
	{
		CmdSendVote(s);
	}

	[Command]
	void CmdSendVote(string s)
	{
		RpcSendVote(s);
	}

	[ClientRpc]
	void RpcSendVote(string s)
	{
		EatLocation location;

		if (EatLocation.Presets.TryGetValue(s, out location))
		{
			VoteTracker.AddVote(location);
		}
		else
		{
			VoteTracker.AddVote(s);
		}

		VotesWindow.singleton.RefreshVotes();
	}

	public void SendSuggestion(string s, string desc)
	{
		CmdSendSuggestion(s, desc);
	}

	[Command]
	void CmdSendSuggestion(string s, string desc)
	{
		RpcSendSuggestion(s, desc);
	}

	[ClientRpc]
	void RpcSendSuggestion(string s, string desc)
	{
		EatLocation location;
		if (!EatLocation.Presets.TryGetValue(s, out location))
		{
			location = new EatLocation(s, desc);
		}
		VoteTracker.AddVote(location);
		VotesWindow.singleton.RefreshVotes();

		if (!isLocalPlayer)
		{
			ReceiveSuggestion.ShowSuggestion(location);
		}
	}

	//public void SendAbstain()
	//{
	//	CmdSendAbstain();
	//}

	//[Command]
	//void CmdSendAbstain()
	//{
	//	RpcAbstain();
	//}

	//[ClientRpc]
	//void RpcAbstain()
	//{
	//}

	public void CompleteChoice()
	{
		CmdCompleteChoice();
	}

	[Command]
	void CmdCompleteChoice()
	{
		RpcCompleteChoice();
	}

	[ClientRpc]
	void RpcCompleteChoice()
	{
		VoterCounter.ModifyCurr(1);
	}

	public void FinalDecision()
	{
		CmdFinalDecision();
	}

	[Command]
	void CmdFinalDecision()
	{
		List<string> listBest = new List<string>();
		int highest = -1;
		foreach (var kvp in VoteTracker.Votes)
		{
			if (kvp.Value > highest)
			{
				listBest.Clear();
				listBest.Add(kvp.Key);
			}
			else if (kvp.Value == highest)
			{
				listBest.Add(kvp.Key);
			}
		}
		string finalLocation;
		if (listBest.Count == 0)
		{
			listBest.AddRange(EatLocation.Presets.Keys);
		}
		finalLocation = listBest[Random.Range(0, listBest.Count)];
		RpcFinalDecision(finalLocation);
	}

	[ClientRpc]
	void RpcFinalDecision(string decision)
	{
		StartCoroutine(WaitBeforeDisplayFinalResults(decision));
	}

	IEnumerator WaitBeforeDisplayFinalResults(string decision)
	{
		yield return new WaitForSeconds(3);

		FinalResult.SetLocation(decision);
	}
}
