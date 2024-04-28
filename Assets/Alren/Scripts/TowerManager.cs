using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> towerFloors = new();
    [SerializeField] private float floorCreationTimeInterval = 45f;
    [SerializeField] private float floorElevationTime = 3f;
    [HideInInspector] public bool isGameStarted;
    private int deactiveIndex = 1;
    private float timer;
    void Start()
    {
        timer = 0;
    }

    void Update()
    {
        if (isGameStarted)
        {
            timer += Time.deltaTime;
            if(towerFloors.Count > deactiveIndex && timer > floorCreationTimeInterval * deactiveIndex)
            {
                towerFloors[deactiveIndex].SetActive(true);
                towerFloors[deactiveIndex].transform.DOLocalMoveY(0, floorElevationTime);
                deactiveIndex++;
            }
        }
    }

    
}
