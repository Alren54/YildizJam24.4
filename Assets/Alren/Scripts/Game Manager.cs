using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using tzdevil.Gameplay;
using UnityEngine;

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
        [SerializeField] private tzdevil.Gameplay.GameManager tzGameManager;

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

        [SerializeField] private List<int> prices;

        [Header("Hexagons")]
        [HideInInspector] public List<GameObject> allHexagons;
        [SerializeField] LayerMask _hexagonLayer;
        private List<GameObject> bayHexagons = new();
        private List<GameObject> sandHexagons = new();
        private List<Vector3> _raycastPoses = new() {
            new(-1.75f, 0, 1),
            new(0, 0, 2),
            new(1.75f, 0, 1),
            new(-1.75f, 0, -1),
            new(0, 0, -2),
            new(1.75f, 0, -1)};
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
            allHexagons = FindObjectsByType<Hexagon>(FindObjectsSortMode.None).Where(h => h != this).Select(h => h.gameObject).ToList();
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

        public void SetResourceCountTexts()
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
                str.Append(prices[i]);
                text.SetText(str.ToString());
                i++;
            }
        }

        public void BuyHexagon(int element)
        {
            if (res[element].ResourceCount >= prices[element])
            {
                res[element].ResourceCount -= prices[element];
                StartCoroutine(tzGameManager.BuyNewBlock(element));
                SetResourceCountTexts();
            }
        }

        public GameObject GatherBay()
        {
            bayHexagons.Clear();
            foreach (var hexagon in allHexagons)
            {
                foreach (var pos in _raycastPoses)
                {
                    if (!Physics.Raycast(hexagon.transform.position + pos + new Vector3(0, 10, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, _hexagonLayer))
                    {
                        bayHexagons.Add(hexagon);
                        print("Kenar Hexa sec");
                        break;
                    }
                }
            }
            return bayHexagons[Random.Range(0, bayHexagons.Count)]; //Select Bay
        }

        public GameObject GatherSandTiles(GameObject hexagon)
        {
            sandHexagons.Clear();
            foreach (var pos in _raycastPoses)
            {
                if (Physics.Raycast(hexagon.transform.position + pos + new Vector3(0, 10, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, _hexagonLayer))
                {
                    sandHexagons.Add(hit.collider.gameObject);
                    print("Kum Hexa sec");
                }
            }
            if (sandHexagons.Count > 0)
            {
                return sandHexagons[Random.Range(0, sandHexagons.Count)];
            }
            return null;
        }

        public IEnumerator StartDisaster(GameObject mainHex)
        {
            print("Felaket Basliyor");

            mainHex.transform.DOScaleY(0, .5f).SetEase(Ease.InCubic);

            yield return new WaitForSeconds(.51f);

            print("Felaketin ilki bitti");

            allHexagons.Remove(mainHex);

            HexagonType destroyedHexType = mainHex.GetComponent<Hexagon>().HexagonType;

            var alternativeHex = GatherSandTiles(mainHex);

            Destroy(mainHex);

            if (destroyedHexType == HexagonType.Sand && alternativeHex != null)
            {
                alternativeHex.transform.DOScaleY(0, .5f).SetEase(Ease.InCubic);

                yield return new WaitForSeconds(.51f);

                print("Felaketin ikimcisi bitti");
                allHexagons.Remove(alternativeHex);
                Destroy(alternativeHex);
            }
        }
    }
}