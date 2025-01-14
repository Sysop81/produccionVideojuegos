using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWhiteShipController : EnemyController
{
    
    private bool _isMirrorMovement;

    void Awake()
    {
        Intialize();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ForwardSpeed = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate( ForwardSpeed * Time.deltaTime * Direction ,Space.World);
    }
    
    public void SetInverseMove(bool isInverseMovement = false)
    {
        SpriteRenderer.flipY = isInverseMovement;
        _isMirrorMovement = isInverseMovement;
        Direction = isInverseMovement ? Vector3.down : Vector3.up;
    }

    new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        
        if (other.CompareTag("wp"))
        {
            ForwardSpeed += 1f;
            Direction = Vector3.left;
            transform.rotation = Quaternion.Euler(0, 0, _isMirrorMovement ? -90:90);
        }
    }
}
