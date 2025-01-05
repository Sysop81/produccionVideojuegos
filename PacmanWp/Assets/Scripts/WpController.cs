using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WpController : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private LayerMask wpMask;
    public List<Vector2> availableDirections;
    public List<GameObject> availableWPoints;
    
    private readonly Vector2[] _directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private readonly float _rDistance = 1f;
    private readonly float _offset = 0.5f;
    private BoxCollider2D _col;
    private string _layerName = "Wpoints";
    
    /// <summary>
    /// Method Start
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _col = GetComponent<BoxCollider2D>();
        availableDirections = new List<Vector2>();
        
        // For each direction call the CheckAvailableDirection method to determine if the direction is an avaliable [no wall] direction  
        foreach (var direction in _directions) CheckAvailableDirection(direction);
        
        // Finally get the neighbor wp
        Invoke("CheckAvailableWp",0.5f);
    }

    
    /// <summary>
    /// Method CheckAvailableWp
    /// This method uses the raycast in the 4 addresses of each wp to determine which are its neighbor wp
    /// </summary>
    private void CheckAvailableWp()
    {
        
        // Getting center and colliders limits 
        Vector2 colliderCenter = (Vector2)transform.position + _col.offset;
        Vector2 colliderSize = _col.size * transform.lossyScale; 

        // Ray distance 
        float rayDistance = 15;
        

        // throw ray in all 4 directions
        foreach (Vector2 direction in availableDirections)
        {
            
            Vector2 start = colliderCenter;

            // Adjust the initial position of the ray according to the direction
            if (direction == Vector2.left)
                start.x -= colliderSize.x / 2;
            else if (direction == Vector2.right)
                start.x += colliderSize.x / 2;
            else if (direction == Vector2.up)
                start.y += colliderSize.y / 2;
            else if (direction == Vector2.down)
                start.y -= colliderSize.y / 2;

            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            
            // Apply BoxCast to detect collisions
            RaycastHit2D hit =
                Physics2D.BoxCast(start, Vector2.one * _offset, 0f, direction, rayDistance, wpMask);

            // Adjust the layer again so that it detects the wp and manage the hit to add wp to the avaliable wp directions
            gameObject.layer = LayerMask.NameToLayer($"{_layerName}"); 
            if(hit.collider != null && hit.collider.gameObject != gameObject) availableWPoints.Add(hit.collider.gameObject);
            
            // Finally draw the ray to Debug
            Debug.DrawRay(start, direction * rayDistance, hit.collider ? Color.green : Color.red);
        }
    }
    
    /// <summary>
    /// Method CheckAvailableDirection
    /// This method uses raycast to determine the avaliable Vector2 directions
    /// </summary>
    /// <param name="direction"></param>
    private void CheckAvailableDirection(Vector2 direction)
    {
        // Apply BoxCast to detect collisions
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * _offset, 0f, direction, _rDistance, wallLayerMask);

        // If no collider is hit then there is no obstacle in that direction
        if (!hit.collider) availableDirections.Add(direction);
    }
}
