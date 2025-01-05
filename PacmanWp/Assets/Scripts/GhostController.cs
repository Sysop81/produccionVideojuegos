using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public  enum GhotsTypes {
    Pinky,
    Blinky,
    Clyde,
    Inky
}

public class GhostController : MonoBehaviour
{
    private static readonly int IsScary = Animator.StringToHash("isScary");
    private static Dictionary<string, GhotsTypes> ghostTypesDic = new Dictionary<string, GhotsTypes>()
    {
        { "Pinky", GhotsTypes.Pinky },
        { "Blinky", GhotsTypes.Blinky },
        { "Clyde", GhotsTypes.Clyde },
        { "Inky",GhotsTypes.Inky }
    };
    private static int[] _patrullerArray = { 24,18,19,11,12,13,5,4,3,10,9,2,1,0,6,7,15,26,33,42,43,44,45,46,47,38,29,28,25 };
    
    [SerializeField] private Transform player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer hEye;
    [SerializeField] private SpriteRenderer vEye;
    [SerializeField] private Color gHostColor;
    
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private PlayerController _playerController;
    private bool _isDead;
    private bool _isScary;
    private const float SPEED = 3f;
    private const float LOW_SPEED = 1.5f;
    private float _speed;
    private GhotsTypes _ghotsType;
    private List<Transform> wayPoints;
    private List<Transform> wpPatruller = new List<Transform>();
    private bool _isBuildingWp = true; // WP list is Building -> (if it is false -> start game)
    private int _nextPosition = 0; // [Array wp] Patruller ghost
    
    // Ghost movements
    private Vector2 _currentV2Movemnt;
    private Vector2 _lastV2Movemnt;
    private Transform _currentWPMovemnt;
    private Transform _lastWPMovemnt;

    /// <summary>
    /// Method Start
    /// The method Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _speed = SPEED;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = gHostColor;
        _playerController = player.gameObject.GetComponent<PlayerController>();
        _ghotsType = ghostTypesDic[name]; 
        
