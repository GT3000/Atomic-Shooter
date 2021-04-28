using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject[] enemiesToSpawn;
    [SerializeField] private int totalEnemiesSpawned;
    [SerializeField] private int currentEnemiesSpawned;
    private GameObject enemiesSpawnedContainer;
    
    [Header("Powerups")]
    [SerializeField] private GameObject[] powerupsToSpawn;
    [SerializeField] private int totalPowerupsSpawned;
    [SerializeField] private int currentPowerupsSpawned;
    
    private Vector3 screenPos;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        
        enemiesSpawnedContainer = new GameObject("Enemies Spawned");
        
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerUps());
        
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemies()
    {
        while (player != null)
        {
            if (currentEnemiesSpawned < totalEnemiesSpawned)
            {
                int randomEnemy = Random.Range(0, enemiesToSpawn.Length - 1);
                float randomX = Random.Range(-screenPos.x, screenPos.x);
                
                Vector3 randomPos = new Vector3(randomX, screenPos.y, 0);

                GameObject tempEnemy = Instantiate(enemiesToSpawn[randomEnemy], randomPos, Quaternion.identity);
                tempEnemy.transform.parent = enemiesSpawnedContainer.transform;

                currentEnemiesSpawned++;
            }
            
            yield return new WaitForSeconds(5.0f);
        }

        yield return null;
    }
    
    private IEnumerator SpawnPowerUps()
    {
        if (currentPowerupsSpawned < totalPowerupsSpawned)
        {
            int randomPowerup = Random.Range(0, powerupsToSpawn.Length);
            float randomX = Random.Range(-screenPos.x, screenPos.x);
                
            Vector3 randomPos = new Vector3(randomX, screenPos.y, 0);

            GameObject tempPowerup = Instantiate(powerupsToSpawn[randomPowerup], randomPos, Quaternion.identity);
            
            yield return new WaitForSeconds(5.0f);

            currentPowerupsSpawned++;

            StartCoroutine(SpawnPowerUps());
        }

        yield return null;
    }
}
