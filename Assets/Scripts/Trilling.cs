using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trilling : MonoBehaviour {

	public float speed;
	public float amplitudeX;
	public float amplitudeY;

	private Vector3 initialPos;

	private void Start () {
		initialPos = gameObject.transform.position;
	}
	private void Update () {
		Vector3 trillPos = gameObject.transform.position;													//circle equation

		trillPos.x = initialPos.x + amplitudeX * Mathf.Sin (Time.time * speed);
		trillPos.y = initialPos.y + amplitudeY * Mathf.Cos (Time.time * speed);

		gameObject.transform.position = trillPos;
	}
}