        // Call method to get all way point and build the patruller map
        InvokeRepeating("LoadWayPoints",0.1f,0.1f);
    }
    
    /// <summary>
    /// Method Update
    /// This method is called once per frame
    /// </summary>
    void Update()
    {
        // Manages start and game over
        if (_isBuildingWp) return;
        if(gameManager.gameState == GameState.GameOver) Destroy(gameObject);
        
        // Manages if the player has the powerUp
        if (_playerController.GetHasPowerUp() && !_isScary)
        {
            _animator.SetTrigger(IsScary);
            _isScary = false;
            _speed = LOW_SPEED;
            StartCoroutine(ScaryAnimationProps());
        }
        
        // Manages the ghos move (Vector2 or transform)
        if (_ghotsType == GhotsTypes.Pinky || _ghotsType == GhotsTypes.Clyde)
        {
            MoveTowards(_currentWPMovemnt, _speed); // Transform WP movement
            return;
        }
        
        MoveTowards(_currentV2Movemnt, _speed); // Vector 2 movement
    }
    
    

    /// <summary>
    /// Method BuildGhostProperties
    /// This method load the instance properties of each ghost
    /// </summary>
    private void BuildGhostProperties()
    {
        switch (_ghotsType)
        {
            case GhotsTypes.Pinky: // Transform
                _currentWPMovemnt = wpPatruller[_nextPosition];
                break;
            case GhotsTypes.Clyde: // Transform
                _currentWPMovemnt = wayPoints[23];
                _lastWPMovemnt = _currentWPMovemnt;
                break;
            case GhotsTypes.Blinky: // V2
                _currentV2Movemnt = Vector2.left;
                _lastV2Movemnt = _currentV2Movemnt;
                break;
            default: // Inky V2
                _currentV2Movemnt = Vector2.up;
                hEye.enabled = false;
                vEye.enabled = true;
                break;
        }
    }
    
    /// <summary>
    /// Method LoadWayPoints
    /// This method call the gameManager to extract a way point list builded
    /// </summary>
    private void LoadWayPoints()
    {
        // Calling to gameManger to get the builded wp list
        wayPoints = gameManager.GetWayPointList();
        // If list it is ready
        if (wayPoints != null && wayPoints.Count != 0)
        {
            // Cancel InvokeRepeating and set flag "_isBuildingWp" to false and build a Patratuller map
            CancelInvoke("LoadWayPoints");
            _isBuildingWp = false;
            BuildPatrullerMap();
        }
    }
    
    
    /// <summary>
    /// Method BuildPatrullerMap
    /// Thios method build the wpPatruller list
    /// </summary>
    private void BuildPatrullerMap()
    {
        if(wpPatruller.Count == _patrullerArray.Length) return;
        // Adding the equals index wp to wpPatruller wp list
        foreach (var index in _patrullerArray)
        {
            wpPatruller.Add(wayPoints[index]);   
        }
        
        // Set initial properties for all Ghost.
        BuildGhostProperties();
    }
    
    /// <summary>
    /// Method MoveTowards
    /// This method moves the ghost using motion vectors
    /// </summary>
    /// <param name="targetDirection"></param>
    /// <param name="speed"></param>
    private void MoveTowards(Vector2 targetDirection, float speed)
    {
        _rb.velocity = speed  * targetDirection;
    }
    
    /// <summary>
    /// Method MoveTowards
    /// This method moves the ghost pointing to the transform of the target wp
    /// </summary>
    /// <param name="targetWp"></param>
    /// <param name="speed"></param>
    private void MoveTowards(Transform targetWp, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWp.position, speed * Time.deltaTime);
    }

    /// <summary>
    /// Setter SetIsDead
    /// </summary>
    /// <param name="isDead"></param>
    public void SetIsDead(bool isDead)
    {
        _isDead = isDead;
    }
    
    /// <summary>
    /// Setter SetIsScary
    /// </summary>
    /// <param name="isScary"></param>
    public void SetIsScary(bool isScary)
    {
        _isScary = isScary;
    }
    
    /// <summary>
    /// Triggeer OnTriggerEnter2D 
    /// </summary>
    /// <param name="other">Trigger gameObject</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Manages the collision with Way point
        if (collision.gameObject.CompareTag("wayPoint"))
        {
            // Get Script Controller of way point object collision
            var wp = collision.GetComponent<WpController>();
            
            // Manages a Clyde new movement [Randon Patruller]
            if (_ghotsType == GhotsTypes.Clyde)
            {
                //ManageClydeByVector(wp); // Another way to do it. Using movement vectors
                
                // Get the clean direction. All available wp collision direction minus the direction it comes from
                List<GameObject> cleanDirections = new();
                foreach (var wpo in wp.availableWPoints)
                {
                    if(wpo.transform.position != _lastWPMovemnt.position) cleanDirections.Add(wpo);
                }
                
                // Set the current Wp to last and load a randon wp of clena directions
                _lastWPMovemnt = _currentWPMovemnt;
                _currentWPMovemnt =  cleanDirections[Random.Range(0, cleanDirections.Count)].transform;
               
                // Get the current position and manage ghost eyes calling the method
                var currentPosition = _currentWPMovemnt.position - transform.position;
                var currentDir = new Vector2(currentPosition.x,currentPosition.y).normalized;
                ManageEyes(GetDirection(currentDir));
            }
            
            // Manages a Blinky new movement
            if (_ghotsType == GhotsTypes.Blinky)
            {
                // Instantiate a local variables to determine the blinky movement
                Vector2 direction = Vector2.zero;
                float minDistance = float.MaxValue;
                
                // Obtains the clean direction
                List<Vector2> cleanDirections = new();
                Vector2 inverseLastDirection = GetInverseDirection();
                foreach (Vector2 availableDirection in wp.availableDirections)
                {
                    if (availableDirection != inverseLastDirection) cleanDirections.Add(availableDirection);
                }
                
                // Get the possible direction into clean direction availables
                foreach (Vector2 availableDirection in cleanDirections)
                {
                    // If the distance in this direction is less than the current
                    // min distance then this direction becomes the new closest
                    Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                    float distance = (player.position - newPosition).sqrMagnitude;

                    // Get the best direction based on distance from the player
                    if (distance < minDistance)
                    {
                        direction = availableDirection;
                        minDistance = distance;
                    }
                }
                
                // Manages ghost eyes, set new direction and change the last direction.
                ManageEyes(direction);
                _currentV2Movemnt = direction;
                _lastV2Movemnt = direction;
            }
            
            // Manages Pinky [Patruller] 
            if (_ghotsType == GhotsTypes.Pinky)
            {
                _nextPosition = (_nextPosition + 1) % wpPatruller.Count;
                _currentWPMovemnt = wpPatruller[_nextPosition];

                // Get current position and finally calling the manage eyes method
                var currentPosition = wpPatruller[_nextPosition].transform.position - transform.position;
                var currentDir = new Vector2(currentPosition.x,currentPosition.y).normalized;
                ManageEyes(GetDirection(currentDir));
            }
        }
        
        // Manages collision with the MAgic door
        if (collision.CompareTag("MagicDoor"))
        {
            // Manage a transform position when player arrives to left o right magic door
            transform.position = new Vector3(collision.name.Contains("L") ? 23f : -7f,
                collision.transform.position.y, collision.transform.position.z);
        }
        
        // Inky default simple movement. Only change direction when collision with a wall
        if (_ghotsType == GhotsTypes.Inky && collision.gameObject.CompareTag("Wall"))
        {
            
            if (_currentV2Movemnt == Vector2.down)
            {
                vEye.flipY = false;
                _currentV2Movemnt = Vector2.up;
                return;
            }
            
            vEye.flipY = true;
            _currentV2Movemnt = Vector2.down;
        }
    }
    
    /// <summary>
    /// Method GetInverseDirection
    /// This method determines the direction the red ghost is coming from. Returns the vector opposite to the last move
    /// </summary>
    /// <returns>Vector2 movement</returns>
    private Vector2 GetInverseDirection()
    {
        if (_lastV2Movemnt == Vector2.up)  return Vector2.down;
        if(_lastV2Movemnt == Vector2.down) return Vector2.up;
        if(_lastV2Movemnt == Vector2.left) return Vector2.right;
        
        return Vector2.left;
    }
    
    /// <summary>
    /// Method GetDirection
    /// This method determines the direction of the ghost to manage the eyes move.
    /// </summary>
    /// <param name="nDirectiion">Vector2 direction</param>
    /// <returns></returns>
    private Vector2 GetDirection(Vector2 nDirectiion)
    {
        // Calculate the absolute value to determine the dominant direction
        if (Mathf.Abs(nDirectiion.x) > Mathf.Abs(nDirectiion.y))
        {
            // If X is dominant -> Return left or right movement
            return nDirectiion.x > 0 ? Vector2.right : Vector2.left;
        }
       
        // If X is dominant -> Return up or down movement 
        return nDirectiion.y > 0 ? Vector2.up : Vector2.down;
    }
    
    /// <summary>
    /// Method ManageEyes
    /// This method activates or deactivates the eyes and flips them if necessary
    /// </summary>
    /// <param name="direction"></param>
    private void ManageEyes(Vector2 direction)
    {
        // Manages Y movement
        if (direction == Vector2.down || direction == Vector2.up)
        {
            hEye.enabled = false;
            vEye.enabled = true;
            vEye.flipY = direction != Vector2.up;
            return;
        }
        // Or X movement
        vEye.enabled = false;
        hEye.enabled = true;
        hEye.flipX = direction != Vector2.right;
    }


    /// <summary>
    /// IEnumerator ScaryAnimationProps
    /// This method manages the scary animation
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaryAnimationProps()
    {
        _sr.color = Color.white;
        yield return new WaitForSeconds(gameManager.GetPowerUpTime());
        _sr.color = gHostColor;
        _speed = SPEED;
    }
    
    
    // ****** Other 
    
    /*/// <summary>
    /// Method ManageClydeByVector
    /// This method manages Clyde ghost by Vector movement. If neccesary activate the clydeV2 variable
    /// </summary>
    /// <param name="wp">WayPoint Controller of the collision object</param>
    private void ManageClydeByVector(WpController wp)
    {
        
        // Get a randon direction from wp script controller
        int iDirection = Random.Range(0,wp.availableDirections.Count);

        // the next available direction
        if (wp.availableDirections.Count > 1 && wp.availableDirections[iDirection] == - new Vector2(transform.position.x, transform.position.y))
        {
            iDirection++;

            // Wrap the index back around if overflowed
            if (iDirection >= wp.availableDirections.Count) iDirection = 0;
            
        }

        clydeV2 = wp.availableDirections[iDirection]; 
    }*/
}
