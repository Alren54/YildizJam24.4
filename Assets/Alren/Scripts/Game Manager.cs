using System.Collections;
using tzdevil.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

namespace Alren
{
    public class GameManager : MonoBehaviour
    {
        [Header("Resources and UI Relatives")]
        public List<Resource> res = new();
        [SerializeField] private List<TextMeshProUGUI> changeWorkerCountTexts = new();

        [SerializeField] private List<TextMeshProUGUI> resWorkerCountTexts = new();
        [SerializeField] private List<TextMeshProUGUI> resCountTexts = new();

        [SerializeField] private List<TextMeshProUGUI> marketResourceCountTexts = new();
        [SerializeField] private tzdevil.Gameplay.GameManager tzGameManager = new();
        [SerializeField] private Hexagon hexagon = new();
        private HexagonType currentHex = new();

        // Sand 0
        // Stone 1
        // Food 2
        // Villager 3

        [Header("Starting Resources")]
        [SerializeField] private int sandWorkerCount;
        [SerializeField] private int sandResourceCount;
        [SerializeField] private int stoneWorkerCount;
        [SerializeField] private int stoneResourceCount;
        [SerializeField] private int foodWorkerCount;
        [SerializeField] private int foodResourceCount;
        [SerializeField] private int villagerWorkerCount;
        [SerializeField] private int villagerResourceCount;


        private void Start()
        {
            res[0].WorkerCount = sandWorkerCount;
            res[0].ResourceCount = sandResourceCount;
            res[1].WorkerCount = stoneWorkerCount;
            res[1].ResourceCount = stoneResourceCount;
            res[2].WorkerCount = foodWorkerCount;
            res[2].ResourceCount = foodResourceCount;
            res[3].WorkerCount = villagerWorkerCount;
            res[3].ResourceCount = villagerResourceCount;
            SetWorkerTexts();
            SetResourceCountTexts();
        }

        public void IncreaseElementCount(int element)
        {
            if (res[3].WorkerCount > 0)
            {
                res[element].WorkerCount++;
                res[3].WorkerCount--;
                SetWorkerTexts();
            }
        }
        public void DecreaseElementCount(int element)
        {
            if (res[element].WorkerCount > 0)
            {
                res[element].WorkerCount--;
                res[3].WorkerCount++;
                SetWorkerTexts();
            }
        }

        void SetWorkerTexts()
        {
            int i = 0;
            foreach (var text in resWorkerCountTexts)
            {
                text.SetText(res[i].WorkerCount.ToString());
                i++;
            }
            i = 0;
            foreach (var text in changeWorkerCountTexts)
            {
                text.SetText(res[i].WorkerCount.ToString());
                i++;
            }
        }

        private void SetResourceCountTexts()
        {
            int i = 0;
            foreach (var text in resCountTexts)
            {
                text.SetText(res[i].ResourceCount.ToString());
                i++;
            }
            i = 0;
            foreach (var text in marketResourceCountTexts)
            {
                StringBuilder str = new();
                str.Append(res[i].ResourceCount.ToString());
                str.Append('/');
                str.Append("20");
                text.SetText(str.ToString());
                i++;
            }
        }

        public void BuyHexagon(int element)
        {
            if (res[element].ResourceCount >= 20)
            {
                res[element].ResourceCount -= 20;
                StartCoroutine(tzGameManager.BuyNewBlock(element));
                SetResourceCountTexts();
            }
        }

        public void StartDisaster()
        {

        }
    }

}
