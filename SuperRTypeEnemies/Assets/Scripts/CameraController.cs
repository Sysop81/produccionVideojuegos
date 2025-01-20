using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float forwardSpeed = 5.0f;
    private bool _isMove = true;

    /// <summary>
    /// Method Update [Lyfe cycle]
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if(_isMove)
            transform.Translate( forwardSpeed * Time.deltaTime * Vector2.right);
    }
    
    /// <summary>
    /// Trigger OnTriggerEnter2D
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FinishZone"))
        {
            _isMove = false;
        }
    }
    
    /// <summary>
    /// Getter GetForwardSpeed
    /// </summary>
    /// <returns></returns>
    public float GetForwardSpeed()
    {
        return forwardSpeed;
    }
    
    /// <summary>
    /// Getter IsMove
    /// </summary>
    /// <returns></returns>
    public bool IsMove()
    {
        return _isMove;
    }
}
