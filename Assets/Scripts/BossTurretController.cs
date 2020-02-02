using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretController : MonoBehaviour
{

    public GameObject enemyLaser;
    private GameObject boss;
    public GameObject explosionObject;
    private Vector3 laserDirection;
    private float shotsPerFrame = 0.2f;
    private float laserSpeed = 6.0f;
    private float laserAngle;
    private bool startShooting;
    private int hitPoints;

    private void Start()
    {
        hitPoints = 1000;
        boss = transform.parent.gameObject;
    }
    private void Update()
    {
        if (BossController.idleState == true && startShooting == false)
        {
            InvokeRepeating("Fire", 0.0000001f, shotsPerFrame);
            startShooting = true;
        }
    }
    private void Fire()
    {
        Vector3 heading = transform.position - boss.transform.position;
        laserDirection = heading.normalized;
        laserAngle = Mathf.Atan2(laserDirection.y, laserDirection.x) * Mathf.Rad2Deg;

        GameObject laser = (GameObject)Instantiate(enemyLaser, transform.position, Quaternion.identity);
        laser.transform.rotation = Quaternion.AngleAxis(laserAngle, Vector3.forward);
        laser.GetComponent<Rigidbody2D>().velocity = laserDirection * laserSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        switch (collidedObject.tag)
        {
            case "laser":
                Projectile proj = collidedObject.GetComponent<Projectile>();
                int dmgValue = proj.GetDamage();
                hitPoints -= dmgValue;
                proj.Hit();
                if (hitPoints < 0)
                {
                    BoomAnimationSpawn();
                }
                break;
        }
    }
    private void BoomAnimationSpawn()
    {
        BossController.TurretDestroyed();
        GameObject explosion = (GameObject)Instantiate(explosionObject, transform.position, Quaternion.identity);
        explosion.GetComponent<Animator>().Play("Boom");
        explosion.transform.parent = transform.parent;
        Destroy(gameObject);
    }
}