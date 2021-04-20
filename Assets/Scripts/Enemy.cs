using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private float speed;
    private Vector3 randomPos;
    private Vector3 screenPos;

    // Start is called before the first frame update
    void Start()
    {
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= 0)
        {
            float randomX = Random.Range(-screenPos.x, screenPos.x);
            randomPos = new Vector3(randomX, screenPos.y, 0);
            transform.position = randomPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Projectile>())
        {
            DamageTaken(1);
        }

        if (other.gameObject.GetComponent<Player>())
        {
            DamageTaken(1000);
        }
    }

    private void DamageTaken(int damageValue)
    {
        health -= damageValue;
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
