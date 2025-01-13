using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController3 : MonoBehaviour
{
    
    [SerializeField] private float forwardSpeed = 4.0f;

    private GameObject _explosion;
    private SpriteRenderer  _sp;
    private Animator _animator;
    private Vector3 _direction;
    private Vector3 _hDirection = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        _direction = Vector3.up;
        _animator = GetComponent<Animator>();
        _sp = GetComponent<SpriteRenderer>();
        InvokeRepeating("MoveVertical",0f,0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate( forwardSpeed * Time.deltaTime * _direction);
        transform.Translate( 2f * Time.deltaTime * _hDirection);
    }
    
    public void SetVerticalMove(Vector3 vMove)
    {
        _hDirection = vMove;
    }
    
    void MoveVertical()
    {
        
        //transform.Translate( _verticalSpeed * _hDirection);

        _hDirection = _hDirection == Vector3.up ? Vector3.down : Vector3.up;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("wp"))
        {
           // _animator.SetBool("isLeftForward",true);
           forwardSpeed += 1f;
           _direction = Vector3.left;
        }
    }
}
