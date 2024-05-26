using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{
    public Transform bulletPoint;
    public GameObject bullet;
    float timeBetween;
    public float startTimeBetween;
    // Start is called before the first frame update
    void Start()
    {
        timeBetween = startTimeBetween;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBetween <= 0)
        {
            Instantiate(bullet,bulletPoint.position,bulletPoint.rotation);
            timeBetween = startTimeBetween;
        }
        else
        {
            timeBetween -= Time.deltaTime;
        }
    }
}




