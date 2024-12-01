using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private static readonly int IsScary = Animator.StringToHash("isScary");
    
    [SerializeField] private Transform player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer hEye;
    [SerializeField] private SpriteRenderer vEye;
    [SerializeField] private Color gHostColor;
    
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private int _hMove,_vMove;
    private float[] _movements;
    private PlayerController _playerController;
    private bool _isDead;
    private bool _isScary;
    private const float SPEED = 2f;
    private const float LOW_SPEED = 1f;
    
    /// <summary>
    /// Method Start
    /// The method Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _hMove = 0;
        _vMove = -1;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = gHostColor;
        _playerController = player.gameObject.GetComponent<PlayerController>();
    }

    
    /// <summary>
    /// Method Update
    /// This method is called once per frame
    /// </summary>
    void Update()
    {
        if(gameManager.gameState == GameState.GameOver) Destroy(gameObject);
        
        MoveDirection(_hMove,_vMove);

        if (_playerController.GetHasPowerUp() && !_isScary)
        {
            _animator.SetTrigger(IsScary);
            _isScary = false;
            StartCoroutine(ScaryAnimationProps());
        }
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
    /// Method MoveDirection
    /// This method move the gosh
    /// </summary>
    /// <param name="hMove">Horizontal move</param>
    /// <param name="vMove">Vertical move</param>
    private void MoveDirection(int hMove, int vMove)
    {
        _rb.velocity = new Vector2(hMove,vMove).normalized * (!_playerController.GetHasPowerUp() ? SPEED : LOW_SPEED);
    }
    
    
    /// <summary>
    /// Triggeer OnTriggerEnter2D 
    /// </summary>
    /// <param name="other">Trigger gameObject</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            
            if (_vMove < 0)
            {
                hEye.enabled = false;
                vEye.enabled = true;
                vEye.flipY = false;
                _vMove = 1;
            }
            else
            {
                hEye.enabled = false;
                vEye.enabled = true;
                vEye.flipY = true;
                _vMove = -1;
            }
        }
    }
    
    
    /// <summary>
    /// IEnumerator ScaryAnimationProps
    /// This method manages the scary animation
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaryAnimationProps()
    {
        vEye.enabled = false;
        _sr.color = Color.white;
        yield return new WaitForSeconds(gameManager.GetPowerUpTime());
        vEye.enabled = true;
        _sr.color = gHostColor;
    }
}
