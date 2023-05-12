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
    public GameObject healthIcon;
    public int health = 3;
    private GameObject[] healthIcons;
    public Material damageMaterial;
    public Material defaultMaterial;
    public bool vulnerable = true;
    public float damageCooldown = 2.5f;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        timestamp = Time.time;
        healthIcons = PopulateHealth();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleFire();
    }

    private GameObject[] PopulateHealth()
    {
        GameObject[] temp = new GameObject[health];
        for (int i = 0; i < health; i++)
        {
            Quaternion rotation = healthIcon.transform.rotation;
            Vector3 originalPosition = healthIcon.transform.position;
            Vector3 position = new Vector3(originalPosition.x + (i * 100), originalPosition.y, originalPosition.z);
            temp[i] = Instantiate(healthIcon, position, rotation);
        }
        return temp;
    }

    void HandleFire()
    {
        if (Input.GetKeyDown(fireKey) && Time.time >= timestamp)
        {
            Vector3 spawnPos = new Vector3(transform.position.x + 80, transform.position.y, transform.position.z);
            GameObject projectileInstance = Instantiate(projectile, spawnPos, projectile.transform.rotation);
            ProjectileBehavior projectileScript = projectileInstance.GetComponent<ProjectileBehavior>();
            projectileScript.SetPlayerProjectile(true);
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
        Damage();
    }

    private void Damage()
    {
        if (vulnerable)
        {
            --health;
            if (health >= 0)
            {
                vulnerable = false;
                GameObject currentHealthIcon = healthIcons[health];
                Destroy(currentHealthIcon);
                StartCoroutine(DamageEffect.Play(gameObject, 2, damageMaterial, defaultMaterial));
                StartCoroutine(Invulnerable());
            }
            else
            {
                Destroy(gameObject);
                gameManager.GameOver();
            }

        }
    }

    IEnumerator Invulnerable()
    {
        yield return new WaitForSeconds(damageCooldown);
        vulnerable = true;
    }
}
