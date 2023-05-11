using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
    public float childAngleRange = 45;
    public int scale = 0; // scale should be 0, 1, 2, or 3;
    public float rotationSpeed = 0;

    public GameObject childAsteroid;

    // Start is called before the first frame update
    void Start()
    {
        SetScale(scale);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetScale(int newScale)
    {
        scale = newScale;

        float calculatedScale = Mathf.Pow(2, newScale);
        float zScale = transform.localScale.z;

        transform.localScale = new Vector3(calculatedScale, calculatedScale, zScale);
    }


    private void OnTriggerEnter(Collider other)
    {
        bool isProjectile = other.CompareTag("Projectile");

        if (isProjectile)
        {
            Explode();
        }
    }

    private void Explode()
    {

        // Spawn smaller asteroids

        int newScale = scale - 1;
        if (newScale >= 0)
        {
            SpawnChild(newScale, true);
            SpawnChild(newScale, false);
        }
        // set new scale

        Destroy(gameObject);
    }

    private void SpawnChild(int newScale, bool upper)
    {

        float childAngle = upper ? Random.Range(0, childAngleRange) : Random.Range(-1 * childAngleRange, 0);

        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, childAngle);
        GameObject child = Instantiate(childAsteroid, transform.position, rotation);

        child.GetComponent<AsteroidBehavior>().SetScale(newScale);
    }
}
