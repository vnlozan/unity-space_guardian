using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	private int damage = 100;
	public int GetDamage(){
		return damage;
	}
	public void Hit(){
		Destroy (gameObject);
	}
}
