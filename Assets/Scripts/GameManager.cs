using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject Boss_01;
	public GameObject starField;
	public GameObject starField2;
	private ParticleSystem sf1ParticleSystem;
	private ParticleSystem sf2ParticleSystem;

	private Vector3 bossSpawnPosition = new Vector3(16.0f, 16.0f);
	public static Vector3 bossSpawnPos;

	private static int enemiesYouMustDestroyCount = 100;
	public static int enemiesYouHaveDestroyedCount = 0;
	public static int enemiesAlive = 0;
	public static int planetsSpawn = 0;
	
	public static bool meteoriteSpawn = false;
	public static bool enemiesSpawn = true;
	public static bool bossSpawn = false;

	public static float timer;
	public static bool timerActive = true;

	private float middlePosX;
	private float topPos;
	public static float bossIdlePosY;
	private static string BEST_SCORE_KEY_NAME = "BEST_GAME_SCORE";
	private static string LAST_SCORE_KEY_NAME = "LAST_GAME_SCORE";

	private void Start(){
		sf1ParticleSystem = starField.GetComponent<ParticleSystem> ();
		sf2ParticleSystem = starField2.GetComponent<ParticleSystem> ();
		CalculateBossStartPos ();
		timerActive = true;
		timer = 0;
	}
	private void Update () {
		if (timerActive == true)
		{
			timer += Time.deltaTime;
		}
		if(bossSpawn == false)
		{
			if (enemiesYouHaveDestroyedCount >= enemiesYouMustDestroyCount && enemiesSpawn == true) {
				enemiesSpawn = false;
			}
			if(enemiesSpawn == false && enemiesAlive <=0)
			{
				meteoriteSpawn = false;
				bossSpawn = true;
				SpawnBoss();
			}
		}
		if (bossSpawn == true && BossController.idleState == false && sf1ParticleSystem.startSpeed < 30) {
			sf1ParticleSystem.startSpeed += 1.0f;
			sf2ParticleSystem.startSpeed += 0.5f;
		}
		if (BossController.idleState == true) {
			sf1ParticleSystem.startSpeed = 10.0f;
			sf2ParticleSystem.startSpeed = 5.0f;
		}
	}
	private void SpawnBoss(){
		
		Instantiate (Boss_01, bossSpawnPos, Quaternion.identity);
	}
	private void CalculateBossStartPos(){
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, distance));
		topPos = rightMost.y;
		middlePosX = rightMost.x / 2;
		bossSpawnPos = new Vector3 (middlePosX, topPos * 2);
		bossIdlePosY = topPos / 2 + 2.0f;
	}
	public static void SaveNewScore()
	{
		GameManager.timerActive = false;
		float bestScore = PlayerPrefs.GetFloat(BEST_SCORE_KEY_NAME);
		Debug.Log("Your old score is " + bestScore + " seconds");
		Debug.Log("Your current score is " + GameManager.timer + " seconds");
		PlayerPrefs.SetFloat(LAST_SCORE_KEY_NAME, timer);
		if (bestScore < timer)
		{
			PlayerPrefs.SetFloat(BEST_SCORE_KEY_NAME, timer);
			Debug.Log("Your new score is " + bestScore + " seconds");
		}
	}
	public static float GetScore()
	{
		return PlayerPrefs.GetFloat(BEST_SCORE_KEY_NAME);
	}
	public static float GetLastScore()
	{
		return PlayerPrefs.GetFloat(LAST_SCORE_KEY_NAME);
	}
}
