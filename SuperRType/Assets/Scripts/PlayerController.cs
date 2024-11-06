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
    [SerializeField] private GameObject[] shoots;
    [SerializeField] private GameObject shootLoad;
    [SerializeField] private GameObject mainCamera;
    
    private float _hMove;
    private float _vMove;
    private Animator _animator;
    private Rigidbody2D _rb;
    private const float _SHOOT_TIME = 1f;
    private float _time;
    private bool _canUpdateTime;
    private float _cameraSpeed;

    private bool _isShootLoadActive;
    
    // Start is called before the first frame update
    void Start()
    {
        speedForce = 6.0f;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        _cameraSpeed = mainCamera.GetComponent<CameraController>().GetForwardSpeed();
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
        if (_hMove != 0 || _vMove != 0)
        {
            _rb.velocity = new Vector2(_hMove, _vMove).normalized * speedForce;
        }
        else
        {
            _rb.velocity = new Vector2(1, 0).normalized * _cameraSpeed; 
        }

    }

    private void GenerateShoot()
    {

        if (_canUpdateTime)
        {
            _time += Time.deltaTime;

            if (!_isShootLoadActive && _time > 0.2f)
            {
                shootLoad.SetActive(true);
                _isShootLoadActive = true;
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _canUpdateTime = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {

            if (_isShootLoadActive)
            {
                shootLoad.SetActive(false);
                _isShootLoadActive = false;
            }

            var currentShootType = _time < _SHOOT_TIME ? shoots[0] : shoots[1];
            var xOffset = _time < _SHOOT_TIME ? 0.8f : 1.7f;
            
            Instantiate(currentShootType,
                new Vector3(transform.position.x + xOffset,
                    transform.position.y - 0.2f,
                    0.00f),
                transform.rotation);

            _time = 0f;
            _canUpdateTime = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
            
       if (other.gameObject.CompareTag("playerLimit"))
       {
          transform.position = new Vector2(other.transform.position.x - 1.345f, transform.position.y);
       }
    }
}
