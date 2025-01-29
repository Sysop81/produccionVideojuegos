using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BallController : MonoBehaviour
{
    
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _initialPosition;
    private Vector2 _direction;
    private GameManager _gameManager;
    private readonly float _speed = 8.0f;
    private readonly float _speedImpulse = 1.1f;
    private readonly float MAX_SPEED = 10.0f;
    private bool _isImpulseMode = true; // Default value
    
    
    /// <summary>
    /// Method Start
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        // Get initial randon direction
        ManageSpawnDirection(true);
    }

    
    /// <summary>
    /// Trigger OnCollisionEnter2D
    /// </summary>
    /// <param name="collision">Collision gameObject</param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle left and right collision sides
        if (collision.gameObject.CompareTag("Left Limit") || collision.gameObject.CompareTag("Right Limit"))
        {
            // Reset the ball and update the game marker
            StartCoroutine(ResetBall());
            _gameManager.UpdateScore(1,collision.gameObject.CompareTag("Left Limit"));
        }
        
        // Player collision
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1º Simulated mode
            if (_isImpulseMode)
            {
                ManageBall();
                return;
            }
            // 2º Arcade mode
            ManageBall(collision, collision.gameObject.GetComponent<PlayerInput>().user.id == 1 ? 1 : -1);
        }
    }
    
    /// <summary>
    /// Method ManageBall [Simulated]
    /// This method apply impulse force to the rigidbody and set a max speed control
    /// </summary>
    private void ManageBall()
    {
        _rb.AddForce(_direction,ForceMode2D.Impulse);
        _rb.velocity *= _speedImpulse;
            
        if (_rb.velocity.magnitude > MAX_SPEED)
            _rb.velocity = _rb.velocity.normalized * MAX_SPEED;
    }
    
    /// <summary>
    /// Method ManageBall [Arcade]
    /// This method applies a fixed force to the ball. Change the direction of the x axis to the one received as a
    /// parameter and adjust the y axis depending on where the ball hit the player.
    /// </summary>
    /// <param name="player">player gameObject</param>
    /// <param name="x">X axis to apply the new move</param>
    private void ManageBall(Collision2D player, int x)
    {
        float yDif = transform.position.y - player.gameObject.transform.position.y;
        float playerSizeY = player.collider.bounds.size.y;
        float y = yDif / playerSizeY;
        _rb.velocity = new Vector2(x, y) * MAX_SPEED;
    }
    
    
    /// <summary>
    /// IEnumerator ResetBall [Corrutine]
    /// This corrutine handle the reset of the ball
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetBall()
    {
        _rb.velocity = Vector2.zero;
        transform.position = _initialPosition;
        yield return new WaitForSeconds(1.0f);
        ManageSpawnDirection();
        InitializeBall();
    }
    
    /// <summary>
    /// Method ManageSpawnDirection
    /// This method set the randon direction to the ball.
    /// </summary>
    /// <param name="isFirstLoad"></param>
    private void ManageSpawnDirection(bool isFirstLoad = false)
    {
        // Handle left or right side ( multiple of 2 --> left side )
        bool isLeftSide = Random.Range(1, 101) % 2 == 0;
        
        // Get random values
        var xValue  = Random.Range(0.5f, 1.0f);
        var yValue = isFirstLoad ? 0.3f : -0.3f;
        
        // Set the Vector2 direction
        _direction = new Vector2(isLeftSide ? -xValue : xValue,yValue).normalized;
    }
    
    /// <summary>
    /// Method InitializeBall
    /// This method initialize the ball movement
    /// </summary>
    public void InitializeBall()
    {
        if(!_spriteRenderer.enabled) _spriteRenderer.enabled = true;
        _rb.velocity = _direction * _speed;
    }
    
    /// <summary>
    /// Setter SetImpulseType
    /// </summary>
    /// <param name="isImpulse">Impulse type. Simulaterd or Arcade</param>
    public void SetImpulseType(bool isImpulse)
    {
        _isImpulseMode = isImpulse;
    }
}
