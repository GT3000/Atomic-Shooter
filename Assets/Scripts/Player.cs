using System.Collections;
using System.Collections.Generic;
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
    

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        currentSpeed = speed;
        
        InvokeRepeating("ThrusterControl", 0.1f, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        //tracks player relative to camera in world units
        screenPos = Camera.main.WorldToScreenPoint(transform.position);

        Movement();
    }

    private void Movement()
    {
        playerPos = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(playerPos * Time.deltaTime * currentSpeed);
        
        if (transform.position.y > upperLimit * 1.25f)
        {
            transform.position = new Vector3(transform.position.x, upperLimit * 1.25f);
        }
        else if (transform.position.y < -upperLimit)
        {
            transform.position = new Vector3(transform.position.x, -upperLimit);
        }

        //Stops at screen edge
        if (screenPos.x < 0f - transform.position.x)
        {
            if (playerPos.x > 0)
            {
                currentSpeed = speed;
            }
            else
            {
                currentSpeed = 0f;
            }
        }
        else if (screenPos.x > Screen.width - transform.position.x)
        {
            if (playerPos.x < 0)
            {
                currentSpeed = speed;
            }
            else
            {
                currentSpeed = 0f;
            }
        }
    }

    private void ThrusterControl()
    {
        if (currentThruster.sprite == thrusters[0])
        {
            currentThruster.sprite = thrusters[1];
        }
        else
        {
            currentThruster.sprite = thrusters[0];
        }
    }
}
