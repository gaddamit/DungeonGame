using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGamePaused = false;
    private bool _isGameOver = false;
    private int _totalCollectables = 0;
    private int _collected = 0;
    private Collectable[] _collectables;

    [SerializeField]
    private TMP_Text _textCollected;

    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private TimerController _timerController;

    private ActivatableObject[] _activatableObjects;
    private EnemyAI[] _enemyAIs;

    // Start is called before the first frame update
    void Start()
    {
        _collectables = FindObjectsOfType<Collectable>();
        _totalCollectables = _collectables.Length;

        UpdateTextCollected();

        _activatableObjects = FindObjectsOfType<ActivatableObject>();

        _enemyAIs = FindObjectsOfType<EnemyAI>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !_isGameOver)
        {
            if(_isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void CollectItem(Collectable collectable)
    {
        _collected++;
        UpdateTextCollected();

        CheckIfGameOver();
    }

    public void UpdateTextCollected()
    {
        if(_textCollected)
        {
            _textCollected.text = $"Collected: {_collected.ToString()}/{_totalCollectables.ToString()}";
        }
    }

    private void CheckIfGameOver()
    {
        if(_collected >= _totalCollectables)
        {
            _isGameOver = true;

            // Disable player input 
            _playerController?.SetControlsActive(false);

            // Load Game Over scene
            Invoke("DisplayGameOverScene", 2);
        }
    }

    private void PauseGame(string sceneName = "Pause")
    {
        _isGamePaused = true;
        _timerController.IsPaused = _isGamePaused;

        DisplayMenuScene(sceneName);

        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        _isGamePaused = false;
        _timerController.IsPaused = _isGamePaused;
        
        Time.timeScale = 1.0f;

        Invoke("HideMenuScene", 0.25f);
    }

    private void DisplayGameOverScene()
    {
        PauseGame("GameOver");
    }

    private void DisplayMenuScene(string name)
    {
        HideMenuScene();
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }

    public void Restart()
    {
        _isGameOver = false;

        // UI
        _collected = 0;
        UpdateTextCollected();

        _timerController.ResetTimer();

        // Player
        _playerController?.Reset();

        // Collectibles
        foreach(Collectable collectable in _collectables)
        {
            collectable.Reset();
        }

        // Activatable Objects
        foreach(ActivatableObject ao in _activatableObjects)
        {
            ao.Reset();
        }

        // Enemy AIs
        foreach(EnemyAI enemyAI in _enemyAIs)
        {
            enemyAI.Reset();
        }

        ResumeGame();
    }

    private void HideMenuScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "Pause" || scene.name == "GameOver")
        {
            SceneManager.UnloadSceneAsync(scene.name);
        }
    }

    public void Quit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void Resume()
    {
        ResumeGame();
    }
}
