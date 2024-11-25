using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class GhostController : MonoBehaviour
{
    
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private Transform player;
    [SerializeField] GameManager gameManager;
    private Animator _animator;
    private Rigidbody2D _rb;
    //private int _hMove,_vMove;
    private float[] _movements;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        //_hMove = 0;
        //_vMove = -1;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        //MoveDirection(_hMove,_vMove);
        if(gameManager.gameState == GameState.GameOver) Destroy(gameObject);
        
        if (player)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, wallMask);

            if (!hit.collider)
            {
                _rb.velocity = direction * 1.5f;
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }



            //MoveDirection((int)direction.x, (int)direction.y);
            //transform.position +=  (Vector3)direction * 2.0f * Time.deltaTime;
        }
    }

    void MoveDirection(int hMove, int vMove)
    {
        Debug.Log(hMove + "," +vMove);
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
            // Testing ...
            /*var exit = false;
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
            }*/

            
        }
    }

    private Vector2 GetRandomMovement()
    {
        Vector2[] movements = {Vector2.up, Vector2.left, Vector2.down, Vector2.right };
        
        return movements[Random.Range(0, movements.Length)];
    }
}
