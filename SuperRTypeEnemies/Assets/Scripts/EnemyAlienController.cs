using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlienController : EnemyController
{
    
    private Transform _playerTransform;
    
    /// <summary>
    /// Method Awake [Life cycles]
    /// </summary>
    void Awake()
    {
        Intialize();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    /// <summary>
    /// Method Start [Lyfe cycles]
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        ForwardSpeed = Random.Range(0.5f, 4f);
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// Method Update
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        Vector2 direction = (_playerTransform.position - new Vector3(transform.position.x + 0.2f,transform.position.y + 0.2f, transform.position.z)).normalized;
        SpriteRenderer.flipX = _playerTransform.transform.position.x > transform.position.x;
        transform.position +=  ForwardSpeed * Time.deltaTime * (Vector3)direction;
    }
}
