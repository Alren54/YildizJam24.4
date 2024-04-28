using Alren;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private DisasterTimer disasterTimer;
    private ResourceGathering resourceGathering;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private TowerManager towerManager;
    private Keyboard _keyboard;

    [Header("Camera Animation")]
    [SerializeField] private Animator _cameraAnimator;
    private int[] _cameraAnimHashes = new int[2]
    {
            Animator.StringToHash("MainMenu"),
            Animator.StringToHash("Game")
    };

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        resourceGathering = GetComponent<ResourceGathering>();
        _keyboard = Keyboard.current;
    }

    private void Update()
    {
        if(_keyboard.escapeKey.wasPressedThisFrame && !mainMenu.activeInHierarchy && !creditsMenu.activeInHierarchy && !creditsMenu.activeInHierarchy)
        {
            if (PauseMenu.activeInHierarchy)
            {
                PauseMenu.SetActive(false);
                Time.timeScale = 1f;
                HUD.SetActive(true);
            }
            else
            {
                PauseMenu.SetActive(true);
                Time.timeScale = 0f;
                HUD.SetActive(false);
            }
        }
    }

    public void StartGame()
    {
        HUD.SetActive(true);
        mainMenu.SetActive(false);
        disasterTimer.timerStarted = true;
        resourceGathering.isGameStarted = true;
        towerManager.isGameStarted = true;

        _cameraAnimator.Play(_cameraAnimHashes[1]);
    }

    public void QuitGame()
    {
        print("q");
        Application.Quit();
    }

    public void ResumeButton()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        HUD.SetActive(true);
    }

    public void LoadCredits()
    {
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void BackFromCredits()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }
}
