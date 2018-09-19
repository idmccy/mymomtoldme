using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteEntry : MonoBehaviour
{
	[SerializeField] Text _txtLocation = null;
	[SerializeField] Text _txtVoteCount = null;

	public int Count { get; private set; }

	public void SetLocation(string locationName)
	{
		_txtLocation.text = locationName;
	}

	public void UpdateCount(int count)
	{
		Count = Mathf.Clamp(count, 0, int.MaxValue);
		_txtVoteCount.text = Count.ToString();
	}

	public void ModifyCount(int modify)
	{
		UpdateCount(Count + modify);
	}

}
