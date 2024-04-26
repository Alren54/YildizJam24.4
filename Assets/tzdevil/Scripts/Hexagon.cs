using System.Collections.Generic;
using UnityEngine;

namespace tzdevil.Gameplay
{
    public class Hexagon : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private GameManager _gameManager;

        [Header("Raycast Settings")]
        [SerializeField]
        private List<Vector3> _raycastPoses = new()
    {
        new(-1.75f, 0, 1),
        new(0, 0, 2),
        new(1.75f, 0, 1),
        new(-1.75f, 0, -1),
        new(0, 0, -2),
        new(1.75f, 0, -1)
    };
        [SerializeField] private int _hexagonLayer = 1 << 6;
        [SerializeField] private HashSet<Vector3> _blankPlaces = new();

        [SerializeField] private RaycastHit[] _results;

        private void Awake()
        {
            _transform = transform;

            _results = new RaycastHit[1];
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;

            SendRaycasts();
        }

        private void SendRaycasts()
        {
            _blankPlaces = new();

            foreach (var pos in _raycastPoses)
            {
                var posStart = _transform.position + pos + Vector3.up * 2;
                var posEnd = _transform.position + pos + Vector3.down;

                if (Physics.RaycastNonAlloc(posStart, posEnd, _results, Mathf.Infinity, _hexagonLayer) <= 0)
                {
                    print(pos);
                    _blankPlaces.Add(pos);
                }

                Debug.DrawLine(posStart, posEnd, Color.red, 100);
            }

            _gameManager.OnFindBlankPlace?.Invoke(_blankPlaces);
        }
    }
}