using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// LateUpdate is called once per frame after all other updates have been processed,
	// such as the player's position. 
	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}
