using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsLeft = Animator.StringToHash("isLeft");
    private static readonly int IsRight = Animator.StringToHash("isRight");
    
    [SerializeField] private float speedForce;
    private Animator _animator;
    private Rigidbody2D _rb;
    private float _hMove;
    private float _vMove;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveDirection();
    }
    
    private void MoveDirection()
    {
        // Get move axes
        _hMove = Input.GetAxis("Horizontal");
        _vMove = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            _rb.velocity = new Vector2(_hMove, 0).normalized * speedForce;
            _animator.SetBool(_hMove > 0 ? IsRight : IsLeft, true);
        }
    }
}
