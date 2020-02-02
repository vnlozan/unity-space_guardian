using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
	public Text currentScoreText;
	public Text bestScoreText;
	// Use this for initialization
	void Start () {
		float bestScore = GameManager.GetScore();
		float currentScore = GameManager.GetLastScore();
		currentScoreText.text = "Your last score is " + currentScore;
		bestScoreText.text = "Current best score is " + bestScore;
	}
}
