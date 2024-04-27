using Alren;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGathering : MonoBehaviour
{
    [SerializeField] private float SandGatherTime = 10;
    [SerializeField] private float StoneGatherTime = 10;
    [SerializeField] private float FoodGatherTime = 10;
    private float SandTimer;
    private float StoneTimer;
    private float FoodTimer;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        SandTimer = StoneTimer = FoodTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SandTimer = StoneTimer = FoodTimer += Time.deltaTime;
        if ((int)(10 * SandTimer) >= (int)(10 * SandGatherTime))
        {
            
        }
        if ((int)(10 * StoneTimer) >= (int)(10 * StoneGatherTime))
        {

        }
        if ((int)(10 * FoodTimer) >= (int)(10 * FoodGatherTime))
        {

        }
    }
}
