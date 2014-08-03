using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject player;
	private GameCamera cam; //reference to the game camera 

	// Use this for initialization
	void Start () {
		cam = GetComponent<GameCamera>();
		SpawnPlayer();
	}
	private void SpawnPlayer()
	{
		//setTarget method takes transform object as a parameter, so we have to convert the game object into a transform 
		cam.SetTarget((Instantiate(player, Vector2.zero, Quaternion.identity) as GameObject).transform);
	}
}
