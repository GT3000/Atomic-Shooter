using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    private Vector3 playerPos;
    private Vector3 screenPos;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float upperLimit;
    [SerializeField] private float lowerLimit;
    private float currentSpeed;

    [Header("Projectiles")]
    [SerializeField] private Projectile currentProjectile;
    [SerializeField] private Transform[] projectileLocations;
    [SerializeField] private Transform specialProjectileLocation;
    
    [Header("Powerups")]
    [SerializeField] private bool tripleShot = false;
    [SerializeField] private bool speedBoost = false;
    [SerializeField] private GameObject speedBoostThrusters;
    private float tripleShotDuration = 0f;
    [SerializeField] private float speedBoostDuration = 0f;
    
    [Header("Heat System")]
    [SerializeField] private float currentHeat = 0f;
    [SerializeField] private float totalHeat = 10f;
    private bool overHeated = false;
    private float heatTickDown = 0f;
    private float fireTimePassed;
    private UiManager uiManager;



    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        currentSpeed = speed;
        uiManager = FindObjectOfType<UiManager>();
        
        uiManager.HealthBar(health);
    }

    // Update is called once per frame
    void Update()
    {
        //tracks player relative to camera in world units
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, upperLimit, Camera.main.transform.position.z));

        Movement();
        Fire();
    }

    //Controls Player Movement
    private void Movement()
    {
        speedBoostDuration -= Time.deltaTime;
        
        if (speedBoost && speedBoostDuration <= 0)
        {
            currentSpeed = speed;
            speedBoost = false;
            speedBoostThrusters.SetActive(false);
        }
        
        //Get direction of player based on player input
        playerPos = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(playerPos * Time.deltaTime * currentSpeed);
        
        //Controls upper boundary and prevents player from dropping below screen or above certain point
        if (transform.position.y > upperLimit)
        {
            transform.position = new Vector3(transform.position.x, upperLimit);
        }
        else if (transform.position.y < lowerLimit)
        {
            transform.position = new Vector3(transform.position.x, lowerLimit);
        }

        //Stops at screen edge to the left or right of player 
        if (playerPos.x <= screenPos.x)
        {
            Vector3 newPos = transform.position;
            newPos.x = Mathf.Clamp(transform.position.x, -screenPos.x + (transform.localScale.x / 2), screenPos.x - (transform.localScale.x / 2));
            newPos.y = transform.position.y;
            transform.position = newPos;
        }
        else if (playerPos.x >= screenPos.x)
        {
            Vector3 newPos = transform.position;
            newPos.x = Mathf.Clamp(transform.position.x, -screenPos.x + (transform.localScale.x / 2), screenPos.x - (transform.localScale.x / 2));
            newPos.y = transform.position.y;
            transform.position = newPos;
        }
    }

    //Controls player firing mechanic
    private void Fire()
    {
        //increment time
        fireTimePassed += Time.deltaTime;
        tripleShotDuration -= Time.deltaTime;
        
        //When timer has elapsed firing delay then fire projectile(s) at firing point(s) and reset the timer
        if (Input.GetMouseButton(0) && fireTimePassed >= currentProjectile.FireDelay && !overHeated)
        {
            foreach (var location in projectileLocations)
            {
                GameObject tempProjectile = Instantiate(currentProjectile.gameObject, location.position, Quaternion.identity);

                Projectile currentTempProjectile = tempProjectile.GetComponent<Projectile>();
                
                if (currentTempProjectile.ProducesHeat)
                {
                    if (currentHeat <= totalHeat)
                    {
                        currentHeat += currentTempProjectile.WeaponHeat;

                        uiManager.WeaponCoolDown((int)currentHeat);
                    }
                    else
                    {
                        currentHeat = totalHeat;
                        overHeated = true;
                    }
                }
            }
            
            //If tripleshot is activated then start firing current projectile through the special fire slot. This will still work with addon weapons
            if (tripleShot && tripleShotDuration >= 0)
            {
                GameObject tempProjectile = Instantiate(currentProjectile.gameObject, specialProjectileLocation.position, Quaternion.identity);

                Projectile currentTempProjectile = tempProjectile.GetComponent<Projectile>();
                
                if (currentTempProjectile.ProducesHeat)
                {
                    if (currentHeat <= totalHeat)
                    {
                        currentHeat += currentTempProjectile.WeaponHeat;

                        uiManager.WeaponCoolDown((int)currentHeat);
                    }
                    else
                    {
                        currentHeat = totalHeat;
                        overHeated = true;
                    }
                }

                if (tripleShotDuration < 0)
                {
                    tripleShot = false;
                    tripleShotDuration = 0;
                }
            }

            fireTimePassed = 0f;
        }
        else
        {
            heatTickDown += Time.deltaTime;

            if (heatTickDown >= 0.9f && currentHeat >= 0)
            {
                currentHeat--;
                heatTickDown = 0;

                if (currentHeat < 4)
                {
                    overHeated = false;
                }
                
                if (currentHeat <= 0)
                {
                    currentHeat = 0;
                }
                
                uiManager.WeaponCoolDown((int)currentHeat);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            DamagePlayer();
        }
    }

    private void DamagePlayer()
    {
        health--;
        
        uiManager.HealthBar(health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ActivateTripleShot(float durationLeft)
    {
        tripleShot = true;
        tripleShotDuration = durationLeft;
    }

    public void ActivateSpeedBoost(float durationLeft, float speedMultiplier)
    {
        if (!speedBoost)
        {
            speedBoost = true;
            speedBoostDuration = durationLeft;
            currentSpeed *= speedMultiplier;
            speedBoostThrusters.SetActive(true); 
        }
        else
        {
            speedBoostDuration = durationLeft;
        }
        
    }
}
