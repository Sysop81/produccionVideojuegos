using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float yForce;
    
    private PlayerInput _playerInput;
    private Rigidbody2D _rigidbody2D;
    private float _xOffset = 4.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        transform.position = new Vector3((_playerInput.user.id == 1 ? -_xOffset : _xOffset), transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    private void Move()
    {
        if (_playerInput.actions["UMove"].IsPressed())
        {
            _rigidbody2D.velocity = new Vector2(0.0f,yForce * Time.deltaTime);
        } else if (_playerInput.actions["DMove"].IsPressed())
        {
            _rigidbody2D.velocity = new Vector2(0.0f,-yForce * Time.deltaTime);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(0.0f,0.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            //if (collision.gameObject.transform.position.x < 0)
        }
    }
}
