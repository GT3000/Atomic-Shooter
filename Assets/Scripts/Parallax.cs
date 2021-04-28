using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Ludiq.PeekCore;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private float speed = 1.0f;
    private Vector3 screenPos;
    
    // Start is called before the first frame update
    void Start()
    {
        //Gets the current camera area in world space units and puts it into Vector3
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        BackgroundScroll();
    }
    
    // Controls scrolling of the background images
    public void BackgroundScroll()
    {
        //Cycle through each background image
        for (int i = 0; i < backgrounds.Length; i++)
        {
            //If the background image position + the size of the image is less 0 then move up to the top creating a seamless loop
            if (backgrounds[i].transform.position.y + (backgrounds[i].GetComponent<Image>().sprite.bounds.extents.y * 1.99f) < 0)
            {
                backgrounds[i].transform.position = new Vector3(0,
                    screenPos.y + backgrounds[i].GetComponent<Image>().sprite.bounds.extents.y);
            }
            //otherwise move down per the speed set until told otherwise
            else
            {
                backgrounds[i].gameObject.transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
        }
    }
}
