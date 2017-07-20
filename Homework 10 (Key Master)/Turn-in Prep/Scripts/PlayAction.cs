using UnityEngine;
using System.Collections;

public class PlayAction : MonoBehaviour
{

	// Use this for initialization
	public void ClickAction()
	{
		GameMaster.level = 1;
		Application.LoadLevel("maze");
	}
}
