using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BallController : MonoBehaviour
{
    //[SerializeField] private float xForce;
    //[SerializeField] private float yForce;
    
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    public Vector3 _initialPosition;
    public Vector2 _direction;
    private GameManager _gameManager;
    private readonly float _speed = 8.0f;
    private readonly float _playerSpeedMultiplier = 3.0f;
    public bool _isResetBall;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _initialPosition = transform.position;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
        //_direction = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
        ManageSpawnDirection(true);

        //_rb.AddForce(/*new Vector2(xForce * Time.deltaTime, yForce * Time.deltaTime)*/ 200 * _direction, ForceMode2D.Force);
        
        //_rb.AddForce(_direction * _speed, ForceMode2D.Impulse);
    }

    public void InitializeBall()
    {
        //_rb.AddForce(_direction * _speed, ForceMode2D.Impulse);
        _rb.velocity = _direction * _speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameManager.gameState == GameState.Loading || _isResetBall) return;
        if(!_spriteRenderer.enabled) _spriteRenderer.enabled = true;
        //_rb.AddForce(/*new Vector2(xForce * Time.deltaTime, yForce * Time.deltaTime)*/ 50 * Time.deltaTime  * _direction);
        //transform.Translate( _speed * Time.deltaTime * _direction);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.CompareTag("Upper Limit") || collision.gameObject.CompareTag("Lower Limit"))
        {
            _direction = new Vector2(_direction.x, -_direction.y).normalized;
        }*/

        if (collision.gameObject.CompareTag("Left Limit") || collision.gameObject.CompareTag("Right Limit"))
        {
            //_direction = new Vector2(-_direction.x, _direction.y).normalized;
            StartCoroutine(ResetBall());
            _gameManager.UpdateScore(1,collision.gameObject.CompareTag("Left Limit"));
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {
            var pRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("Player Y velocity --> " + pRb.velocity.y);
            var yForce = 2.0f;
            
            if (pRb.velocity.y < 0)
            {
                Debug.Log("Player velocity negativa");
                yForce = -_direction.y; // * -2.0f;
            }else if (pRb.velocity.y > 0)
            {
                Debug.Log("Player velocity positiva");
                yForce = _direction.y; //* 2.0f;
            }
            else
            {
                Debug.Log("Player velocity neutro");
                yForce = _direction.y; //0.0f;
            }

            var pId = collision.gameObject.GetComponent<PlayerInput>().user.id;
            var dirX = Mathf.Abs(_direction.x); //* 10f;  // pId == 1 ? _direction.x * 10f : -_direction.x * 10f;
            //Debug.Log("**** DIRECCION ---> " + (pId == 1 ? dirX : -dirX) + " player --> " + pId);
            //Debug.Log(Mathf.Abs(_direction.x));
            //_rb.velocity = new Vector2( (pId == 1 ? dirX : -dirX), yForce) * 10f;
            _rb.AddForce(-_direction);
            //_direction = new Vector2(-_direction.x * _playerSpeedMultiplier, yForce/*_direction.y*/).normalized;*/
            //_rb.AddForce(new Vector2(- (_direction.x * _playerSpeedMultiplier), yForce/*_direction.y*/) , ForceMode2D.Impulse);
        }

        //_rb.AddForce(/*new Vector2(xForce * Time.deltaTime, yForce * Time.deltaTime)*/ 200 * _direction, ForceMode2D.Force);
    }


    IEnumerator ResetBall()
    {
        _rb.velocity = Vector2.zero;
        _isResetBall = true;
        //_spriteRenderer.enabled = false;
        transform.position = _initialPosition;
        yield return new WaitForSeconds(1.0f);
        //_direction = new Vector2(Random.Range(-0.5f, 0.5f), -0.5f).normalized;
        ManageSpawnDirection();
        //_spriteRenderer.enabled = true;
        _isResetBall = false;
        InitializeBall();
        //_rb.AddForce(new Vector2(0,0), ForceMode2D.Force);
    }

    private void ManageSpawnDirection(bool isFirstLoad = false)
    {

        bool isLeftSide = Random.Range(1, 101) % 2 == 0;
        var xValue  = Random.Range(0.5f, 1.0f);
        var yValue = isFirstLoad ? 1.0f : Random.Range(-1f, -0.1f);
        _direction = new Vector2(isLeftSide ? -xValue : xValue/*Random.Range(-1f, 1f)*/, /*Random.Range(-1f, -0.1f)*/ yValue).normalized;
    }
    
    
    
    
}
