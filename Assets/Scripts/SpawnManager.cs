using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesToSpawn;
    [SerializeField] private int totalEnemiesSpawned;
    [SerializeField] private int currentEnemiesSpawned;
    private Vector3 screenPos;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        StartCoroutine(SpawnEnemies());
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
                tempEnemy.transform.parent = transform;

                currentEnemiesSpawned++;
            }
            
            yield return new WaitForSeconds(5.0f);
        }

        yield return null;
    }
}
