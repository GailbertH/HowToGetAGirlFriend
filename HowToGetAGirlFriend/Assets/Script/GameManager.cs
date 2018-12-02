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
	[SerializeField] private float gameSpeed = 1;

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

	private IEnumerator TimedUpdate()
	{
		yield return new WaitForEndOfFrame ();
		while(stateMachine != null)
		{
			yield return new WaitForSeconds (GameSpeed);
			stateMachine.Update ();
		}
	}

	public void AttackPressed()
	{
		
	}
}
