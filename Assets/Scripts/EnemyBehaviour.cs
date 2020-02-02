using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public GameObject enemyLaser;
	public GameObject explosionObject;
	public GameObject ammo;

	private float addLaserSpeed = 0.0025f;
	public float health = 150.0f;
	public float ammoDropSpeed = -2.0f;
	private float frequency = 0.25f;
	private float laserSpeed = -9.0f;

	private void Start(){
		GameManager.enemiesAlive++;
	}
	private void Update(){
		laserSpeed -= addLaserSpeed;
		float probabillity = Time.deltaTime * frequency;
		if(Random.Range(0.0001f, 0.2f) < probabillity)
		{
			Fire ();
		}
	}
	private void Fire(){
		Vector3 startPos = transform.position + new Vector3 (0.0f, -2.15f, 0.0f);
		GameObject laser = (GameObject)Instantiate (enemyLaser, startPos, Quaternion.Euler(0.0f, 0.0f, 90.0f));
		laser.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range(-4.0f, 4.0f), laserSpeed, 0.0f);
	}
	private void OnTriggerEnter2D(Collider2D collision){
		GameObject collidedObject = collision.gameObject;
		switch (collidedObject.tag) {
			case "laser":
				Projectile proj = collision.gameObject.GetComponent<Projectile> ();
				health -= proj.GetDamage ();
				proj.Hit ();
				if (health < 0) {
					GameObjectDestroy ();
				}
				break;
			case "meteorite":
				GameObjectDestroy();
				break;
		}
	}
	public void GameObjectDestroy(){
		Destroy (gameObject);
		GameManager.enemiesYouHaveDestroyedCount++;
		GameManager.enemiesAlive--;
		GameObject explosion = (GameObject)Instantiate (explosionObject, transform.position, Quaternion.identity);
		explosion.GetComponent<Animator> ().Play ("Boom");
		float ammoSpawnProbability = Random.Range (0.0f, 4.0f);
		if (ammoSpawnProbability < 1.0f) {
			GameObject ammoBox = (GameObject)Instantiate (ammo, transform.position, Quaternion.identity);
			ammoBox.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (-1.0f, 1.0f), ammoDropSpeed);
		}
	}
}
