using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{

    [SerializeField] private float speed = 10.0f;
    private Rigidbody2D _rb;
    private const int DAMAGE = 1; 
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector2(1, 0).normalized * speed;
    }
    
    /// <summary>
    /// Trigger OnTriggerEnter2D
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LimitZone")) 
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("RockBase"))
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<EnemyBaseController>().UpdateLife(DAMAGE);
        }
    }
}
