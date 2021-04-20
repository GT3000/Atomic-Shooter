using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float weaponHeat;
    [SerializeField] private bool producesHeat;
    [SerializeField] private float fireDelay;
    [SerializeField] private float lifetimeMin;
    [SerializeField] private float lifetimeMax;
    [SerializeField] private bool variableLifetime = false;
    private float timePassed;
    private float lifetimeRange;

    public float FireDelay => fireDelay;
    public float WeaponHeat => weaponHeat;
    public bool ProducesHeat => producesHeat;

    // Start is called before the first frame update
    void Start()
    {
        if (variableLifetime)
        {
            lifetimeRange = Random.Range(lifetimeMin, lifetimeMax);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        timePassed += Time.deltaTime;
        
        TimeDestroy();
    }

    private void TimeDestroy()
    {
        if (variableLifetime)
        {
            if (timePassed >= lifetimeRange)
            {
                DestroyProjectile();
            }
        }
        else
        {
            if (timePassed >= lifetimeMax)
            {
                DestroyProjectile();
            }
        }
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            DestroyProjectile();
        }
    }
}
