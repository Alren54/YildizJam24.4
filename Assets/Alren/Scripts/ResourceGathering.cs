using Alren;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGathering : MonoBehaviour
{
    [SerializeField] private double SandGatherTime = 10;
    [SerializeField] private double StoneGatherTime = 10;
    [SerializeField] private double FoodGatherTime = 10;
    private double currentSandGatherTime;
    private double currentStoneGatherTime;
    private double currentFoodGatherTime;
    private double SandTimer;
    private double StoneTimer;
    private double FoodTimer;
    private double tempTimer;
    private bool isResourceChanged;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        SandTimer = StoneTimer = FoodTimer = 0;
        currentSandGatherTime = SandGatherTime;
        currentStoneGatherTime = StoneGatherTime;
        currentFoodGatherTime = FoodGatherTime;
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        isResourceChanged = false;
        SandTimer += Time.deltaTime;
        StoneTimer += Time.deltaTime;
        FoodTimer += Time.deltaTime;
        if ((int)(10 * SandTimer) >= (int)(10 * currentSandGatherTime) && gameManager.res[0].WorkerCount > 0)
        {
            gameManager.res[0].ResourceCount++;
            isResourceChanged = true;
            SandTimer = 0;
            
            if (gameManager.res[0].WorkerCount > 0)
            {
                tempTimer = SandGatherTime;
                for (int i = 0; i < gameManager.res[0].WorkerCount; i++)
                {
                    tempTimer = tempTimer * 9 / 10;
                }
                currentSandGatherTime = tempTimer; 
            }

        }
        if ((int)(10 * StoneTimer) >= (int)(10 * currentStoneGatherTime) && gameManager.res[1].WorkerCount > 0)
        {
            gameManager.res[1].ResourceCount++;
            isResourceChanged = true;
            StoneTimer = 0;

            if (gameManager.res[1].WorkerCount > 0)
            {
                tempTimer = StoneGatherTime;
                for (int i = 0; i < gameManager.res[1].WorkerCount; i++)
                {
                    tempTimer = tempTimer * 9 / 10;
                }
                currentStoneGatherTime = tempTimer;
            }
        }
        if ((int)(10 * FoodTimer) >= (int)(10 * currentFoodGatherTime) && gameManager.res[2].WorkerCount > 0)
        {
            gameManager.res[2].ResourceCount++;
            isResourceChanged = true;
            FoodTimer = 0;
            
            if (gameManager.res[2].WorkerCount > 0)
            {
                tempTimer = FoodGatherTime;
                for (int i = 0; i < gameManager.res[2].WorkerCount; i++)
                {
                    tempTimer = tempTimer * 9 / 10;
                }
                currentFoodGatherTime = tempTimer;
            }
        }
        if (isResourceChanged)
        {
            gameManager.SetResourceCountTexts();
        }
    }
}
