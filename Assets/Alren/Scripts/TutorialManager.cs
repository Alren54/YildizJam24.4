using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] List<Resource> resources = new();
    [SerializeField] GameObject tutorialBox;
    [SerializeField] private TextMeshProUGUI _textTitle;
    [SerializeField] private TextMeshProUGUI _textDescription;
    
    public void OpenTutorialBox(int element)
    {
        tutorialBox.SetActive(true);
        _textTitle.SetText(resources[element].Name);
        _textDescription.SetText(resources[element].Description);
    }

    public void CloseTutorialBox()
    {
        tutorialBox.SetActive(false);
    }
}
