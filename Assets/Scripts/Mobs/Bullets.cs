using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int dir = 1;
    public float speed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(dir * speed, rb.velocity.x);
        Destroy(gameObject,4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
