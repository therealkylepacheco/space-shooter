using System;
using System.Collections;
using UnityEngine;

public class ShooterBehavior : MonoBehaviour
{
    public float speed = 800;

    public float health = 5;

    private float domainMin = 600;

    public Material damageMaterial;
    public Material defaultMaterial;

    private float timestamp;
    public float timeBetweenShots = 0.5f;
    public GameObject projectile;

    private GameObject player;

    private float projectileOffsetX = -50;
    private float projectileOffsetY = -50;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > domainMin)
        {
            MoveToPosition();
        }
        else
        {
            Fire();
        }
    }

    void MoveToPosition()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        transform.Translate(directionToPlayer * speed * Time.deltaTime);

    }

    void Fire()
    {
        if (Time.time >= timestamp)
        {
            Vector3 spawnPos = new Vector3(transform.position.x + projectileOffsetX, transform.position.y + projectileOffsetY, transform.position.z);

            Vector3 directionToPlayer = (player.transform.position - spawnPos).normalized;

            Quaternion q = Quaternion.FromToRotation(Vector3.right, directionToPlayer);
            Instantiate(projectile, spawnPos, q);
            timestamp = Time.time + timeBetweenShots;
        }
    }

    private void Damage()
    {
        --health;
        if (health > 0)
        {
            StartCoroutine(DamageEffect.Play(gameObject, 5, damageMaterial, defaultMaterial));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Damage();
        }
    }
}
