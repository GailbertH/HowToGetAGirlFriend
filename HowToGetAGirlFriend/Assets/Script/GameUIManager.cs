using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour 
{
	//To be change this should be spawned and handle by Game Manager not a singleton.
	private static GameUIManager instance;
	public static GameUIManager Instance { get { return instance; } }

	[SerializeField] Text killCountText;
	[SerializeField] Text waveCountText;
	void Awake()
	{
		instance = this;
	}

	public void ButtPress()
	{
		Vector3 input = Input.mousePosition;
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++) 
			{
				if(Input.touchCount < i)
					return;

				Input.GetTouch (i);
				Vector3 pos = new Vector3 (Input.GetTouch (i).position.x, Input.GetTouch (i).position.y, 0);
			}
		}
		if (GameManager.Instance != null) 
		{
			Debug.Log ("HELLO");
		}
	}
}
