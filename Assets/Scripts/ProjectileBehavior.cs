using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private float domain = 1400;
    public float speed = 1000;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // TODO: implement rotation
        transform.Translate(Vector3.right * Time.deltaTime * speed);
        if (Mathf.Abs(transform.position.x) > domain)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // KDP update score here based on the tag

        string tag = other.gameObject.tag;
        // Debug.Log($"Projectile hit {tag}");

        Destroy(gameObject);
    }
}
