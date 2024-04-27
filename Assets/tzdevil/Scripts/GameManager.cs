using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace tzdevil.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        private Mouse _mouse;
        private Keyboard _keyboard;

        public static GameManager Instance;

        [Header("References")]
        [SerializeField] private Alren.GameManager _alrenGameManager;

        public UnityEvent OnTryBuyNewHexagon;
        public UnityEvent<HashSet<Vector3>> OnFindBlankHexagon;
        public UnityEvent<Hexagon> OnPlaceNewHexagon;

        [Header("Hexagon Settings")]
        [SerializeField] private HexagonMeshMaterial[] _hexagonMeshMaterialList;

        [Header("Hexagon Properties")]
        [SerializeField] private HashSet<Vector3> _blankPlaces;
        [SerializeField] private int hexagonCount;

        [Header("Game Loop")]
        [SerializeField] private Hexagon _hexagonPrefab;
        [SerializeField] private List<Hexagon> _hexagonList;
        [SerializeField] private bool _alreadySelecting;

        private void Awake()
        {
            Instance = this;

            _blankPlaces = new();

            _mouse = Mouse.current;
            _keyboard = Keyboard.current;
        }

        private void OnEnable()
        {
            OnFindBlankHexagon.AddListener(FoundBlankPlace);
            OnPlaceNewHexagon.AddListener(PlacedNewHexagon);
        }

        private void OnDisable()
        {
            OnFindBlankHexagon.RemoveListener(FoundBlankPlace);
            OnPlaceNewHexagon.RemoveListener(PlacedNewHexagon);
        }

        public IEnumerator BuyNewBlock(int elementId)
        {
            if (_alreadySelecting)
                yield break;

            _alreadySelecting = true;

            _blankPlaces.Clear();
            OnTryBuyNewHexagon?.Invoke();

            yield return new WaitForSeconds(.1f);

            ShowAllBlankPlaces(elementId);
        }

        private void FoundBlankPlace(HashSet<Vector3> blankPlaces)
        {
            foreach (var place in blankPlaces)
                _blankPlaces.Add(place);
        }

        private void PlacedNewHexagon(Hexagon hexagon)
        {
            List<Hexagon> listToBeRemoved = new();
            foreach (var place in _hexagonList)
            {
                if (place == hexagon)
                {
                    listToBeRemoved.Add(place);
                    continue;
                }

                place.transform.position = Vector3.one * 999;
            }

            foreach (var place in listToBeRemoved)
                _hexagonList.Remove(place);

            _alrenGameManager.AllHexagons.Add(hexagon.gameObject);

            _alreadySelecting = false;
        }

        private void ShowAllBlankPlaces(int elementId)
        {
            int i = 0;
            foreach (var place in _blankPlaces)
            {
                if (i <= _hexagonList.Count)
                {
                    var hexagonType = (HexagonType)elementId;
                    var hexagonMeshMaterial = _hexagonMeshMaterialList[elementId];

                    var hexagon = Instantiate(_hexagonPrefab, place, Quaternion.identity);
                    hexagon.SetHexagonSettings(hexagonType, hexagonMeshMaterial);
                    _hexagonList.Add(hexagon);
                }
                else
                {
                    _hexagonList[i].transform.position = place;
                }

                i++;
            }
        }
    }
}