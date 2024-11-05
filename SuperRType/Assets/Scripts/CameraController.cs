using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float forwardSpeed = 5.0f;
    private bool _isMove = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isMove)
            transform.Translate( forwardSpeed * Time.deltaTime * Vector2.right);
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("FinishZone"))
        {
            _isMove = false;
        }
    }
}
