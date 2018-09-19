using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotGoing : MonoBehaviour
{
	void Start()
	{
		Ntwk.singleton.StopClient();
	}
}
