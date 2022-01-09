using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

enum GameState {
    MainMenu,
    InGame,
    Pause,
    GameOver
}

public class GameNavigation : MonoBehaviour
{
    public GameObject MainMenuScreen;
    public GameObject GameScreen;
    public GameObject PauseScreen;
    public GameObject GameOverScreen;

    private Timer _gameTimer;
    private GameState _gameState = GameState.MainMenu;
    
    void Start()
    {
        _gameTimer = GameObject.FindGameObjectWithTag("Timer")
            .GetComponent<Timer>();
        _gameTimer.OnTimerFinished = StopGameSound;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameState == GameState.InGame)
            {
                OnPauseButtonClicked();
            }
            else if (_gameState == GameState.Pause)
            {
                OnResumeButtonClicked();
            }
        }
    }

    public void OnStartGameButtonClicked()
    {
        var audioSources = MainMenuScreen.GetComponentsInChildren<AudioSource>();
        var startGameButtonAudio = audioSources
            .First(a => a.gameObject.name == "StartGameButton");
        var mainMenuAudio = audioSources
            .First(a => a.clip.name == "S31-Winning the Race");
        mainMenuAudio.Stop();
        startGameButtonAudio.Play();
        Invoke("SwitchToGame", startGameButtonAudio.clip.length);
    }

    public void OnMainMenuButtonClicked()
    {
        Time.timeScale = 1.0f;
        _gameTimer.ResetTimer();
        GameScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        MainMenuScreen.SetActive(true);
        PauseScreen.SetActive(false);
        _gameState = GameState.MainMenu;
    }

    public void OnPauseButtonClicked()
    {
        Time.timeScale = 0.0f;
        PauseScreen.SetActive(true);
        _gameState = GameState.Pause;
    }

    public void OnResumeButtonClicked()
    {
        Time.timeScale = 1.0f;
        PauseScreen.SetActive(false);
        _gameState = GameState.InGame;
    }

    public void OnRestartButtonClicked()
    {
        _gameTimer.ResetTimer();
        GameOverScreen.SetActive(false);
        GameScreen.SetActive(true);
        GameScreen.GetComponent<AudioSource>().Play();
        _gameTimer.StartTimer();
    }

    public void OnExitButtonClicked()
    {
        var audioSources = MainMenuScreen.GetComponentsInChildren<AudioSource>();
        var exitButtonAudio = audioSources
            .First(a => a.gameObject.name == "ExitButton");
        var mainMenuAudio = audioSources
            .First(a => a.clip.name == "S31-Winning the Race");
        mainMenuAudio.Stop();
        exitButtonAudio.Play();
        Invoke("ExitGame", exitButtonAudio.clip.length);
    }

    void SwitchToGame()
    {
        MainMenuScreen.SetActive(false);
        GameScreen.SetActive(true);
        _gameState = GameState.InGame;
        _gameTimer.StartTimer();
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void StopGameSound()
    {
        _gameState = GameState.GameOver;
        GameScreen.GetComponent<AudioSource>().Stop();
    }
}
