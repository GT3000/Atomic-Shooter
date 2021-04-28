using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType {TripleShot, SpeedBoost, Shield}

public class Powerup : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [SerializeField] private PowerUpType currentPowerUp;

    [Header("Speed Boost")] 
    [SerializeField] private float speedMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        
        if (transform.position.y <= 0)
        {
            DestroyPowerup();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            switch (currentPowerUp)
            {
                case PowerUpType.TripleShot:
                    other.GetComponent<Player>().ActivateTripleShot(duration);
                    break;
                
                case PowerUpType.SpeedBoost:
                    other.GetComponent<Player>().ActivateSpeedBoost(duration, speed);
                    break;
                
                case PowerUpType.Shield:
                    //Set shield
                    break;
            }
            
            if (currentPowerUp == PowerUpType.TripleShot)
            {
                other.GetComponent<Player>().ActivateTripleShot(duration);
            }
            
            DestroyPowerup();
        }
    }

    private void DestroyPowerup()
    {
        Destroy(gameObject);
    }
}
