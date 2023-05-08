using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private KeyCode up = KeyCode.UpArrow;
    private KeyCode down = KeyCode.DownArrow;
    private KeyCode left = KeyCode.LeftArrow;
    private KeyCode right = KeyCode.RightArrow;

    private KeyCode fireKey = KeyCode.Space;

    private float domain = 1058;
    private float range = 467;

    public float timeBetweenShots = 0.25f;
    private float timestamp;

    public float speed = 500;
    public GameObject projectile;


    // Start is called before the first frame update
    void Start()
    {
        timestamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleFire();
    }

    void HandleFire()
    {
        if (Input.GetKeyDown(fireKey) && Time.time >= timestamp)
        {
            Instantiate(projectile, transform.position, projectile.transform.rotation);
            timestamp = Time.time + timeBetweenShots;
        }
    }

    void HandleMovement()
    {
        bool isInUpperDomain = WithinDomain(true);
        bool isInLowerDomain = WithinDomain(false);
        bool isInUpperRange = WithinRange(true);
        bool isInLowerRange = WithinRange(false);

        if (Input.GetKey(up) && isInUpperRange)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
        if (Input.GetKey(down) && isInLowerRange)
        {
            transform.Translate(Vector3.down * Time.deltaTime * speed);
        }
        if (Input.GetKey(left) && isInLowerDomain)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (Input.GetKey(right) && isInUpperDomain)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
    }

    bool WithinDomain(bool upper)
    {
        if (upper)
        {
            return transform.position.x < domain;
        }
        else
        {
            return transform.position.x > (-1 * domain);
        }
    }

    bool WithinRange(bool upper)
    {
        if (upper)
        {
            return transform.position.y < range;
        }
        else
        {
            return transform.position.y > (-1 * range);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            Debug.Log("Asteroid Collision");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("KDP");
    }
}
