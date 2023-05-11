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
    private float asteroidRate = 5;
    public GameObject chaser;
    private float chaserRate = 10;
    public GameObject harasser;
    private float harasserRate = 15;
    public GameObject shooter;
    private float shooterRate = 20;

    private int adjustCount = 0;

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

            int enemyCount = chaserCount + shooterCount + harasserCount;

            if (startWave)
            {
                // kdp logic for kicking off new wave fires here
                Debug.Log($"WAVE {waveCount} STARTING");

                float chaserSpawnRate = waveCount > 0 ? chaserRate : 0; // kdp need to calculate this
                float shooterSpawnRate = waveCount > 1 ? shooterRate : 0;
                float harasserSpawnRate = waveCount > 2 ? harasserRate : 0;


                // always spawn asteroids
                coroutines.Add(StartCoroutine(SpawnAsteroid(asteroidRate)));

                if (chaserSpawnRate > 0)
                {
                    coroutines.Add(StartCoroutine(SpawnEnemy(chaserSpawnRate, chaser)));
                }

                if (shooterSpawnRate > 0)
                {
                    coroutines.Add(StartCoroutine(SpawnEnemy(shooterSpawnRate, shooter)));
                }

                if (harasserSpawnRate > 0)
                {
                    coroutines.Add(StartCoroutine(SpawnEnemy(harasserSpawnRate, harasser)));
                }


                startWave = false;
            }
            else if (cycle > 4) // kdp arbitrary cycle count
            {
                foreach (Coroutine coroutine in coroutines)
                {
                    StopCoroutine(coroutine);
                }

                if (enemyCount == 0)
                {
                    Debug.Log($"WAVE {waveCount} FINISHED"); // kdp display message
                    waveCount++;
                    cycle = 0;
                    startWave = true;
                    if (waveCount > 3)
                    {
                        AdjustSpawnRates();
                    }
                }
            }
            else
            {
                Debug.Log($"KDP INCREMENT CYCLE TO {cycle + 1}");
                cycle++; // kdp need to increment this ONLY IF wave is actively spawning (will need additional checks when enemy count is a factor)
            }
        }


    }

    void AdjustSpawnRates()
    {

        switch (adjustCount)
        {
            case 0:
                asteroidRate *= 0.75f;
                break;
            case 1:
                chaserRate *= 0.75f;
                break;
            case 2:
                shooterRate *= 0.75f;
                break;
            case 3:
                harasserRate *= 0.75f;
                adjustCount = -1;
                break;
        }

        adjustCount++;
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
