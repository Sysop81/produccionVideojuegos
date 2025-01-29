using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float yForce;
    
    private PlayerInput _playerInput;
    private Rigidbody2D _rigidbody2D;
    private float _xOffset = 4.0f;
    private GameManager _gameManager;
    
    /// <summary>
    /// Method Start
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        // Set player on the correct screen position. P1 --> LEFT SIDE & P2 --> RIGHT SIDE
        var playerId = _playerInput.user.id; 
        transform.position = new Vector3((playerId == 1 ? -_xOffset : _xOffset), transform.position.y, transform.position.z);
        _gameManager.SetPlayerActive(true, playerId == 1);
    }

    /// <summary>
    /// Method Update
    /// Update is called once per frame
    /// </summary>
    void FixedUpdate()
    {
        Move();
    }
    
    /// <summary>
    /// Mthod Move
    /// This method handle the player y axis move
    /// </summary>
    private void Move()
    {
        if (_playerInput.actions["UMove"].IsPressed())
        {
            _rigidbody2D.velocity = Vector2.up * yForce; 
        } else if (_playerInput.actions["DMove"].IsPressed())
        {
            _rigidbody2D.velocity = Vector2.down * yForce;
        }
        else
        {
            _rigidbody2D.velocity = Vector2.zero; 
        }
    }
}
