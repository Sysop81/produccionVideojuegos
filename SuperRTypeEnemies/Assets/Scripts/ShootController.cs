using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    private Rigidbody2D _rb;
    private const int _DAMAGE = 1; 
    
    
    /// <summary>
    /// Method Start [Life cycle]
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Method Update [Lyfe cycle]
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        _rb.velocity = new Vector2(1, 0).normalized * speed;
    }
    
    /// <summary>
    /// Getter GetDamage
    /// </summary>
    /// <returns></returns>
    public int GetDamage()
    {
        return _DAMAGE;
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
}
