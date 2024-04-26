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

        private void Awake()
        {
            _transform = transform;
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
                if (!Physics.Raycast(_transform.position + pos + new Vector3(0, 10, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, _hexagonLayer))
                {
                    _blankPlaces.Add(_transform.position + pos);
                }
            }

            _gameManager.OnFindBlankPlace?.Invoke(_blankPlaces);
        }
    }
}