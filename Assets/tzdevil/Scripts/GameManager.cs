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

        public UnityEvent OnTryBuyNewHexagon;
        public UnityEvent<HashSet<Vector3>> OnFindBlankHexagon;
        public UnityEvent<GameObject> OnPlaceNewHexagon;

        [SerializeField] private HashSet<Vector3> _blankPlaces;
        [SerializeField] private int hexagonCount;

        [Header("Game Loop")]
        [SerializeField] private GameObject _hexagonPrefab;
        [SerializeField] private List<GameObject> _hexagonList;
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

        public IEnumerator BuyNewBlock()
        {
            _alreadySelecting = true;

            _blankPlaces.Clear();
            OnTryBuyNewHexagon?.Invoke();

            yield return new WaitForSeconds(.1f);

            ShowAllBlankPlaces();
        }

        private void FoundBlankPlace(HashSet<Vector3> blankPlaces)
        {
            foreach (var place in blankPlaces)
                _blankPlaces.Add(place);
        }

        private void PlacedNewHexagon(GameObject hexagon)
        {
            List<GameObject> listToBeRemoved = new();
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

            _alreadySelecting = false;
        }

        private void Update()
        {
            if (_keyboard.lKey.wasPressedThisFrame && !_alreadySelecting)
                StartCoroutine(BuyNewBlock());
        }

        private void ShowAllBlankPlaces()
        {
            int i = 0;
            foreach (var place in _blankPlaces)
            {
                if (i <= _hexagonList.Count)
                {
                    var hexagon = Instantiate(_hexagonPrefab, place, Quaternion.identity);
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