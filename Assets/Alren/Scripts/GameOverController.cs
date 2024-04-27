using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildings = new();
    [SerializeField] private GameObject GameOverPanel;
    public void CheckIfAllBuildingsAlive()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f;

    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
