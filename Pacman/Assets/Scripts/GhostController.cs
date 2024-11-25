using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class GhostController : MonoBehaviour
{
    
    [SerializeField] private LayerMask wallMask;
    private Animator _animator;
    private Rigidbody2D _rb;
    private int _hMove,_vMove;
    private float[] _movements;
    // Start is called before the first frame update
    void Start()
    {
        _hMove = 0;
        _vMove = -1;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        /*if (CheckMovement(Vector2.up))
        {
            Debug.Log("Move UP");
            _hMove = 0;
            _vMove = 1;
        } else if (CheckMovement(Vector2.down))
        {
            Debug.Log("Move DOWN");
            _hMove = 0;
            _vMove = -1;
        } else if (CheckMovement(Vector2.left))
        {
            Debug.Log("Move LEFT");
            _hMove = -1;
            _vMove = 0;
        } else if (CheckMovement(Vector2.right))
        {
            Debug.Log("Move RIGHT");
            _hMove = 1;
            _vMove = 0;
        }
        else
        {
            Debug.Log("Move ELSE 0 0");
            _hMove = 0;
            _vMove = 0;
        }*/
        
        MoveDirection(_hMove,_vMove);
    }

    void MoveDirection(int hMove, int vMove)
    {
        _rb.velocity = new Vector2(hMove,vMove).normalized * 1f;
    }
    
    private bool CheckMovement(Vector2 pDirection)
    {
        
        Vector2 posicion1 = (Vector2)transform.position + Vector2.Perpendicular(pDirection) * 0.25f;
        Vector2 posicion2 = (Vector2)transform.position - Vector2.Perpendicular(pDirection) * 0.25f;

        
        RaycastHit2D hit1 = Physics2D.Raycast(posicion1, pDirection, 0.5f, wallMask);
        RaycastHit2D hit2 = Physics2D.Raycast(posicion2, pDirection, 0.5f, wallMask);
        
        
        Debug.DrawRay(posicion1, pDirection * 0.5f, Color.red);
        Debug.DrawRay(posicion2, pDirection * 0.5f, Color.red);
        
        return !hit1.collider && !hit2.collider;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            var exit = false;
            while (!exit)
            {
                var vector = GetRandomMovement();
                Debug.Log(" randon "+ vector);
                if (vector == Vector2.up && CheckMovement(Vector2.up))
                {
                    Debug.Log("Move UP");
                    _hMove = 0;
                    _vMove = 1;
                    exit = true;
                } else if (vector == Vector2.down && CheckMovement(Vector2.down))
                {
                    Debug.Log("Move DOWN");
                    _hMove = 0;
                    _vMove = -1;
                    exit = true;
                } else if (vector == Vector2.left && CheckMovement(Vector2.left))
                {
                    Debug.Log("Move LEFT");
                    _hMove = -1;
                    _vMove = 0;
                    exit = true;
                } else if (vector == Vector2.right && CheckMovement(Vector2.right))
                {
                    Debug.Log("Move RIGHT");
                    _hMove = 1;
                    _vMove = 0;
                    exit = true;
                }
                else
                {
                    Debug.Log("Move ELSE 0 0");
                    _hMove = 0;
                    _vMove = 0;
                }
            }

            
        }
    }

    private Vector2 GetRandomMovement()
    {
        Vector2[] movements = { Vector2.left,Vector2.up, Vector2.down, Vector2.right };
        
        return movements[UnityEngine.Random.Range(0, movements.Length)];
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Trigger con + " );
        if (collision.gameObject.CompareTag("Direction"))
        {
            /*if (CheckMovement(Vector2.up))
            {
                Debug.Log("Move UP");
                _hMove = 0;
                _vMove = 1;
            } else if (CheckMovement(Vector2.down))
            {
                Debug.Log("Move DOWN");
                _hMove = 0;
                _vMove = -1;
            } else if (CheckMovement(Vector2.left))
            {
                Debug.Log("Move LEFT");
                _hMove = -1;
                _vMove = 0;
            } else if (CheckMovement(Vector2.right))
            {
                Debug.Log("Move RIGHT");
                _hMove = 1;
                _vMove = 0;
            }
            else
            {
                Debug.Log("Move ELSE 0 0");
                _hMove = 0;
                _vMove = 0;
            }
        }

    }*/
}
