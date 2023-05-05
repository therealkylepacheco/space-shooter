using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject star;
    public float backgroundRate = 0.25f;

    public bool gameIsActive = true;

    private float backgroundRange = 543;
    private float backgroundX = 1280;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnStar()); // kdp add this back in
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnStar()
    {
        while (gameIsActive)
        {
            yield return new WaitForSeconds(backgroundRate);

            float backgroundY = Random.Range(-1 * backgroundRange, backgroundRange);
            Vector3 starPosition = new Vector3(backgroundX, backgroundY, star.transform.position.z);
            Quaternion starRotation = star.transform.rotation;

            Instantiate(star, starPosition, starRotation);
        }
    }
}
