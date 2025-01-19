using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRedShipController : EnemyController
{
    
    private readonly float _verticalSpeed = 2.0f;
    
    /// <summary>
    /// Method Awake [Life cycle]
    /// </summary>
    void Awake()
    {
        // Calling the parent Initialize method
        Intialize();
    }
    
    /// <summary>
    /// Method Start [Life cycle]
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        ForwardSpeed = 5.0f;
        Direction = Vector3.zero;
        InvokeRepeating(nameof(MoveVertical),0f,0.3f);
    }
    
    /// <summary>
    /// Method Update
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        transform.Translate( ForwardSpeed * Time.deltaTime * Vector2.left);
        transform.Translate( _verticalSpeed * Time.deltaTime * Direction);
    }
    
    /// <summary>
    /// Method MoveVertical
    /// Repeating method to change Y direction
    /// </summary>
    void MoveVertical()
    {
        Direction = Direction == Vector3.up ? Vector3.down : Vector3.up;
    }
    
    /// <summary>
    /// Setter SetVerticalMove
    /// </summary>
    /// <param name="vMove"></param>
    public void SetVerticalMove(Vector3 vMove)
    {
        Direction = vMove;
    }
}
