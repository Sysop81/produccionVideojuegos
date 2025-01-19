using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBombController : EnemyController
{
    
    private static readonly int IsUp = Animator.StringToHash("isEnd");
    private float _timeToEnd;
    private Animator _animator;
    
    /// <summary>
    /// Method Awake [Life cycle]
    /// </summary>
    private void Awake()
    {
        Intialize();
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Method Start [Life cycle]
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        ForwardSpeed = 1.0f;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        _timeToEnd = Random.Range(1f, 3f);
        Invoke(nameof(ChangeState),_timeToEnd);
    }

    /// <summary>
    /// Method Update
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        transform.Translate( ForwardSpeed * Time.deltaTime * Direction);
    }
    
    /// <summary>
    /// Getter Direction
    /// </summary>
    new public Vector3 Direction
    {
        get => base.Direction;
        set => base.Direction = value;
    }
    
    /// <summary>
    /// Method ChangeState
    /// This method manage the animator to set the final state and invoke the explosion method
    /// </summary>
    private void ChangeState()
    {
        _animator.SetBool(IsUp,true);
        Invoke(nameof(SetExplosion),2f);
    }
    
    /// <summary>
    /// Method SetExplosion
    /// This method launch a corrutine to manage the explosion
    /// </summary>
    private void SetExplosion()
    {
        StartCoroutine(ManageBombExplosion());
    }
    
    /// <summary>
    /// IEnumerator ManageBombExplosion [Corrutine]
    /// </summary>
    /// <returns></returns>
    private IEnumerator ManageBombExplosion()
    {
        LaunchExplosion();
        SpriteRenderer.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
