using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Rendering.FilterWindow;
using System.Text;

namespace Alren
{
    public class GameManager : MonoBehaviour
    {
        [Header("Resources and UI Relatives")]
        [SerializeField] private List<Resource> res = new();
        [SerializeField] private List<TextMeshProUGUI> resCountTexts = new();
        [SerializeField] private List<TextMeshProUGUI> changeResourceCountText;
        [SerializeField] private List<TextMeshProUGUI> marketResourceText;
        // Sand 0
        // Stone 1
        // Food 2
        // Villager 3

        [Header("Starting Resources")]
        [SerializeField] private int sandCount;
        [SerializeField] private int stoneCount;
        [SerializeField] private int foodCount;
        [SerializeField] private int villagerCount;


        private void Start()
        {
            res[0].AvailableCount = sandCount;
            res[1].AvailableCount = stoneCount;
            res[2].AvailableCount = foodCount;
            res[3].AvailableCount = villagerCount;
            SetResourceTexts();
        }

        public void IncreaseElementCount(int element)
        {
            if (res[3].AvailableCount > 0)
            {
                res[element].AvailableCount++;
                res[3].AvailableCount--;
                SetResourceTexts();
            }
        }
        public void DecreaseElementCount(int element)
        {
            if (res[element].AvailableCount > 0)
            {
                res[element].AvailableCount--;
                res[3].AvailableCount++;
                SetResourceTexts();
            }
        }

        void SetResourceTexts()
        {
            int i = 0;
            foreach (var text in resCountTexts)
            {
                text.SetText(res[i].AvailableCount.ToString());
                i++;
            }
            i = 0;
            foreach(var text in marketResourceText)
            {
                StringBuilder str = new();
                str.Append(res[i].AvailableCount.ToString());
                str.Append('/');
                str.Append("20");
                text.SetText(str.ToString());
                i++;
            }
            i = 0;
            foreach(var text in changeResourceCountText)
            {
                text.SetText(res[i].AvailableCount.ToString());
                i++;
            }
        }

        public void BuyHexagon(int element){ 
            if(res[element].AvailableCount >= 20) res[element].AvailableCount -= 20;
            SetResourceTexts();
        }
    }

}
