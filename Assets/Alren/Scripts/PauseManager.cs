using Alren;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private DisasterTimer disasterTimer;
    private ResourceGathering resourceGathering;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private TowerManager towerManager;

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
    }

    public void StartGame()
    {
        HUD.SetActive(true);
        mainMenu.SetActive(false);
        disasterTimer.timerStarted = true;
        resourceGathering.isGameStarted = true;
        //towerManager.isGameStarted = true;

        _cameraAnimator.Play(_cameraAnimHashes[1]);
    }

    public void QuitGame()
    {
        print("q");
        Application.Quit();
    }
}
