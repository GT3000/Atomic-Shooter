using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float weaponHeat;
    [SerializeField] private bool producesHeat;
    [SerializeField] private float fireDelay;
    [SerializeField] private float timeDelayMin;
    [SerializeField] private float timeDelayMax;
    [SerializeField] private bool variableTimeDelay = false;
    private float timePassed;
    private float timeDelayRange;

    public float FireDelay => fireDelay;
    public float WeaponHeat => weaponHeat;
    public bool ProducesHeat => producesHeat;

    // Start is called before the first frame update
    void Start()
    {
        if (variableTimeDelay)
        {
            timeDelayRange = Random.Range(timeDelayMin, timeDelayMax);
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
        if (variableTimeDelay)
        {
            if (timePassed >= timeDelayRange)
            {
                DestroyProjectile();
            }
        }
        else
        {
            if (timePassed >= timeDelayMax)
            {
                DestroyProjectile();
            }
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
