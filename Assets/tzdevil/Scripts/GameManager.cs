using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace tzdevil.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public UnityEvent<HashSet<Vector3>> OnFindBlankPlace;

        [SerializeField] private HashSet<Vector3> _blankPlaces;

        private void Awake()
        {
            Instance = this;

            _blankPlaces = new();
        }

        private void OnEnable()
        {
            OnFindBlankPlace.AddListener(FoundBlankPlace);
        }

        private void OnDisable()
        {
            OnFindBlankPlace.RemoveListener(FoundBlankPlace);
        }

        private void FoundBlankPlace(HashSet<Vector3> blankPlaces)
        {
            foreach (var place in blankPlaces)
                _blankPlaces.Add(place);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                ShowAllBlankPlaces();
        }

        private void ShowAllBlankPlaces()
        {
            foreach (var place in _blankPlaces)
            {
                print(place);
                Debug.DrawRay(place, Vector3.up, Color.red, 999);
            }
        }
    }
}