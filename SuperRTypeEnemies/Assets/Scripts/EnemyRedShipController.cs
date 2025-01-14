using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRedShipController : EnemyController
{
    
    private float _verticalSpeed = 2.0f;
    //private Vector3 _hDirection = Vector3.zero;
    //private GameObject _explosion;

    void Awake()
    {
        Intialize();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ForwardSpeed = 5.0f;
        Direction = Vector3.zero;
        InvokeRepeating("MoveVertical",0f,0.3f);
    }
    
    void MoveVertical()
    {
        Direction = Direction == Vector3.up ? Vector3.down : Vector3.up;
    }

    public void SetVerticalMove(Vector3 vMove)
    {
        Direction = vMove;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate( ForwardSpeed * Time.deltaTime * Vector2.left);
        transform.Translate( _verticalSpeed * Time.deltaTime * Direction);
    }
}
