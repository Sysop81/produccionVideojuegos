using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public enum GameState
{
    Loading,
    InGame
}

public class GameManager : MonoBehaviour
{
    
    public GameState gameState = GameState.Loading;
    [SerializeField] private TextMeshProUGUI scoreTextP1;
    [SerializeField] private TextMeshProUGUI scoreTextP2;
    [SerializeField] private TextMeshProUGUI waitingTextP1;
    [SerializeField] private TextMeshProUGUI waitingTextP2;

    private int _p1Score;
    private int _p2Score;
    
    public bool _isP1Active;
    public bool _isP2Active;

    private BallController _ball;
    
    // Start is called before the first frame update
    void Start()
    {
        _ball = FindObjectOfType<BallController>();
        _p1Score = 0;
        _p2Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isP1Active && _isP2Active) gameState = GameState.InGame;
    }
    
    /// <summary>
    /// Method UpdateScore
    /// This method updates the score text
    /// </summary>
    /// <param name="value"></param>
    public void UpdateScore(int value, bool isP1)
    {
        if (isP1)
        {
            _p1Score += value;
            scoreTextP1.text = _p1Score.ToString();
            return;
        }
        
        _p2Score += value;
        scoreTextP2.text = _p2Score.ToString();
    }

    public void SetPlayerActive(bool value, bool isP1)
    {
        if (isP1)
        {
            _isP1Active = value;
            waitingTextP1.gameObject.SetActive(false);
            return;
        }
        
        _isP2Active = value;
        waitingTextP2.gameObject.SetActive(false);
        
        if(_isP1Active && _isP2Active) _ball.InitializeBall();
    }
}
