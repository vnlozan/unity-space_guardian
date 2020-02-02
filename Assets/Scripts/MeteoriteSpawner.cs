using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour {

	public GameObject Meteorite;
	private Vector3 meteoritePosition;
	private static float meteoriteSpeed = -16.0f;
	private float addSpeed = 0.25f;
	private float spawnProbabillity;
	private float spawnFrequency = 0.8f;

	private void Start(){
		Invoke ("EnableMeteoriteSpawn", 3.0f);
	}
	private void Update () {
		if (GameManager.meteoriteSpawn) {
			meteoriteSpeed -= addSpeed;
			if (Random.value <= spawnFrequency * Time.deltaTime)
			{
				SpawnMeteor();
			}
		}
	}
	private void EnableMeteoriteSpawn(){
		GameManager.meteoriteSpawn = true;
	}
	private void SpawnMeteor(){
		meteoriteSpeed = -16.0f;
		meteoritePosition = new Vector3 (Random.Range(PlayerController.xMin, PlayerController.xMax), 23.0f, 0.0f);
		GameObject meteorite = (GameObject)Instantiate (Meteorite, meteoritePosition, Quaternion.identity);
		meteorite.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0.0f, meteoriteSpeed, 0.0f);
	}
}
