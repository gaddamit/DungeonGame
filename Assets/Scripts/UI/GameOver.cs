using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if(!_gameManager)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonRestartPressed()
    {
        _gameManager?.Restart();
    }

    public void OnButtonQuitPressed()
    {
        _gameManager.Quit();
    }

    public void OnButtonResumePressed()
    {
        _gameManager.Resume();
    }
}
