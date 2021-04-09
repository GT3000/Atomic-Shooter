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
    [SerializeField] private float widthLimit;
    private float currentSpeed;
    [SerializeField] private SpriteRenderer currentThruster;
    [SerializeField] private Sprite[] thrusters;
    [SerializeField] private Projectile currentProjectile;
    [SerializeField] private Transform[] projectileLocations;
    private float fireTimePassed;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //tracks player relative to camera in world units
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, upperLimit, Camera.main.transform.position.z));
        print(screenPos);
        
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
        if (Input.GetMouseButton(0) && fireTimePassed >= currentProjectile.FireDelay)
        {
            foreach (var location in projectileLocations)
            {
                GameObject tempProjectile = Instantiate(currentProjectile.gameObject, location.position, Quaternion.identity);
            }

            fireTimePassed = 0f;
        }
    }

}
