using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour 
{
	[SerializeField] GameObject genericPopup;

	private void PopulateMissions()
	{

	}

	public void PlayClicked()
	{
		LoadingManager.Instance.SetSceneToUnload (SceneNames.LOBBY_SCENE);
		LoadingManager.Instance.SetSceneToLoad (SceneNames.GAME_UI + "," + SceneNames.GAME_SCENE);
		LoadingManager.Instance.LoadGameScene ();
	}

	public void ExitClicked()
	{
		Application.Quit ();
	}
}