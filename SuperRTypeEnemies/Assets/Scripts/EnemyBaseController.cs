using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEditor.Tools;


//using Random = Unity.Mathematics.Random;

public class EnemyBaseController : EnemyController
{
    [SerializeField] private GameObject baseSpawner;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    private static readonly int IsDestroy = Animator.StringToHash("isDestroyed");
    
    private bool _isActive;
    private Animator _animator;
    private int _life = 5;
    
    /// <summary>
    /// Method Awake [Life cycle]
    /// </summary>
    void Awake()
    {
        Intialize();
    }

    /// <summary>
    /// Method Start [Life cycle]
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Method Update
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (_isActive) ManageDoor(true);
    }
    
    /// <summary>
    /// Method ManageDoor
    /// TODO complete this......
    /// </summary>
    /// <param name="value"></param>
    void ManageDoor(bool value)
    {
        _animator.SetBool(IsOpen, value);

        if (value) StartCoroutine(LaunchEnemies());

    }

    public void UpdateLife(int value)
    {
        if (_life > 0) _life -= value;
    }

    IEnumerator LaunchEnemies()
    {
        baseSpawner.SetActive(true);
        yield return new WaitForSeconds(3.2f);
        ManageDoor(false);
    }

    new private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BaseZone"))
        {
            ManageDoor(true);
        }
        
        if (other.gameObject.CompareTag("Shoot"))
        {
            Destroy(other.gameObject);
            UpdateLife(other.gameObject.GetComponent<ShootController>().GetDamage());
            if (_life <= 0)
            {
                // This base is destroyed by player
                _animator.SetBool(IsDestroy,true);
                
                LaunchExplosion(6);
            }
        }
    }
}
