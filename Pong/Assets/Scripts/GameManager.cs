using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] private GameObject gameTypePanel;
    
    private int _p1Score;
    private int _p2Score;
    private bool _isP1Active;
    private bool _isP2Active;
    private BallController _ball;
    
    /// <summary>
    /// Method Start
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _ball = FindObjectOfType<BallController>();
        _p1Score = 0;
        _p2Score = 0;
    }

    /// <summary>
    /// Method Update
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Set the game state in geme only when all player are ready
        if (_isP1Active && _isP2Active) gameState = GameState.InGame;
    }
    
    /// <summary>
    /// Setter SetGameMode
    /// Canvas button handler to set the ball game mode. Arcade or simulated
    /// </summary>
    public void SetGameMode()
    {
        _ball.SetImpulseType(!EventSystem.current.currentSelectedGameObject.name.ToLower().Contains("arcade"));
        gameTypePanel.SetActive(false);
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
    
    /// <summary>
    /// Method SetPlayerActive
    /// This method handle the players activation and initializes the ball
    /// </summary>
    /// <param name="value">Boolean value to set activation flag</param>
    /// <param name="isP1">Boolean value to indicates the player type</param>
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
