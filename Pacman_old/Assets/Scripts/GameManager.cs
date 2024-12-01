using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;


public enum GameState
{
    Loading,
    InGame,
    InPaused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    private const string MAX_SCORE = "MAX_SCORE";
    public GameState gameState = GameState.Loading;
    [SerializeField] private TextMeshProUGUI maxScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private List<GameObject> lives;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject readyPanel;
    private int _maxScore;
    private int _score;
    private int _numLives;
    private GhostController[] _ghosts;
    
    // Start is called before the first frame update
    void Start()
    {
        _ghosts = FindObjectsOfType<GhostController>();
        _numLives = lives.Count;
        ShowMaxScore();
    }
    
    
    /// <summary>
    /// Method ShowMaxScore
    /// This method shows the max score to user
    /// </summary>
    private void ShowMaxScore()
    {
        _maxScore = PlayerPrefs.GetInt(MAX_SCORE,0);
        Debug.Log("Max score: " + _maxScore);
        maxScoreText.text = _maxScore.ToString();
    }
    
    
    /// <summary>
    /// Method SetMaxScore
    /// This method updates the max score registered and launch a conffi celebration particle system
    /// </summary>
    private void SetMaxScore()
    {
        if (_score > _maxScore) PlayerPrefs.SetInt(MAX_SCORE,_score);
    }
    
    
    /// <summary>
    /// Method UpdateScore
    /// This method updates the score text
    /// </summary>
    /// <param name="value"></param>
    public void UpdateScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();
    }
    
    /// <summary>
    /// Method UpdatePanelLives
    /// This method updates the live panel
    /// </summary>
    private void UpdatePanelLives()
    {
        /*Image heratImage = lives[_numLives].GetComponent<Image>();
        var tempColor = heratImage.color;
        tempColor.a = 0.3f;
        heratImage.color = tempColor;*/
        lives[_numLives].SetActive(false);
    }
    
    /// <summary>
    /// Method GameOver
    /// This method manages the counter lives and game state
    /// </summary>
    public void GameOver()
    {
        _numLives--;

        if (_numLives <= 0)
        {
            SetMaxScore();
            gameOverPanel.SetActive(true);
            gameState = GameState.GameOver;
            //gameOverText.gameObject.SetActive(true);
            //restartButton.gameObject.SetActive(true);
            //return;
        }
        else
        {
            StartCoroutine(ManageReadyPanel());
        }
        
        UpdatePanelLives();
    }

    public void StartGame()
    {
        
        gameOverPanel.SetActive(false);
        UpdateScore(0);
        StartCoroutine(ManageReadyPanel());
    }

    public void UpdateGhostVisibility(bool visibility)
    {
        _ghosts.ToList().ForEach(ghost => ghost.gameObject.SetActive(visibility));
    }

    private IEnumerator ManageReadyPanel()
    {
        readyPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        readyPanel.SetActive(false);
        gameState = GameState.InGame;
        
    }

}
