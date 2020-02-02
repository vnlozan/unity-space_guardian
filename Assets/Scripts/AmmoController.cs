using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour {
	
	private int rocketsCount;
	private int shieldEnergy;
	private int healEnergy;

	private void Start () {
		if (1 < Random.Range (0, 5)) {
			rocketsCount = Random.Range(0,2);
		}
		healEnergy = Random.Range (50,300);
		shieldEnergy = Random.Range (0,30);
	}
	public int GetRocketsCount(){
		return rocketsCount;
	}
	public int GetShieldEnergy(){
		return shieldEnergy;
	}
	public int GetHealEnergy(){
		return healEnergy;
	}
}
