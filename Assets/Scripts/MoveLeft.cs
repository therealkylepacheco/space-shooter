using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 500;
    public float lowerDomain = -1300;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < lowerDomain)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
