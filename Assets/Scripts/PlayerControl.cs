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
		//if (upvote)
		//{
			if (!EatLocation.Presets.TryGetValue(s, out location))
			{
				location = new EatLocation(s, "no description");
			}
			VoteTracker.AddVote(location);
		//}
		//else
		//{
		//	var subName = s.Substring(0, s.Length - 2);
		//	if (!EatLocation.Presets.TryGetValue(subName, out location))
		//	{
		//		location = new EatLocation(subName);
		//	}
		//	VoteTracker.RemoveVote(location);
		//}
		VotesWindow.singleton.RefreshVotes();
	}

	public void SendAbstain()
	{
		CmdSendAbstain();
	}

	[Command]
	void CmdSendAbstain()
	{
		RpcAbstain();
	}

	[ClientRpc]
	void RpcAbstain()
	{
	}

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
}
