using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float xForce;
    [SerializeField] private float yForce;
    
    private Rigidbody2D _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.AddForce(new Vector2(xForce * Time.deltaTime, yForce * Time.deltaTime), ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Upper Limit"))
        {
            yForce = yForce * -1;
        }

        if (collision.gameObject.CompareTag("Lower Limit"))
        {
            yForce = yForce * 1;
        }
    }
    
    
}
