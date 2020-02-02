using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour {

	private Vector3 initialPos;

	private float amplitudeX = 10.0f;
	private float amplitudeY = 1.0f;
	private float rotationSpeed = 1.0f;
	private float orbitSpeed = 1.5f;
	private float comingSpeed = 4.0f;
	private float timeDelay;

	public bool cameState = false;
	public static bool swingState = false;
	public static bool idleState = false;
    private static int turretsCount = 5;

	public GameObject explosion;
	public GameObject flash;

	private void Update(){
        if (turretsCount <= 0)
        {
			Die();
        }
        if (cameState == false) {
			transform.position += Vector3.down * Time.deltaTime * comingSpeed;
			if (transform.position.y <= GameManager.bossIdlePosY) {
				Invoke ("IdleStatePosWait", 2.0f);
				cameState = true;
			}
		}

		if (cameState == true && idleState == true) {
			Vector3 swingPos = initialPos;
			swingPos.x = initialPos.x + amplitudeX * Mathf.Sin ((Time.time - timeDelay) * orbitSpeed);
			swingPos.y = initialPos.y + amplitudeY * Mathf.Cos ((Time.time - timeDelay) * orbitSpeed);
			transform.position = swingPos;
		}
		transform.Rotate (0.0f, 0.0f, rotationSpeed);
	}
	private void IdleStatePosWait(){
		idleState = true;
		initialPos = transform.position;
		timeDelay = Time.time;
	}
    public static void TurretDestroyed()
    {
        turretsCount--;
    }
	private void Die(){
		gameObject.SetActive(false);
		GameManager.SaveNewScore();
		GameObject expl = Instantiate (explosion,transform.position,Quaternion.identity);
		expl.GetComponent<Animator> ().Play ("Boom");
		Instantiate (flash,new Vector2(16.0f,9.0f),Quaternion.identity);
		//Destroy (gameObject);
		Invoke("LoadWinScene", 1.0f);
	}
	private void LoadWinScene()
	{
		SceneManager.LoadScene("Win");
	}
}
