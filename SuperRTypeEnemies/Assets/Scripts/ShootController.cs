using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    private Rigidbody2D _rb;
    private const int DAMAGE = 1; 
    //private GameObject _explosion;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_explosion = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "Explosion");
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
            //LoadExplosion(GetAleatoryTranformPosition(other.gameObject.transform.position));
        }
    }

    /*private void LoadExplosion(Vector3 position)
    {
        //Instantiate(_explosion, position, Quaternion.identity);
        
        for (int i = 0; i < 6; i++)
        {
            Instantiate(_explosion, position/*GetAleatoryTranformPosition(), Quaternion.identity);
        }
    }
    
    private Vector3 GetAleatoryTranformPosition(Vector3 position)
    {
        var offset = 0.5f;
        var x = Random.Range(position.x -offset, position.x + offset);
        var y = Random.Range(position.y -offset, position.y + offset);
        
        return new Vector3(x,y,transform.position.z);
    }*/
}
