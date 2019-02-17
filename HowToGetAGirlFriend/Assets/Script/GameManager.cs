using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	private static GameManager instance;
	public static GameManager Instance { get { return instance; } }
	private GameStateMachine stateMachine;
	public GameStateMachine StateMachine { get { return this.stateMachine; } }
	private Coroutine updateRoutine;
	private bool isReviving = false;
	private PlayerProgressData playerData = new PlayerProgressData();

	private List<EnemyController> deadEnemy = new List<EnemyController> ();
	private List<PlayerController> deadPlayer = new List<PlayerController> ();

	[SerializeField] private float gameSpeed = 1;
	public int divider = 1;

	public PlayerProgressData PlayerData()
	{
		return playerData;
	}
	public float GameSpeed
	{
		set{ gameSpeed = value; }
		get{ return gameSpeed; }
	}

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		stateMachine = new GameStateMachine (this);
	}

	void OnDestroy()
	{
		if (updateRoutine != null)
		{
			StopCoroutine (updateRoutine);
		}
		if (stateMachine != null)
		{
			stateMachine.Destroy ();
			stateMachine = null;
		}
	}

	public void BeginStateUpdate()
	{
		updateRoutine = StartCoroutine (TimedUpdate());
	}

	public void ChangeDivider()
	{
		if (divider >= 8)
			divider = 1;
		else
			divider = divider * 2;

		GameUIManager.Instance.UpdateUI ();
	}

	private IEnumerator TimedUpdate()
	{
		yield return new WaitForEndOfFrame ();
		while(stateMachine != null)
		{
			yield return new WaitForSeconds (GameSpeed/divider);
			stateMachine.Update ();
		}
	}

	public void AIAttack()
	{
		if (PlayerManager.Instance != null)
			PlayerManager.Instance.PlayerAIAttack ();
		else
			Debug.LogError ("PlayerManager is null!");

		if (EnemyManager.Instance != null)
			EnemyManager.Instance.EnemyAIAttack ();
		else
			Debug.LogError ("EnemyManage is null!");
	}

	public void ExitGame()
	{
		if (StateMachine.GetCurrentState.State == GameState.INGAME) 
		{
			StateMachine.SwitchState (GameState.EXIT);
		}
	}

	public void MainPlayerAttack()
	{
		if (PlayerManager.Instance != null)
			PlayerManager.Instance.MainCharacterAttack ();
	}

	public void PlayerAttack()
	{
		if (EnemyManager.Instance == null || EnemyManager.Instance.enemies.Count <= 0)
			return;

		bool hasChanges = false;
		int rand = Random.Range (0, EnemyManager.Instance.enemies.Count);
		EnemyManager.Instance.enemies [rand].DecreseLife (1);
		//LogHandler.AddLog ("Life Dec " + rand + " Remaining Life =" +  enemyController [rand].life);
		if (EnemyManager.Instance.enemies [rand].life <= 0) 
		{
			EnemyManager.Instance.enemies [rand].gameObject.SetActive (false);
			deadEnemy.Add (EnemyManager.Instance.enemies [rand]);
			EnemyManager.Instance.enemies.RemoveAt (rand);
			CheckWinOrLoseCondition ();
			playerData.KillCount++;
			hasChanges = true;
		}

		if (EnemyManager.Instance.enemies.Count <= 0 && isReviving == false) 
		{
			playerData.WaveCount++;
			isReviving = true;
			Invoke ("DelayReviveEnemy", 0.5f);
			hasChanges = true;
		}

		if(hasChanges && GameManager.instance != null)
			GameUIManager.Instance.UpdateUI ();
	}

	public void EnemyAttack()
	{
		if (PlayerManager.Instance == null || PlayerManager.Instance.fullListOfPlayer.Count <= 0)
			return;

		int rand = Random.Range (0, PlayerManager.Instance.fullListOfPlayer.Count);
		PlayerManager.Instance.fullListOfPlayer [rand].DecreseLife (1);
	}

	private void DelayReviveEnemy()
	{
		Debug.Log ("ALL IS DEAD, REBIRTH INITIALIZE");
		EnemyManager.Instance.enemies = new List<EnemyController> ();
		EnemyManager.Instance.enemies = deadEnemy;
		deadEnemy = new List<EnemyController> ();
		for (int i = 0; i < EnemyManager.Instance.enemies.Count; i++) 
		{
			EnemyManager.Instance.enemies [i].life = 20;
			EnemyManager.Instance.enemies [i].gameObject.SetActive (true);
			EnemyManager.Instance.enemies [i].PlaySpawnAnim ();
		}
		isReviving = false;
	}

	private void CheckWinOrLoseCondition()
	{
		//if (enemyController.Count <= 0)
	}
}
