using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEditor.Tools;


//using Random = Unity.Mathematics.Random;

public class EnemyBaseController : MonoBehaviour
{
    [SerializeField] private GameObject baseSpawner;
    [SerializeField] private GameObject explosion;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    private static readonly int IsDestroy = Animator.StringToHash("isDestroyed");
    
    private GameObject _player;
    private bool _isActive;
    private Animator _animator;
    private int _life = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
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
        
        
        for (int i = 0; i < 6; i++)
        {
            Instantiate(explosion, Tools.GetAleatoryTranformPosition(transform.position), Quaternion.identity);
        }
    }

    IEnumerator LaunchEnemies()
    {
        baseSpawner.SetActive(true);
        yield return new WaitForSeconds(3.2f);
        ManageDoor(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BaseZone"))
        {
            ManageDoor(true);
        }
    }
}
