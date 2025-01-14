using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController2_old : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float forwardSpeed = 1.0f;

    private GameObject _explosion;
    private SpriteRenderer  _sp;
    
    // Start is called before the first frame update
    void Start()
    {
        _explosion = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "Explosion");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        forwardSpeed = Random.Range(0.5f, 4f);
        _sp = GetComponent<SpriteRenderer>();
        //InvokeRepeating("MoveEnemy",0f,0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate( forwardSpeed * Time.deltaTime * Vector2.left);
        
        Vector2 direction = (playerTransform.position - new Vector3(transform.position.x + 0.2f,transform.position.y + 0.2f, transform.position.z)).normalized;
        _sp.flipX = playerTransform.transform.position.x > transform.position.x;
        transform.position +=  forwardSpeed * Time.deltaTime * (Vector3)direction;
    }

    private void MoveEnemy()
    {
        
        /*var verticalMove = (playerTransform.position.y > transform.position.y ? Vector3.up : Vector3.down) * 0.5f;
        
        Debug.Log((playerTransform.position.y > transform.position.y  ? "player por encima" : "player por debajo") + " pos -> " + verticalMove);
        transform.Translate(verticalMove);*/
        
        
        var yMove = playerTransform.position.y > transform.position.y ? 0.5f : -0.5f;
        transform.position=new Vector3(transform.position.x, transform.position.y + yMove, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("DeathZone")) Destroy(gameObject);
        
        if (other.CompareTag("Shoot"))
        {
            for (int i = 0; i < 4; i++)
            {
                Instantiate(_explosion, Tools.GetAleatoryTranformPosition(transform.position,Random.Range(0.5f,1f)), Quaternion.identity);
            }
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
