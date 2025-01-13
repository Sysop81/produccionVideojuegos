using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController3 : MonoBehaviour
{
    
    [SerializeField] private float forwardSpeed = 4.0f;

    private GameObject _explosion;
    private SpriteRenderer  _sp;
    private Animator _animator;
    private Vector3 _direction;
    private Vector3 _hDirection = Vector3.zero;
    private bool _isMirrorMovement = false;
    
    
    // TODO -> Hide the ship ON initial and render later
    
    
    // Start is called before the first frame update
    void Awake()
    {
        _direction = Vector3.up;
        _animator = GetComponent<Animator>();
        _sp = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _explosion = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "Explosion");
        /*_direction = Vector3.up;
        _animator = GetComponent<Animator>();
        _sp = GetComponent<SpriteRenderer>();*/
        //InvokeRepeating("MoveVertical",0f,0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate( forwardSpeed * Time.deltaTime * _direction,Space.World);
        //transform.Translate( 2f * Time.deltaTime * _hDirection, Space.World);
    }
    
    public void SetVerticalMove(Vector3 vMove, bool isDown = false)
    {
        _hDirection = vMove;
        _sp.flipY = isDown;
        _isMirrorMovement = isDown;
        if(isDown) _direction = Vector3.down;
    }
    
    void MoveVertical()
    {
        _hDirection = _hDirection == Vector3.up ? Vector3.down : Vector3.up;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.CompareTag("DeathZone")) Destroy(gameObject);
        
        if (other.CompareTag("wp"))
        {
           // _animator.SetBool("isLeftForward",true);
           forwardSpeed += 1f;
           _direction = Vector3.left;
           transform.rotation = Quaternion.Euler(0, 0, _isMirrorMovement ? -90:90);
           
        }
        
        if (other.CompareTag("Shoot"))
        {
            for (int i = 0; i < 4; i++)
            {
                Instantiate(_explosion, Tools.GetAleatoryTranformPosition(transform.position,Random.Range(0.5f,1f)), Quaternion.identity);
            }
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
