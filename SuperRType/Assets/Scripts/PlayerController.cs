using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsDown = Animator.StringToHash("isDown");
    private static readonly int IsUp = Animator.StringToHash("isUp");

    [SerializeField] private float speedForce;
    //[SerializeField] private GameObject shootPrefab;
    [SerializeField] private GameObject[] shoots;
    
    private float _hMove;
    private float _vMove;
    private Animator _animator;
    private Rigidbody2D _rb;
    private const float _SHOOT_TIME = 1f;
    private float _time;
    private bool _canUpdateTime;
    
    // Start is called before the first frame update
    void Start()
    {
        speedForce = 6.0f;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check Player directions
        MoveDirection();
        
        // Shoot
        GenerateShoot();
    }

    private void MoveDirection()
    {
        // Get move axes
        _hMove = Input.GetAxis("Horizontal");
        _vMove = Input.GetAxis("Vertical");
        
        // Manage animation
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _animator.SetBool(IsUp, true);
        }else if (Input.GetKey(KeyCode.DownArrow))
        {
            _animator.SetBool(IsDown, true);
        }
        else
        {
            _animator.SetBool(IsUp, false);
            _animator.SetBool(IsDown, false);
        }
        
        // Change the Rigidbody velocity 
        if (_hMove != 0)
        {
            _rb.velocity = new Vector2(_hMove, _vMove).normalized * speedForce;
        }
        else
        {
            _rb.velocity = new Vector2(1,_vMove).normalized * 1f;
        }

    }

    private void GenerateShoot()
    {
        
        if(_canUpdateTime) _time += Time.deltaTime;
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _canUpdateTime = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            
            //Debug.Log(_time + " < " + _SHOOT_TIME +" = " + (_time < _SHOOT_TIME));
            var currentShootType = _time < _SHOOT_TIME ? shoots[0] : shoots[1];
            
            Instantiate(currentShootType,
                new Vector3(transform.position.x + 0.8f,
                    transform.position.y - 0.2f,
                    0.00f),
                transform.rotation);

            _time = 0f;
            _canUpdateTime = false;
        }
    }
}
