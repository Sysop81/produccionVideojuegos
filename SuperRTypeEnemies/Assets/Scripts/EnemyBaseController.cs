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

    void Awake()
    {
        Intialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            ManageDoor(true);
        }
    }

    void ManageDoor(bool value)
    {
        _animator.SetBool(IsOpen, value);

        if (value) StartCoroutine(LaunchEnemies());

    }

    public void UpdateLife(int value)
    {
        if (_life > 0)
        {
            _life -= value;
            return;
        }
        
        // This base is destroyed by player
        _animator.SetBool(IsDestroy,true);
        
        LaunchExplosion(6);
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
    }
}
