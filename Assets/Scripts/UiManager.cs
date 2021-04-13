using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponCoolDownBlocks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WeaponCoolDown(float weaponHeat)
    {
        for (int i = 0; i < weaponCoolDownBlocks.Length; i++)
        {
            if (weaponHeat > i)
            {
                weaponCoolDownBlocks[i].SetActive(false);
            }
            else if (weaponHeat <= i)
            {
                weaponCoolDownBlocks[i].SetActive(true);
            }
        }
    }
}
