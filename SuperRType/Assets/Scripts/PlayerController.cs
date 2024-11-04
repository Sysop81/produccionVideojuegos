using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speedForce;
    
    private float _hMove;
    private float _vMove;
    private Animator _animator;
    private Rigidbody2D _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        speedForce = 1200.0f;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*_hMove = Input.GetAxis("Horizontal");
        _vMove = Input.GetAxis("Vertical");
        Debug.Log("hMove -> " + _hMove + " vMove -> " + _vMove);*/

        MoveDirection();
    }

    private void MoveDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _animator.SetBool("isUp", true);
            _rb.AddForce(new Vector2(0.0f,speedForce * Time.deltaTime));
            
        }else if (Input.GetKey(KeyCode.DownArrow))
        {
            _animator.SetBool("isDown", true);
            _rb.AddForce(new Vector2(0.0f,-speedForce * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _animator.SetBool("isUp", false);
            _animator.SetBool("isDown", false);
            _rb.AddForce(new Vector2(speedForce * Time.deltaTime, 0.0f));
        }
        else
        {
            _animator.SetBool("isUp", false);
            _animator.SetBool("isDown", false);
        }
    }
}
