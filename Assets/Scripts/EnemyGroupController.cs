using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupController : MonoBehaviour {

	public GameObject enemyPref;

	public float width = 10.0f;
	public float height = 5.0f;
	public float speed = 5.0f;
	public bool moveRight = true;
	public float spawnDelay = 0.5f;

	private float xMin;
	private float xMax;

	private void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3(0, 0, distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint (new Vector3(1, 0, distance));
		xMin = leftMost.x;
		xMax = rightMost.x;
		SpawnUntilFull ();
	}
	private void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, new Vector3(width, height));
	}
	private bool EverybodyIsDead(){
		if (GameManager.enemiesSpawn == true) {
			foreach (Transform childPos in transform) {
				if (childPos.childCount > 0) {
					return false;
				}
			}
			return true;
		}
		return false;
	}
	private void Update () {
		if (moveRight) {
			transform.position += Vector3.right * Time.deltaTime * speed;
		} else {
			transform.position += Vector3.left * Time.deltaTime * speed;
		}

		float rightEdgeOfFormation = transform.position.x + (0.5f * width);
		float leftEdgeOfFormation = transform.position.x - (0.5f * width);

		if (leftEdgeOfFormation < xMin) {
			moveRight = true;
		}else if (rightEdgeOfFormation > xMax){
			moveRight = false;
		}

	    if (EverybodyIsDead () ) {
		    SpawnUntilFull ();
	    }
	}
	private void SpawnEnemies(){
		foreach (Transform child in transform) {
			GameObject enemy = (GameObject)Instantiate (enemyPref, child.transform.position, Quaternion.Euler (new Vector3 (0.0f, 0.0f, 180.0f)));
			enemy.transform.parent = child;
		}
	}
	private Transform NextFreePos(){
		if (GameManager.enemiesSpawn == true) {
			foreach (Transform childPos in transform) {
				if (childPos.childCount == 0) {
					return childPos;
				}
			}
			return null;
		}
		return null;
	}
	private void SpawnUntilFull(){
		Transform freePosition = NextFreePos ();
		if(freePosition){
			GameObject enemy = (GameObject)Instantiate (enemyPref, freePosition.position, Quaternion.identity);
			enemy.transform.parent = freePosition;
		}
		if(NextFreePos()){
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}
}
