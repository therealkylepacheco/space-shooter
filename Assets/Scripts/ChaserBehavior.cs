using System;
using System.Collections;
using UnityEngine;

public class ChaserBehavior : MonoBehaviour
{
    public float speed = 400;

    public float health = 2;

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

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        transform.Translate(directionToPlayer * speed * Time.deltaTime);
    }

    private void Damage()
    {
        --health;
        if (health > 0)
        {
            StartCoroutine(DamageEffect.Play(gameObject, 0, damageMaterial, defaultMaterial));
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
