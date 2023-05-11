using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject star;
    public float backgroundRate = 0.25f;

    public bool gameIsActive = false;

    private float backgroundRange = 543;
    private float backgroundX = 1280;

    private int asteroidCount;
    private int chaserCount;
    private int harasserCount;
    private int shooterCount;

    private float startTime;

    private float enemyDomain = 1058;
    private float enemyRange = 467;

    public GameObject asteroid;
    public GameObject chaser;
    public GameObject harasser;
    public GameObject shooter;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        asteroidCount = GetAsteroidCount();
        chaserCount = GetChaserCount();
        harasserCount = GetHarasserCount();
        shooterCount = GetShooterCount();
    }

    void StartGame()
    {
        gameIsActive = true;
        startTime = Time.deltaTime;
        StartCoroutine(SpawnStar());
        StartCoroutine(GenerateWave(10));
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

    int GetAsteroidCount()
    {
        return FindObjectsOfType<AsteroidBehavior>().Length;
    }
    int GetChaserCount()
    {
        return FindObjectsOfType<ChaserBehavior>().Length;
    }
    int GetHarasserCount()
    {
        return FindObjectsOfType<HarasserBehavior>().Length;
    }
    int GetShooterCount()
    {
        return FindObjectsOfType<ShooterBehavior>().Length;
    }

    IEnumerator GenerateWave(float waveDelay)
    {
        bool startWave = true;
        List<Coroutine> coroutines = new List<Coroutine>();

        int waveCount = 0;
        int cycle = 0;

        while (gameIsActive)
        {
            yield return new WaitForSeconds(waveDelay);

            if (startWave)
            {
                // kdp logic for kicking off new wave fires here
                Debug.Log($"WAVE {waveCount} STARTING");


                coroutines.Add(StartCoroutine(SpawnAsteroid(5)));
                startWave = false;
            }
            else if (cycle > 5)
            { //kdp arbitrary cycle count
                foreach (Coroutine coroutine in coroutines)
                {
                    StopCoroutine(coroutine);
                }
                Debug.Log($"WAVE {waveCount} FINISHED");
                waveCount++;
                cycle = 0;
                startWave = true;
            }

            cycle++;
        }


    }


    IEnumerator SpawnAsteroid(float spawnRate)
    {
        while (gameIsActive)
        {
            yield return new WaitForSeconds(spawnRate);
            GameObject asteroidInstance = SpawnGameObject(enemyDomain, enemyRange, asteroid);
            AsteroidBehavior asteroidScript = asteroidInstance.GetComponent<AsteroidBehavior>();
            asteroidScript.SetScale(Random.Range(0, 3));
        }
    }

    IEnumerator SpawnEnemy(float spawnRate, GameObject enemy)
    {
        while (gameIsActive)
        {
            yield return new WaitForSeconds(spawnRate);
            GameObject enemyInstance = SpawnGameObject(enemyDomain, enemyRange, enemy);
        }
    }


    private GameObject SpawnGameObject(float x, float y, GameObject gameObj)
    {
        float yPos = Random.Range(-1 * enemyRange, enemyRange);
        Vector3 spawnPosition = new Vector3(backgroundX, yPos, star.transform.position.z);
        Quaternion spawnRotation = asteroid.transform.rotation;

        return Instantiate(gameObj, spawnPosition, spawnRotation);
    }
}
