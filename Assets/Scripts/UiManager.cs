using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponCoolDownBlocks;
    [SerializeField] private GameObject[] currentHealthSprites;
    [SerializeField] private Sprite[] depletedHealthBar;
    [SerializeField] private Sprite[] filledHealthBar;

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

    public void HealthBar(int playerHealth)
    {
        for (int i = 0; i < currentHealthSprites.Length; i++)
        {
            if (playerHealth < i + 1)
            {
                currentHealthSprites[i].GetComponent<Image>().sprite =
                    depletedHealthBar[i];
            }
            else if (playerHealth >= i)
            {
                currentHealthSprites[i].GetComponent<Image>().sprite = filledHealthBar[i];
            }
        }
    }
}
