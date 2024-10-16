using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveFlag : MonoBehaviour
{
    
    private GameObject _player;
    private float _moveSpeed = 2f;
    private bool _moveFlag = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveFlag)
        {
            if (transform.position.y > _player.transform.position.y)
            {
                transform.Translate(Time.deltaTime * _moveSpeed * Vector3.down);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider2D>().enabled = _player.transform.position.y > transform.position.y;
            _moveFlag = true;
        }
    }
}
