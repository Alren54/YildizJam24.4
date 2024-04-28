using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildings = new();
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private TextMeshProUGUI endGameTime;
    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
    }
    public void CheckIfAllBuildingsAlive()
    {
        GameOverPanel.SetActive(true);
        StringBuilder str = new();
        str.Append("You lived for " + (int)timer + " seconds");
        endGameTime.SetText(str.ToString());
        Time.timeScale = 0f;
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
