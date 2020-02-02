using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteController : MonoBehaviour {
	public GameObject explosionObject;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "rocket")
		{
			RocketController rocket = collision.gameObject.GetComponent<RocketController>();
			rocket.Hit();
			GameObject explosion = (GameObject)Instantiate(explosionObject, transform.position, Quaternion.identity);
			explosion.GetComponent<Animator>().Play("Boom");
			Destroy(gameObject);
		}
	}

}
