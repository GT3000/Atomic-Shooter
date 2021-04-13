using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    private Vector3 playerPos;
    private Vector3 screenPos;
    [SerializeField] private float speed;
    [SerializeField] private float upperLimit;
    private float currentSpeed;
    [SerializeField] private SpriteRenderer currentThruster;
    [SerializeField] private Sprite[] thrusters;
    [SerializeField] private Projectile currentProjectile;
    [SerializeField] private Transform[] projectileLocations;
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
        //Get direction of player based on player input
        playerPos = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(playerPos * Time.deltaTime * currentSpeed);
        
        //Controls upper boundary and prevents player from dropping below screen or above certain point
        if (transform.position.y > upperLimit)
        {
            transform.position = new Vector3(transform.position.x, upperLimit);
        }
        else if (transform.position.y < 0.5f)
        {
            transform.position = new Vector3(transform.position.x, 0.5f);
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

        //Thruster animation control, flips between current thruster sprites
        if (currentThruster.sprite == thrusters[0])
        {
            currentThruster.sprite = thrusters[1];
        }
        else
        {
            currentThruster.sprite = thrusters[0];
        }
    }

    //Controls player firing mechanic
    private void Fire()
    {
        //increment time
        fireTimePassed += Time.deltaTime;
        
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

}
