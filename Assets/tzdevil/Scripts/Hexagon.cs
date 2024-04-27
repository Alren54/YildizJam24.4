using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace tzdevil.Gameplay
{
    public class Hexagon : MonoBehaviour, IPointerClickHandler
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private Camera _camera;
        [SerializeField] private Renderer _renderer;

        [Header("Raycast Settings")]
        [SerializeField]
        private List<Vector3> _raycastPoses = new() {
            new(-1.75f, 0, 1),
            new(0, 0, 2),
            new(1.75f, 0, 1),
            new(-1.75f, 0, -1),
            new(0, 0, -2),
            new(1.75f, 0, -1)};
        [SerializeField] private int _hexagonLayer = 1 << 6;
        [SerializeField] private HashSet<Vector3> _blankPlaces = new();

        [Header("Hexagon Settings")]
        [SerializeField] public HexagonType _hexagonType;
        [SerializeField] private Material _material;

        [Header("Game Loop")]
        [SerializeField] private bool _placed;

        private void Awake()
        {
            _transform = transform;
            _camera = Camera.main;
            _renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;

            _gameManager.OnTryBuyNewHexagon.AddListener(TryBuyNewHexagon);

            SendRaycasts();
        }

        public void SetHexagonSettings(HexagonType hexagonType, Material material)
        {
            _hexagonType = hexagonType;
            _material = material;
        }

        private void OnDisable()
        {
            _gameManager.OnTryBuyNewHexagon.RemoveListener(TryBuyNewHexagon);
        }

        private void TryBuyNewHexagon()
        {
            SendRaycasts();
        }

        private void SendRaycasts()
        {
            _blankPlaces = new();

            foreach (var pos in _raycastPoses)
            {
                if (!Physics.Raycast(_transform.position + pos + new Vector3(0, 10, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, _hexagonLayer))
                    _blankPlaces.Add(_transform.position + pos);
            }

            _gameManager.OnFindBlankHexagon?.Invoke(_blankPlaces);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && !_placed)
            {
                _placed = true;
                _gameManager.OnPlaceNewHexagon?.Invoke(gameObject);
                _renderer.material = _material;
            }
        }
    }
}