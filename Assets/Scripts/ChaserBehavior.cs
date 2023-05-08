using System.Collections;
using UnityEngine;

public class ChaserBehavior : MonoBehaviour
{
    public float speed = 400;

    public float health = 2;

    public Material damageMaterial;
    public Material defaultMaterial;

    private GameObject player;
    private float flickerRate = 0.1f;
    private int flickerCount = 25;

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

    private void ChangeMaterial(Material newMaterial)
    {
        Material[] materialArr = { newMaterial };
        GameObject child = transform.GetChild(0).gameObject;
        child.GetComponent<Renderer>().materials = materialArr;
    }

    private void Damage()
    {
        --health;
        if (health > 0)
        {
            // kdp play damage effect
            StartCoroutine(DamageEffect());
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


    IEnumerator DamageEffect()
    {
        int i = 0;

        while (i <= flickerCount)
        {
            yield return new WaitForSeconds(flickerRate);
            ChangeMaterial((i % 2 == 0) ? damageMaterial : defaultMaterial);
            i++;
        }

    }
}
