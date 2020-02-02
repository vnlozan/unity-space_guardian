using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	#region ammo parameters
	public int playerHealth = 5000;
	private int shieldEnergy = 1000;
	private int rocketsCount = 10;
    private float rocketShootDelay = 0.5f;
    private bool rocketShoot = false;
	private bool shieldIsActive = false;
	#endregion
	#region positions and physics
	public float speed = 1f;
	private float rocketSpeed = 10.0f;
	public float projSpeed = 10.0f;
	public float firingRate = 0.25f;
	private float padding = 1.21f;
	private float paddingY = 1.3f;
	static public float xMin;// = 2.25f;
	static public float xMax;// = 29.75f;
	private float yMin;
	private float yMax;
	#endregion
	#region game objects
	public GameObject rocketRef;
	public GameObject projectile;
	public GameObject explosionObject;
	public GameObject floatingText;
    private GameObject[] shields;
    #endregion
    public Text shieldEnergyText;
    public Text rocketsCountText;
	private Animator playerAnimator;
	public Slider hpSlider;

	private void Start () {
		playerAnimator = GetComponent<Animator> ();
		shieldIsActive = false;
		hpSlider.maxValue = playerHealth;
		hpSlider.value = playerHealth;
        shields = GameObject.FindGameObjectsWithTag("Shield");
        ShieldSwitch(false);
        float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3(0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint (new Vector3(1,1,distance));
		xMin = leftMost.x + padding;
		xMax = rightMost.x - padding;

		yMin = leftMost.y + paddingY;
		yMax = rightMost.y - paddingY;
	}
	private void Fire(){
		Vector3 startPos = transform.position + new Vector3 (0.0f, 2.25f, 0.0f);
		GameObject proj = (GameObject)Instantiate (projectile, startPos, Quaternion.Euler(0.0f, 0.0f, 90.0f));
		proj.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, projSpeed);
	}
	private void LaunchRockets(){
		Vector3 rocketStartPos = transform.position + new Vector3 (0.0f, 2.25f, 0.0f);
		GameObject rocket = (GameObject)Instantiate (rocketRef, rocketStartPos, Quaternion.Euler(0.0f, 0.0f, 180.0f));
		rocket.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, rocketSpeed);
        rocketsCount--;
	}
	private void Update () {
        #region shieldBehavior
		if (Input.GetKey (KeyCode.LeftControl)) {
			if (shieldEnergy > 0.0f) {
				ShieldSwitch (true);
				shieldIsActive = true;
				shieldEnergy--;
			} else {
				ShieldSwitch (false);
				shieldIsActive = false;
			}
		}
		if (shieldIsActive && shieldEnergy > 0.0f) {
			if (Input.GetKeyUp (KeyCode.LeftControl)) {
				ShieldSwitch (false);
				shieldIsActive = false;
			}
		}
		#endregion
		#region shootBehavior
		if (Input.GetKeyDown (KeyCode.Space)) {
				InvokeRepeating ("Fire", 0.0001f, firingRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("Fire");
		}
		if(Input.GetKeyDown(KeyCode.V)){
            if(rocketsCount > 0 && rocketShoot == false)
            {
                rocketShoot = true;
                LaunchRockets();
                Invoke ("ShootTrigger", rocketShootDelay);
            }
		}
		#endregion
		#region movementBehavior
		if (Input.GetButton("MoveLeft"))
		{
			transform.position += Vector3.left * speed * Time.deltaTime;
			SetAnimationByStateName("left");
		}
		else if (Input.GetButton("MoveRight"))
		{
			transform.position += Vector3.right * speed * Time.deltaTime;
			SetAnimationByStateName("right");
		}
		else
		{
			SetAnimationByStateName("");
		}
		if (Input.GetButton("MoveUp"))
		{
			transform.position += Vector3.up * speed * Time.deltaTime;
		}
		if (Input.GetButton("MoveDown"))
		{
			transform.position += Vector3.down * speed * Time.deltaTime;
		}
		float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
		float newY = Mathf.Clamp(transform.position.y, yMin, yMax);
		transform.position = new Vector3(newX, newY, transform.position.z);
        #endregion
        UpdateHUD();
	}
	private void ShootTrigger()
    {
        rocketShoot = false;
    }
	private void OnTriggerEnter2D(Collider2D collision){
		GameObject collidedObject = collision.gameObject;

		switch (collidedObject.tag) {
		case "laser":
			Projectile proj = collidedObject.GetComponent<Projectile> ();
			int dmgValue = proj.GetDamage ();
			playerHealth -= dmgValue;
			hpSlider.value = playerHealth;
			SpawnFloatingText ( "damage", dmgValue.ToString() );
			proj.Hit ();
			if (playerHealth < 0) {
				Die (); 
			}
			break;
		case "meteorite":
			Die ();
			break;
		case "ammo":
			    AmmoController ammoController = collidedObject.GetComponent<AmmoController> ();

                int addRocketsCount = ammoController.GetRocketsCount();
                int addShieldEnergy = ammoController.GetShieldEnergy();
                int addPlayerHealth = ammoController.GetHealEnergy();

			    shieldEnergy += addShieldEnergy;
			    playerHealth += addPlayerHealth;
			    rocketsCount += addRocketsCount;
			    SpawnFloatingText ("rockets", addRocketsCount.ToString());
			    SpawnFloatingText ("heal_energy", addPlayerHealth.ToString());
			    SpawnFloatingText ("shield_energy", addShieldEnergy.ToString());
			    Destroy (collision.gameObject);
			    break;
		}
	}
	public void ShieldSwitch(bool val){
		foreach (GameObject shield in shields) {
            shield.SetActive(val);
		}
	}
	void LoadLoseScene()
	{
		SceneManager.LoadScene("Lose");
	}
	public void Die(){
		//Destroy (gameObject);
		gameObject.SetActive(false);
		GameManager.SaveNewScore();
		GameObject explosion = (GameObject)Instantiate (explosionObject, transform.position, Quaternion.identity);
		explosion.GetComponent<Animator> ().Play ("Boom");
		Invoke("LoadLoseScene",1.0f);
	}
	private void SpawnFloatingText(string propriety, string value){
        if(value != "0"){
		    Vector3 startPos;
		    GameObject infoObject;
		    switch (propriety) {
                case "rockets":
                    startPos = transform.position + new Vector3(2.0f, 0.6f);
                    infoObject = Instantiate(floatingText, startPos, Quaternion.identity);
                    infoObject.transform.parent = transform;
                    infoObject.GetComponent<TextMesh>().text = "+" + value;
                    infoObject.GetComponent<TextMesh>().color = Color.yellow;
                    StartCoroutine(TextMoveUp(infoObject));
                    break;
                case "shield_energy":
                    startPos = transform.position + new Vector3(2.0f, 1.0f);
                    infoObject = Instantiate(floatingText, startPos, Quaternion.identity);
                    infoObject.transform.parent = transform;
                    infoObject.GetComponent<TextMesh>().text = "+" + value;
                    infoObject.GetComponent<TextMesh>().color = Color.blue;
                    StartCoroutine(TextMoveUp(infoObject));
                    break;
                case "heal_energy":
			        startPos = transform.position + new Vector3 (2.0f, 1.4f);
			        infoObject = Instantiate (floatingText, startPos, Quaternion.identity);
			        infoObject.transform.parent = transform;
                    infoObject.GetComponent<TextMesh> ().text = "+" + value;
                    infoObject.GetComponent<TextMesh> ().color = Color.green;
			        StartCoroutine (TextMoveUp(infoObject));
			    break;
                case "damage":
                    startPos = transform.position + new Vector3(2.0f, 0.5f);
                    infoObject = Instantiate(floatingText, startPos, Quaternion.identity);
                    infoObject.transform.parent = transform;
                    infoObject.GetComponent<TextMesh>().text = "-" + value;
                    infoObject.GetComponent<TextMesh>().color = Color.red;
                    StartCoroutine(TextMoveDown(infoObject));
                    break;
            }
        }
	}
	private IEnumerator TextMoveUp(GameObject objectToMove){
		for (int i = 0; i < 100; i++) {
			objectToMove.transform.position += Vector3.up * 0.02f;
			yield return null;
		}
		Destroy (objectToMove);
	}
    private IEnumerator TextMoveDown(GameObject objectToMove)
    {
        for (int i = 0; i < 100; i++)
        {
            objectToMove.transform.position += Vector3.down * 0.02f;
            yield return null;
        }
        Destroy(objectToMove);
    }
	private void SetAnimationByStateName(string stateName){
		switch(stateName){
		case "right":
			playerAnimator.SetBool ("rightMove", true);
			playerAnimator.SetBool ("leftMove", false);
			break;
		case "left":
			playerAnimator.SetBool ("rightMove", false);
			playerAnimator.SetBool ("leftMove", true);
			break;
		default:
			playerAnimator.SetBool ("rightMove", false);
			playerAnimator.SetBool ("leftMove", false);
			break;
		}
	}
    private void UpdateHUD()
    {
        shieldEnergyText.text = "[ "+ shieldEnergy.ToString() + " ]";
        rocketsCountText.text = "[" + rocketsCount.ToString() + " ]";
    }
}
