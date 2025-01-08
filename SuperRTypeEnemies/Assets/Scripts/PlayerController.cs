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
    private  CameraController _cameraScript;
    private Camera _camera;
    private const float _X_LIMIT_OFFSET = 0.7f;
    private const float _Y_LIMIT_OFFSET = 0.4f;
    private bool _isShootLoadActive;
    
    // Start is called before the first frame update
    void Start()
    {
        speedForce = 6.0f;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        _cameraScript = mainCamera.GetComponent<CameraController>();
        _camera = mainCamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cameraScript.IsMove())
        {
            // Check Player directions
            MoveDirection();
        }
        else
        {
            // Atomotic player move
            MoveXplayerAuto(_cameraScript.GetForwardSpeed());
        }
        
        // Shoot
        GenerateShoot();
    }
    
    /// <summary>
    /// Method MoveDirection
    /// This method move the player and set animation
    /// </summary>
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
            MoveXplayerAuto(_cameraScript.GetForwardSpeed());
        }
        
        // Manage screen camera player limits
        CheckLimits();
    }

    private void MoveXplayerAuto(float xSpeed)
    {
        _rb.velocity = new Vector2(1, 0).normalized * xSpeed; 
    }
    
    /// <summary>
    /// Method GenerateShoot
    /// This method generate and manage the player shoot [short and long]
    /// </summary>
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
    
    /// <summary>
    /// Trigger OnTriggerEnter2D
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        
       if (other.gameObject.CompareTag("LimitZone"))
       {
           Time.timeScale = 0;
           Debug.Log("¡¡¡ End Game !!!");
       }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("RockBase"))
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Method CheckLimits
    /// This method checks the player limits
    /// </summary>
    private void CheckLimits()
    {
        // Get display limits on world
        Vector3 xLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0.5f, _camera.nearClipPlane));
        Vector3 xRight = _camera.ViewportToWorldPoint(new Vector3(1, 0.5f, _camera.nearClipPlane));
        Vector3 yTop = _camera.ViewportToWorldPoint(new Vector3(0.5f, 1, _camera.nearClipPlane));
        Vector3 yDown = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0, _camera.nearClipPlane));

        // Apply offset to display limits 
        xLeft.x += _X_LIMIT_OFFSET;
        xRight.x -= _X_LIMIT_OFFSET;
        yTop.y -= _Y_LIMIT_OFFSET;
        yDown.y += _Y_LIMIT_OFFSET;
        
        // Get current player position
        Vector3 playerPosition = transform.position;

        // Update a player position 
        playerPosition.x = Mathf.Clamp(playerPosition.x, xLeft.x, xRight.x);
        playerPosition.y = Mathf.Clamp(playerPosition.y, yDown.y, yTop.y);

        // Apply changes to player transform position
        transform.position = playerPosition;
    }
}
