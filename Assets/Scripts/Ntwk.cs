using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class Ntwk : NetworkManager
{
	const string MATCH_NAME = "MATCH";
	const int REQUEST_DOMAIN = 1111;
	const int ELO_SCORE = 0;

	void Awake()
	{
		StartMatchMaker();
	}

	void Start()
	{
		Begin();
	}

	void Begin()
	{
		matchMaker.ListMatches(0, 10, "", false, ELO_SCORE, REQUEST_DOMAIN, OnMatchList);
	}

	public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
	{
		base.OnMatchList(success, extendedInfo, matches);

		if (success)
		{
			if (matches.Count == 0) // no match available
			{
				matchMaker.CreateMatch(MATCH_NAME, 10, true, "", "", "", ELO_SCORE, REQUEST_DOMAIN, OnMatchCreate);
			}
			else
			{
				print("Found " + matches.Count);
				matchMaker.JoinMatch(matches[0].networkId, "", "", "", ELO_SCORE, REQUEST_DOMAIN, OnMatchJoined);
			}
		}
		else
		{
			DebugText.Log("FAILED OnMatchList: " + extendedInfo);
		}
	}

	public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
	{
		base.OnMatchCreate(success, extendedInfo, matchInfo);

		if (success)
		{
			DebugText.Log("Match created!");
		}
		else
		{
			DebugText.Log("FAILED OnMatchCreate: " + extendedInfo);
		}
	}

	public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
	{
		base.OnMatchJoined(success, extendedInfo, matchInfo);

		if (success)
		{
			DebugText.Log("Match joined!");
			if (numPlayers == 0)
			{
				//DebugText.Log("No one here, start match!");
			}
		}
		else
		{
			DebugText.Log("FAILED OnMatchJoined: " + extendedInfo);
		}
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);

		DebugText.Log("Disconnected! Reconnecting...");

		StopMatchMaker();
		StartMatchMaker();
		Begin();
	}
}
