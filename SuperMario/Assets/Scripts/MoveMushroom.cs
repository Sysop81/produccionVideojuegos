using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMush : MonoBehaviour
{

    
    private float _moveSpeed = 1.3f;
    private Vector3 _startPos = Vector3.right;
    
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(100f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * _moveSpeed * _startPos);
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            _startPos = Vector3.left;
        }
    }
}
