using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController_old : MonoBehaviour
{
    
    [SerializeField] private float forwardSpeed = 5.0f;
    private float _verticalSpeed = 2.0f;
    private Vector3 _hDirection = Vector3.zero;
    private GameObject _explosion;
    
    // Start is called before the first frame update
    void Start()
    {
        _explosion = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "Explosion");
        InvokeRepeating("MoveVertical",0f,0.3f);
    }

    void MoveVertical()
    {
        
        //transform.Translate( _verticalSpeed * _hDirection);

        _hDirection = _hDirection == Vector3.up ? Vector3.down : Vector3.up;
    }

    public void SetVerticalMove(Vector3 vMove)
    {
        _hDirection = vMove;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate( forwardSpeed * Time.deltaTime * Vector2.left);
        transform.Translate( _verticalSpeed * Time.deltaTime * _hDirection);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("DeathZone")) Destroy(gameObject);

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
