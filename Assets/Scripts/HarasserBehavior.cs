using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarasserBehavior : MonoBehaviour
{
    public GameObject projectile;
    public Material damageMaterial;
    public Material defaultMaterial;

    public float speed = 1200;
    public float health = 3;
    // public float offsetWait = 250;
    public Vector3 offsetWait = new Vector3(0, 250, 0);
    public Vector3 offsetFire = new Vector3(1000, 0, 0);

    public float shotSpeed = 0.5f;

    public float shotCount = 5;

    public float projectileOffset = 50;

    public float waitTime = 20;

    private GameObject player;
    private bool firing = false;
    private bool waiting = true;
    private bool arrivedWait = false;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player(Clone)");
        // rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            MoveToWait();
        }
        else
        {
            MoveToFire();
        }
    }

    void MoveToWait()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 destination = player.transform.position - offsetWait;

        if (HasArrived(destination.x, transform.position.x) && !arrivedWait)
        {
            StartCoroutine(Wait());
        }

        if (arrivedWait)
        {
            // rb.MovePosition(destination);
            transform.position = destination;
        }
        else
        {
            Vector3 direction = (destination - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    void MoveToFire()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 destination = playerPosition + offsetFire;

        if (HasArrived(destination.y, transform.position.y) && !firing)
        {
            StartCoroutine(Fire(playerPosition));
        }

        if (firing)
        {
            // rb.MovePosition(destination);
            transform.position = destination;
        }
        else
        {

            Vector3 direction = (destination - transform.position).normalized;

            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    IEnumerator Fire(Vector3 playerPosition)
    {
        int count = 0;
        firing = true;

        while (count < shotCount)
        {
            yield return new WaitForSeconds(shotSpeed);

            Vector3 spawnPos = new Vector3(transform.position.x - projectileOffset, transform.position.y, transform.position.z);

            Vector3 direction = (playerPosition - spawnPos).normalized;

            Quaternion q = new Quaternion(0, 0, 1, 0);

            Instantiate(projectile, spawnPos, q);

            count++;
        }
        firing = false;
        waiting = true;
        arrivedWait = false;
        offsetWait *= -1;
    }

    IEnumerator Wait()
    {
        arrivedWait = true;
        while (waiting)
        {
            yield return new WaitForSeconds(waitTime);
            waiting = false;
        }
    }

    private bool HasArrived(float destination, float position)
    {
        return Mathf.Abs(destination - position) <= 5;
    }


    private void Damage()
    {
        --health;
        if (health > 0)
        {
            StartCoroutine(DamageEffect.Play(gameObject, 3, damageMaterial, defaultMaterial));
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
