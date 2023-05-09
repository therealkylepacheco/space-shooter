using System;
using System.Collections;
using UnityEngine;

public class ShooterBehavior : MonoBehaviour
{
    public float speed = 800;

    public float health = 5;

    private float domain = 1058;
    private float range = 467;

    public Material damageMaterial;
    public Material defaultMaterial;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        // Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // transform.Translate(directionToPlayer * speed * Time.deltaTime);
    }

    private void Damage()
    {
        Debug.Log("KDP DAMAGE");
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
