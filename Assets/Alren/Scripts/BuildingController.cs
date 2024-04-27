using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildingPanels;
    public void OpenBuildingInterface(int element)
    {
        buildingPanels[element].SetActive(true);
    }

    public void CloseBuildingInterface(int element)
    {

    }
}
