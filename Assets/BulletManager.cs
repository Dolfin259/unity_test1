using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    Rigidbody2D rb2d;

    public float speed = 10f;
    
    void Start()
    {
       rb2d = GetComponent<Rigidbody2D>();
       rb2d.velocity = transform.right * speed;
    }

    void Update()
    {
        
    }
}
