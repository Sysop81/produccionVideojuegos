using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsHmove = Animator.StringToHash("isHmove");
    private static readonly int IsVmove = Animator.StringToHash("isVmove");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    
    [SerializeField] private float speedForce;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] GameManager gameManager;
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private CircleCollider2D _col;
    private float _hMove = -1;
    private float _vMove;
    private Vector2 _movement;
    private bool _hasPowerUp;
    private Vector3 _initialTransform;
    private bool _isEndInitialMovement;
    
    /// <summary>
    /// Method Start
    /// The method Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _initialTransform = transform.position;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<CircleCollider2D>();
        gameManager.StartGame();
        _sr.flipX = true;
        StartCoroutine(LaunchInitialMovement());
    }

    /// <summary>
    /// Method Update
    /// This method is called once per frame
    /// </summary>
    void Update()
    {
        if(gameManager.gameState == GameState.GameOver) Destroy(gameObject);
        
        if (gameManager.gameState == GameState.InGame) MoveDirection();
    }

    
    /// <summary>
    /// Method MoveDirection
    /// This method manages the player moves and animations
    /// </summary>
    private void MoveDirection()
    {
        // Check to end the initial left movement
        if (!_isEndInitialMovement) return;
        
        // Get move axes
        _hMove = Input.GetAxis("Horizontal");
        _vMove = Input.GetAxis("Vertical");

        // Avoid diagonal movement. If there any horizontal move -1 or 1 the vertical move is equal to zero 
        // Preference to horizontal move
        if (Mathf.Abs(_hMove) > 0) _vMove = 0;

        // Set the normalized directional Vector2. Similar to struct Vector2.left, Vector2.UP...
        Vector2 direction = new Vector2(_hMove, _vMove).normalized;
        
        // Checking movement. Only move if RayCast hits return true
        if (!CheckMovement(direction)) return;
        
        // Calculate the movement.
        _movement = direction * speedForce;

        // Manage animations
        if (_hMove != 0)
        {
            _animator.SetBool(IsHmove, true);
            _animator.SetBool(IsVmove, false);
            _sr.flipX = _hMove < 0;
            _rb.velocity = _movement;
        }
        else if (_vMove != 0)
        {
            _animator.SetBool(IsVmove, true);
            _animator.SetBool(IsHmove, false);
            _sr.flipY = _vMove > 0;
            _rb.velocity = _movement;
        }
    }
    
    
    /// <summary>
    /// Method CheckMovement
    /// This method check if the player movement hit with a wall mask or not
    /// </summary>
    /// <param name="pDirection">Direction to move the player</param>
    /// <returns></returns>
    private bool CheckMovement(Vector2 pDirection) 
    {
        // Calcaulate collider transform + offset to set a raycast position
        Vector2 colliderCenter = (Vector2)transform.position + _col.offset;
        Vector2 offset = Vector2.Perpendicular(pDirection).normalized * 0.4f;
        
        // Build position 
        Vector2 position1 = colliderCenter + offset;
        Vector2 position2 = colliderCenter - offset;
        Vector2 position3 = colliderCenter;
        
        // Instantiate three raycast hit to detect wall collider (wallMask)
        RaycastHit2D hit1 = Physics2D.Raycast(position1, pDirection, 1.0f, wallMask);
        RaycastHit2D hit2 = Physics2D.Raycast(position2, pDirection, 1.0f, wallMask);
        RaycastHit2D hit3 = Physics2D.Raycast(position3, pDirection, 1.0f, wallMask);
        
        // [Optional] Draw a ray on scene
        Debug.DrawRay(position1, pDirection * 1f, Color.red);
        Debug.DrawRay(position2, pDirection * 1f, Color.red);
        Debug.DrawRay(position3, pDirection * 1f, Color.green);
        
        // Manage return hit or not hit with wall mask
        return !hit1.collider && !hit2.collider && !hit3.collider;
    }
    

    /// <summary>
    /// Triggeer OnTriggerEnter2D 
    /// </summary>
    /// <param name="other">Trigger gameObject</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("MagicDoor"))
        {
            // Manage a transform position when player arrives to left o right magic door
            transform.position = new Vector3(other.name.Contains("L") ? 23f : -7f,
                other.transform.position.y, other.transform.position.z);
        }
        
        if (other.CompareTag("Pellets") || other.CompareTag("BigPellets"))
        {
            gameManager.UpdateScore(100);
            if (other.CompareTag("BigPellets")) StartCoroutine(ManagePowerUp());
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.CompareTag("Ghost"))
        {
            if (!_hasPowerUp)
            {
                _rb.velocity = Vector2.zero;
                gameManager.UpdateGhostVisibility(false);
                StartCoroutine(ManagePlayerDeath());
                return;
            }
            
            other.gameObject.GetComponent<GhostController>().SetIsDead(true);
        }
    }
    
    
    /// <summary>
    /// IEnumerator ManagePlayerDeath [Corrutine]
    /// Manage the player animation state
    /// </summary>
    /// <returns></returns>
    IEnumerator ManagePlayerDeath()
    {
        
        _animator.SetTrigger(IsDead);
        yield return new WaitForSeconds(2f);
        _animator.SetBool(IsHmove, true);
        _animator.SetBool(IsVmove, false);
        transform.position = _initialTransform;
        gameManager.GameOver();
        gameManager.UpdateGhostVisibility(true);
    }
    
    
    /// <summary>
    /// IEnumerator ManagePowerUp [Corrutine]
    /// Manage the player powerUp
    /// </summary>
    /// <returns></returns>
    IEnumerator ManagePowerUp()
    {
        _hasPowerUp = true;
        yield return new WaitForSeconds(gameManager.GetPowerUpTime());
        _hasPowerUp = false;
    }
    
    
    /// <summary>
    /// IEnumerator LaunchInitialMovement [Corrutine]
    /// Manage the player initial movement
    /// </summary>
    /// <returns></returns>
    IEnumerator LaunchInitialMovement()
    {
        yield return new WaitForSeconds(1.0f);
        _movement = new Vector2(_hMove, 0).normalized * speedForce;
        _rb.velocity = _movement;
        _isEndInitialMovement = true;
    }

    
    /// <summary>
    /// Getter GetHasPowerUp
    /// </summary>
    /// <returns></returns>
    public bool GetHasPowerUp()
    {
        return _hasPowerUp;
    }
}
