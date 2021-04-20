using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownExample : MonoBehaviour
{
    private float timeBetweenShots = 1.0f;
    private float timePassed;
    [SerializeField] private Slider cooldownSlider;
    private bool inCooldown = false;
    
    // Start is called before the first frame update
    void Start()
    {
        cooldownSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    private void Fire()
    {
        //Add to timePassed
        timePassed += Time.deltaTime;
        
        //When player clicks on mouse button and timePassed is greater than timeBetweenShots fire a sphere
        //First reset timePassed, then create sphere, add a rigidbody, turn off gravity, and add a force to shoot it up
        //Deducts from cooldown gauge until it depletes while it's not in cooldown and then it prevents shots until gauge is filled
        if (Input.GetMouseButtonDown(0) && timePassed >= timeBetweenShots && !inCooldown)
        {
            timePassed = 0f;

            GameObject tempShot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempShot.AddComponent<Rigidbody>();
            tempShot.GetComponent<Rigidbody>().useGravity = false;
            tempShot.GetComponent<Rigidbody>().AddForce(Vector3.up * 25, ForceMode.Impulse);

            cooldownSlider.value -= 0.25f;

            if (cooldownSlider.value <= 0)
            {
                inCooldown = true;
            }
        }
        else if (inCooldown && cooldownSlider.value != 1)
        {
            cooldownSlider.value += Time.deltaTime;

            if (cooldownSlider.value >= 1)
            {
                inCooldown = false;
            }
        }
    }
}
