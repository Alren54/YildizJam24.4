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
    }

    public void QuitGame()
    {
        print("q");
        Application.Quit();
    }
}
