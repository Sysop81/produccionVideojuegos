using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsDown = Animator.StringToHash("isDown");
    private static readonly int IsUp = Animator.StringToHash("isUp");

    [SerializeField] private float speedForce;
    [SerializeField] private GameObject[] shoots;
    [SerializeField] private GameObject shootLoad;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject explosion;
    private float _hMove;
    private float _vMove;
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private const float _SHOOT_TIME = 1f;
    private float _time;
    private bool _canUpdateTime;
    private  CameraController _cameraScript;
    private Camera _camera;
    private const float _X_LIMIT_OFFSET = 0.7f;
    private const float _Y_LIMIT_OFFSET = 0.4f;
    private bool _isShootLoadActive;
    private bool _isDead;
    private PlayerInput _playerInput;

    private bool _isOnScreenMovement;
    
    /// <summary>
    /// Method Start [Life cycle]
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        speedForce = 6.0f;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _playerInput = GetComponent<PlayerInput>();

        _cameraScript = mainCamera.GetComponent<CameraController>();
        _camera = mainCamera.GetComponent<Camera>();
    }

    /// <summary>
    /// Method Update [Life cycle]
    /// Update is called once per frame
    /// </summary>
    void FixedUpdate()
    {
        if (_cameraScript.IsMove())
        {
            // Check Player directions
            MoveDirection();
        }
        else
        {
            // Atomatic player move
            MoveXplayerAuto(_cameraScript.GetForwardSpeed());
        }
    }

    void Update()
    {
        // Shoot
        GenerateShoot();
    }

    public PlayerInput GetPlayerInput()
    {
        return _playerInput;
    }
    
    /// <summary>
    /// Method MoveDirection
    /// This method move the player and set animation
    /// </summary>
    private void MoveDirection()
    {
        // Get move axes
        if (!_isOnScreenMovement)
        {
            
            _hMove = _playerInput.actions["Horizontal"].ReadValue<float>(); // Input.GetAxis("Horizontal");
            _vMove = _playerInput.actions["Vertical"].ReadValue<float>();   // Input.GetAxis("Vertical");
        }
        
        
        // Manage animation
        if (_vMove > 0)
        {
            _animator.SetBool(IsUp, true);
        }else if (_vMove < 0)
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
            //_rb.velocity = new Vector2(_hMove, _vMove).normalized * speedForce;
            MovePlayer();
        }
        else
        {
            MoveXplayerAuto(_cameraScript.GetForwardSpeed());
        }
        
        // Manage screen camera player limits
        CheckLimits();
    }

    private void MovePlayer()
    {
        _rb.velocity = new Vector2(_hMove, _vMove).normalized * speedForce;
    }

    public void IsOnScreenMovement(bool value)
    {
        _isOnScreenMovement = value;
    }


    public void MoveUpLeft()
    {
        _hMove = -1; _vMove = 1;
        IsOnScreenMovement(true);
    }

    public void MoveUpRight()
    {
        _hMove = 1; _vMove = 1;
        Debug.Log(" *** mov : " + _hMove + " / " + _vMove);
        IsOnScreenMovement(true);
    }

    public void MoveDownLeft()
    {
        _hMove = -1; _vMove = -1;
        IsOnScreenMovement(true);
    }

    public void MoveDownRight()
    {
        _hMove = 1; _vMove = -1;
        IsOnScreenMovement(true);
    }
    
    /// <summary>
    /// Method MoveXplayerAuto
    /// Manages player and reset animator values
    /// </summary>
    /// <param name="xSpeed"></param>
    private void MoveXplayerAuto(float xSpeed)
    {
        _animator.SetBool(IsUp, false);
        _animator.SetBool(IsDown, false);
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
        
        if(_playerInput.actions["Fire"].IsPressed())
        {
            _canUpdateTime = true;
        }

        if (_playerInput.actions["Fire"].WasReleasedThisFrame())
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
        
       if (other.gameObject.CompareTag("LimitZone")) ManageGameOver();
       
       /*if (!_isDead && (other.gameObject.CompareTag("RockBase") || other.gameObject.CompareTag("Enemy")))
       {
           _isDead = true;
           Destroy(other.gameObject);
           //Tools.DrawExplosion(explosion,transform.position,6);
           ExplosionController.DrawExplosion(explosion,transform.position,6);
           _sr.enabled = false;
           Invoke("ManageGameOver", 1f);
       }*/
    }
    
    /// <summary>
    /// method ManageGameOver
    /// This method is calling when player get the level limit or when the player is dead
    /// </summary>
    private void ManageGameOver()
    {
        Time.timeScale = 0;
        Debug.Log("¡¡¡ End Game !!!");
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
