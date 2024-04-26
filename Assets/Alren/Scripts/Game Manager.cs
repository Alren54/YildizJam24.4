using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Alren
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Resource> res = new();
        [SerializeField] private List<TextMeshProUGUI> resCountTexts = new();



        [SerializeField] private TextMeshProUGUI changeResourceCountText;
        // Sand 0
        // Stone 1
        // Food 2
        // Villager 3

        private void Start()
        {
            SetResourceTexts();
        }

        public void IncreaseElementCount(int element)
        {
            if (res[3].AvailableCount > 0)
            {
                res[element].AvailableCount++;
                changeResourceCountText.SetText(res[element].AvailableCount.ToString());
                resCountTexts[element].SetText(res[element].AvailableCount.ToString());
            }
        }
        public void DecreaseElementCount(int element)
        {
            if (res[element].AvailableCount > 0)
            {
                res[element].AvailableCount--;
                changeResourceCountText.SetText(res[element].AvailableCount.ToString());
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

        }
    }

}
