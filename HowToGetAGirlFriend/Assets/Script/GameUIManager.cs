using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour 
{
	//To be change this should be spawned and handle by Game Manager not a singleton.
	private static GameUIManager instance;
	public static GameUIManager Instance { get { return instance; } }
	[SerializeField] GameObject popupWingMan;
	[SerializeField] GameObject popupTestLuck;
	[SerializeField] Text killCountText;
	[SerializeField] Text waveCountText;
	[SerializeField] Text speedText;
	[SerializeField] Text resultText;


	private string[] winReply = {
		"Yay, Nag reply siya",
		"Ayiee... natawa siya sa joke mo",
		"Tapos na, pakasalan mo na",
		"You got a GF (Shiva)",
		"Yay",
		"Wala na ako maisip basta nice.",
		"Yan na asawa mo na",
		"Gwapo mo naman..",
	};

	private string[] loseReply = {
		"Seenzoned",
		"Who you?",
		"Ay, Strict parents ko eh",
		"..... :'(",
		"How could this happen to me~",
		"haha.. ha.. huhu...",
		"May 2D girls pa naman eh",
		"its okay..",
		"Stickfigure lang yan, its okay."
	};
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
			GameManager.Instance.MainPlayerAttack ();
			GameManager.Instance.PlayerAttack ();
		}
	}

	public void SpeedPress()
	{
		if (GameManager.Instance != null) 
		{
			GameManager.Instance.ChangeDivider ();
		}
	}

	public void BackButton()
	{
		if (popupWingMan != null)
			popupWingMan.gameObject.SetActive (false);
		
		if (popupTestLuck != null)
			popupTestLuck.gameObject.SetActive (false);
	}

	public void OpenWingMan()
	{
		if (popupWingMan != null)
			popupWingMan.gameObject.SetActive (true);
	}

	public void TestLuck()
	{
		if (popupTestLuck != null)
			popupTestLuck.gameObject.SetActive (true);

		if (GameManager.Instance != null) 
		{
			GameManager.Instance.divider = 1;
		}
	}

	public void LuckButtonPressed()
	{
		int range = 1;
		if (GameManager.Instance.PlayerData ().KillCount < 100)
			range = 101;
		else
			range = (int)GameManager.Instance.PlayerData ().KillCount + 25;

		int luck = Random.Range (0, range);
		if (luck <= GameManager.Instance.PlayerData ().KillCount) 
		{
			int index = Random.Range (0, winReply.Length);
			if(resultText != null)
				resultText.text = winReply[index];
		} 
		else
		{
			int index = Random.Range (0, loseReply.Length);
			if(resultText != null)
				resultText.text = loseReply[index];
		}
	}

	public void UpdateUI()
	{
		if(killCountText != null)
			killCountText.text = string.Format("Confidence: {0}", GameManager.Instance.PlayerData().KillCount);
		if(waveCountText != null)
			waveCountText.text = string.Format("Level: {0}", GameManager.Instance.PlayerData().WaveCount);
		if(speedText != null)
			speedText.text = string.Format("x{0}", GameManager.Instance.divider);
	}
}
