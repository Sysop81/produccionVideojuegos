using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlienController : EnemyController
{
    
    private Transform _playerTransform;
    
    
    void Awake()
    {
        Intialize();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ForwardSpeed = Random.Range(0.5f, 4f);
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (_playerTransform.position - new Vector3(transform.position.x + 0.2f,transform.position.y + 0.2f, transform.position.z)).normalized;
        SpriteRenderer.flipX = _playerTransform.transform.position.x > transform.position.x;
        transform.position +=  ForwardSpeed * Time.deltaTime * (Vector3)direction;
    }
}
