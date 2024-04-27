using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildingPanels;
    public void OpenAndCloseBuildingInterface(int element)
    {
        if (!buildingPanels[element].activeInHierarchy)
        {
            buildingPanels[element].SetActive(true);
        }
        else buildingPanels[element].SetActive(false);

    }
}
