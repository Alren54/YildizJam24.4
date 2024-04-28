using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

namespace tzdevil.Gameplay
{
    [System.Serializable]
    public class HexagonMeshMaterial
    {
        [field: SerializeField] public Mesh Mesh { get; set; }
        [field: SerializeField] public Material[] MaterialList { get; set; }
    }

    public class Hexagon : MonoBehaviour/*, IPointerClickHandler*/
    {
        private Random _random = new();

        private Mouse _mouse;
        private Keyboard _keyboard;

        private const float MOVING_SMOOTHNESS = 60f;

        [Header("References")]
        [SerializeField] protected Transform _transform;
        [SerializeField] private tzdevil.Gameplay.GameManager _gameManager;
        [SerializeField] protected Alren.GameManager _alrenManager;
        [SerializeField] private Camera _camera;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private MeshFilter _meshFilter;

        [Header("Open Places")]
        [SerializeField] private List<Vector3> _placesYouCanPlaceHexagon;

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

        [field: Header("Game Loop")]
        [field: SerializeField] public bool Placed { get; set;  }
        [SerializeField] private Vector3 _lastPosition;
        [SerializeField] private RaycastHit[] _results;

        private void Awake()
        {
            _transform = transform;
            _camera = Camera.main;
            _renderer = GetComponent<Renderer>();

            _mouse = Mouse.current;
            _keyboard = Keyboard.current;

            if (!Placed)
                gameObject.layer = 0;

            _results = new RaycastHit[6];
        }

        private void Start()
        {
            _gameManager = tzdevil.Gameplay.GameManager.Instance;
            _alrenManager = Alren.GameManager.Instance;

            _gameManager.OnTryBuyNewHexagon.AddListener(TryBuyNewHexagon);

            SendRaycasts();
        }

        public void SetHexagonSettings(HexagonType hexagonType, HexagonMeshMaterial hexagonMeshMaterial, List<Vector3> openPlaces)
        {
            HexagonType = hexagonType;
            _hexagonMeshMaterial = hexagonMeshMaterial;

            _meshFilter.mesh = _hexagonMeshMaterial.Mesh;
            _renderer.materials = _hexagonMeshMaterial.MaterialList;

            _placesYouCanPlaceHexagon = openPlaces;
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

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    if (eventData.button == PointerEventData.InputButton.Left && !_placed)
        //    {
        //        var pos = GetRoundedPosition(_transform.position);
        //        pos.y = 0;

        //        if (!_placesYouCanPlaceHexagon.Contains(pos))
        //            return;

        //        _placed = true;
        //        _gameManager.OnPlaceNewHexagon?.Invoke(this);
        //        Debug.Log("placed", gameObject);

        //        gameObject.layer = 6;
        //    }
        //}

        private void Update()
        {
            HexagonMovement();

            CheckHexagonPlacement();

            if (_keyboard.uKey.wasPressedThisFrame)
                CheckIfItsIsland(_raycastPoses, new());
        }

        private void CheckHexagonPlacement()
        {
            if (_mouse.leftButton.wasPressedThisFrame && !Placed)
            {
                var pos = GetRoundedPosition(_transform.position);

                var count = 0;
                foreach (var raycastPos in _raycastPoses)
                {
                    if (Physics.RaycastNonAlloc(pos + raycastPos + Vector3.up * 10, Vector3.down, _results, Mathf.Infinity, 1 << 6) > 0)
                        count++;
                }

                if (count == 0)
                    return;

                Placed = true;
                _gameManager.OnPlaceNewHexagon?.Invoke(this);
                Debug.Log("placed", gameObject);

                _transform.localScale = Vector3.zero;
                _transform.position += new Vector3(0, _random.Next(-4, 2) / 10f, 0);

                _transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutBack);

                gameObject.layer = 6;
            }
        }

        private void HexagonMovement()
        {
            if (!Placed)
            {
                var pos = GetRoundedPositionHolding();
                _transform.position = Vector3.Lerp(_transform.position, pos, MOVING_SMOOTHNESS * Time.deltaTime);
            }
        }

        private Vector3 GetRoundedPositionHolding()
        {
            Ray ray = _camera.ScreenPointToRay(_mouse.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << 6 | 1 << 7))
            {
                Vector3 pointAboveMouse = GetRoundedPosition(hit.point);

                var count = 0;
                foreach (var pos in _raycastPoses)
                {
                    if (Physics.RaycastNonAlloc(pointAboveMouse + pos + Vector3.up * 10, Vector3.down, _results, Mathf.Infinity, 1 << 6) > 0)
                        count++;
                }

                if (count == 0)
                {
                    //print("You can't place the hexagon here!");
                }

                if (Physics.Raycast(pointAboveMouse + Vector3.up * 10, Vector3.down, out RaycastHit hit2, Mathf.Infinity, 1 << 6 | 1 << 7))
                {
                    if (hit2.collider.gameObject.layer == 6)
                        return _lastPosition;

                    Vector3 finalPos = hit2.point;

                    _lastPosition = finalPos;

                    return finalPos;
                }
                else
                    return _lastPosition;
            }
            else
                return _lastPosition;
        }

        private Vector3 GetRoundedPosition(Vector3 position)
        {
            float snappedX = Mathf.Round(position.x / 1.75f) * 1.75f;
            position.x = snappedX;

            if (snappedX % 3.5f != 0)
            {
                int rounded = (int)Mathf.Round(position.z);
                float snappedZ = rounded % 2 == 0 ? rounded - 1 : rounded;
                position.z = snappedZ;
            }
            else
            {
                int rounded = (int)Mathf.Round(position.z);
                float snappedZ = rounded % 2 == 0 ? rounded : rounded + 1;
                position.z = snappedZ;
            }

            position.y = 0;

            return position;
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