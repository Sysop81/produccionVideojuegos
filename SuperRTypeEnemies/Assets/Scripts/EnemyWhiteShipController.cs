using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWhiteShipController : EnemyController
{
    
    private bool _isMirrorMovement;
    
    /// <summary>
    /// Method Awake [Life cycle]
    /// </summary>
    void Awake()
    {
        // Calling the parent Initialize method
        Intialize();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Method Start [Life cycle]
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        ForwardSpeed = 4.0f;
    }

    /// <summary>
    /// Method Update
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        transform.Translate( ForwardSpeed * Time.deltaTime * Direction ,Space.World);
    }
    
    /// <summary>
    /// Method SetInverseMovement
    /// This method manages the sprite and movement vector to set the correct direction and sprite render
    /// </summary>
    /// <param name="isInverseMovement"></param>
    public void SetInverseMove(bool isInverseMovement = false)
    {
        SpriteRenderer.flipY = isInverseMovement;
        _isMirrorMovement = isInverseMovement;
        Direction = isInverseMovement ? Vector3.down : Vector3.up;
    }
    
    /// <summary>
    /// Trigger OnTriggerEnter2D
    /// </summary>
    /// <param name="other"></param>
    new void OnTriggerEnter2D(Collider2D other)
    {
        // Calling a parent Ontrigger
        base.OnTriggerEnter2D(other);
        
        // Manage the collision with the way point to change the sprite rotation and gameobject vector direction
        if (other.CompareTag("wp"))
        {
            ForwardSpeed += 1f;
            Direction = Vector3.left;
            transform.rotation = Quaternion.Euler(0, 0, _isMirrorMovement ? -90:90);
        }
    }
}
