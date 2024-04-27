using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace tzdevil.Gameplay
{
    [System.Serializable]
    public class HexagonMeshMaterial
    {
        [field: SerializeField] public Mesh Mesh { get; set; }
        [field: SerializeField] public Material[] MaterialList { get; set; }
    }

    public class Hexagon : MonoBehaviour, IPointerClickHandler
    {
        private Keyboard _keyboard;

        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private tzdevil.Gameplay.GameManager _gameManager;
        [SerializeField] protected Alren.GameManager _alrenManager;
        [SerializeField] private Camera _camera;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private MeshFilter _meshFilter;

        [Header("Raycast Settings")]
        [SerializeField]
        protected List<Vector3> _raycastPoses = new() {
            new(-1.75f, 0, 1),
            new(0, 0, 2),
            new(1.75f, 0, 1),
            new(-1.75f, 0, -1),
            new(0, 0, -2),
            new(1.75f, 0, -1)};
        [SerializeField] private int _hexagonLayer = 1 << 6;
        [SerializeField] private HashSet<Vector3> _blankPlaces = new();

        [field: Header("Hexagon Settings")]
        [field: SerializeField] public HexagonType HexagonType { get; set; }
        [SerializeField] private HexagonMeshMaterial _hexagonMeshMaterial;

        [Header("Game Loop")]
        [SerializeField] private bool _placed;

        private void Awake()
        {
            _transform = transform;
            _camera = Camera.main;
            _renderer = GetComponent<Renderer>();

            _keyboard = Keyboard.current;
        }

        private void Start()
        {
            _gameManager = tzdevil.Gameplay.GameManager.Instance;
            _alrenManager = Alren.GameManager.Instance;

            _gameManager.OnTryBuyNewHexagon.AddListener(TryBuyNewHexagon);

            SendRaycasts();
        }

        public void SetHexagonSettings(HexagonType hexagonType, HexagonMeshMaterial hexagonMeshMaterial)
        {
            HexagonType = hexagonType;
            _hexagonMeshMaterial = hexagonMeshMaterial;
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
                _gameManager.OnPlaceNewHexagon?.Invoke(this);
                _meshFilter.mesh = _hexagonMeshMaterial.Mesh;
                _renderer.materials = _hexagonMeshMaterial.MaterialList;
                Debug.Log("placed", gameObject);
            }
        }

        private void Update()
        {
            if (_keyboard.uKey.wasPressedThisFrame)
                CheckIfItsIsland(_raycastPoses, new());
        }

        private void CheckIfItsIsland(List<Vector3> raycastPoses, List<GameObject> hexagonToBeRemovedList)
        {
            var tempPos = new List<Vector3>(raycastPoses);

            var hitCount = 0;
            foreach (var pos in tempPos)
            {
                if (Physics.Raycast(_transform.position + pos + Vector3.up, Vector3.down, out RaycastHit hit, Mathf.Infinity, _hexagonLayer))
                {
                    if (hit.collider.gameObject.CompareTag("Main"))
                    {
                        Debug.Log("h" + hexagonToBeRemovedList.Count, gameObject);
                        return;
                    }

                    var removedPos = -pos;

                    hexagonToBeRemovedList.Add(gameObject);
                    hit.collider.GetComponent<Hexagon>().CheckIfItsIsland(tempPos.Where(p => p != removedPos).ToList(), hexagonToBeRemovedList);

                    hitCount++;
                }
            }

            if (hitCount == 0)
            {
                Debug.Log(hexagonToBeRemovedList.Count, gameObject);
                return;
            }
        }
    }
}