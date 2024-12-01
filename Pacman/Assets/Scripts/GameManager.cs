using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


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
    [SerializeField] private Tilemap[] tilemap;
    [SerializeField] private GameObject sPellet;
    [SerializeField] private GameObject bPellet;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private TextMeshProUGUI maxScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private List<GameObject> lives;
    
    private int _maxScore;
    private int _score;
    private int _numLives;
    private List<Image> _iLives;
    private GhostController[] _ghosts;
    private const int POWER_UP_TIME = 10;
    
    /// <summary>
    /// Method Start
    /// The method Start is called before the first frame update
    /// </summary>
    void Start()
    {
        BuildsPellets();
        _ghosts = FindObjectsOfType<GhostController>();
        _numLives = lives.Count;
        BuildListOfImageLives();
        ShowMaxScore();
    }
    
    
    /// <summary>
    /// Method ShowMaxScore
    /// This method shows the max score to user
    /// </summary>
    private void ShowMaxScore()
    {
        _maxScore = PlayerPrefs.GetInt(MAX_SCORE,0);
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
        _iLives[_numLives].color = Color.black;
    }
    
    /// <summary>
    /// Method BuildListOfImageLives
    /// This method build a gameObject image list of lives to prevent a expensive GetComponent call
    /// </summary>
    private void BuildListOfImageLives()
    {
        _iLives = new List<Image>();
        foreach (var live in lives)
            _iLives.Add(live.GetComponent<Image>());
    }

    
    /// <summary>
    /// Method StartGame
    /// This method launch the game
    /// </summary>
    public void StartGame()
    {
        gameOverPanel.SetActive(false);
        UpdateScore(0);
        StartCoroutine(ManageReadyPanel());
    }
    
    /// <summary>
    /// Method GameOver
    /// This method manages the counter lives and game state
    /// </summary>
    public void GameOver()
    {
        _numLives--;
        UpdatePanelLives();
        
        if (_numLives <= 0)
        {
            SetMaxScore();
            gameOverPanel.SetActive(true);
            gameState = GameState.GameOver;
            return;
        }
        
        StartCoroutine(ManageReadyPanel());
    }
    
    /// <summary>
    /// Method UpdateGhostVisibility
    /// This method updates the all ghost visibility, for example, when player is dead
    /// </summary>
    /// <param name="visibility"></param>
    public void UpdateGhostVisibility(bool visibility)
    {
        _ghosts.ToList().ForEach(ghost => ghost.gameObject.SetActive(visibility));
    }
    
    
    /// <summary>
    /// Method BuildsPellets
    /// This method build all small pellet on map
    /// </summary>
    private void BuildsPellets()
    {
        // Get the container transform to set childs pellets
        Transform pelletParent = GameObject.Find("Pellets").transform;
        
        // Get tileMap limits.
        BoundsInt bounds = tilemap[1].cellBounds;
        
        // Start matrix loop
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                // Get current cell
                var cell = new Vector3Int(x, y, 0);
                
                // Check if cell is null in all traget tilemaps
                if (tilemap[1].GetTile(cell) == null && 
                    tilemap[0].GetTile(cell) == null && 
                    tilemap[2].GetTile(cell) == null)
                {
                    
                    // Get the world position base on wall tilemap. Only on null cell
                    var worldPosition = tilemap[1].CellToWorld(cell);
                    worldPosition += tilemap[1].cellSize / 2;
                    
                    // Instantiate a pellet prefab and set into pellets container [parent]
                    var pellet = Instantiate(sPellet , worldPosition, Quaternion.identity);
                    pellet.transform.SetParent(pelletParent);
                }
            }
        }
    }
    
    /// <summary>
    /// IEnumerator ManageReadyPanel [Corrutine]
    /// Manages the ready mode state
    /// </summary>
    /// <returns></returns>
    private IEnumerator ManageReadyPanel()
    {
        readyPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        readyPanel.SetActive(false);
        gameState = GameState.InGame;
        
    }
    
    /// <summary>
    /// Getter GetPowerUpTime
    /// </summary>
    /// <returns>Value of power up time</returns>
    public int GetPowerUpTime()
    {
        return POWER_UP_TIME;
    }
}
