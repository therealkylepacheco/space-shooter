using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private bool playerProjectile = false;
    private float domain = 1000;
    public float speed = 1000;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    public void SetPlayerProjectile(bool isPlayerProjectile)
    {
        playerProjectile = isPlayerProjectile;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerProjectile)
        {
            gameManager.IncreaseScore(other.tag);
        }

        Destroy(gameObject);
    }
}
