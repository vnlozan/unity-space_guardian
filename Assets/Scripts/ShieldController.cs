using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{

    private GameObject player;
    private float delayTime;
    private float radius = 2.5f;
    private float speed = 20.0f;
    public float shield2Delay;
	private void Start()
    {
        player = transform.parent.gameObject;
        delayTime = Time.time;
    }
	private void Update()
    {
        Vector3 shieldPos = new Vector3();
        shieldPos.x += player.transform.position.x + radius * Mathf.Cos((Time.time - delayTime - shield2Delay) * speed);
        shieldPos.y += player.transform.position.y + radius * Mathf.Sin((Time.time - delayTime - shield2Delay) * speed);
        this.transform.position = shieldPos;

        Vector3 heading = transform.position - player.transform.position;
        Vector3 direction = heading.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
	private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        switch (collidedObject.tag)
        {
            case "laser":
                Projectile proj = collision.gameObject.GetComponent<Projectile>();
                float dmgValue = proj.GetDamage();
                proj.Hit();
                break;
        }
    }
}
