using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour {
	public GameObject planet;
	private float spawnFrequency = 0.9f;
	private float minDistance = 6.0f;
	public Sprite[] planetSprites;
	private void FixedUpdate () {
		if (GameManager.planetsSpawn < 10 && Random.value <= (spawnFrequency * Time.deltaTime))
		{
			Vector3 pos = FindNewPos();
			if (!pos.Equals(Vector3.zero))
			{
				GameManager.planetsSpawn++;
				float planetSpeed = Random.Range(-1, -5);
				float planetScale = Random.Range(0.1f, 1f);
				GameObject p = Instantiate(planet, pos, Quaternion.identity);
				p.transform.parent = this.transform;

				int planetIndex = Random.Range(0, planetSprites.Length);
				p.GetComponent<SpriteRenderer>().sprite = planetSprites[planetIndex];
				p.GetComponent<SpriteRenderer>().material.color = new Color(Random.value,Random.value,Random.value,1.0f);
				p.transform.localScale = new Vector3(planetScale, planetScale);
				p.gameObject.AddComponent<CircleCollider2D>();
				p.gameObject.GetComponent<CircleCollider2D>().radius = 2.4f;
				p.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, -3.0f, 0.0f);
			}
		}
	}
	private Vector3 FindNewPos()
	{
		Vector3 pos = new Vector3(Random.Range(PlayerController.xMin - 5.0f, PlayerController.xMax + 5.0f),23.0f);
		Collider2D[] neighbours = Physics2D.OverlapCircleAll(pos, minDistance, 1<<16);
		if (neighbours.Length <= 0)
		{
			return pos;
		}
		return Vector3.zero;
	}
}
