using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace tzdevil.Gameplay
{
    public class Village : Hexagon
    {
        [SerializeField] private List<Transform> _connectedObjects = new();

        private void Start()
        {
            FindAllIslands();
        }

        public HashSet<Transform> FindAllIslands()
        {
            var allHexagons = FindObjectsByType<Hexagon>(FindObjectsSortMode.None).Where(h => h != this && h.Placed).Select(h => h.transform);

            _connectedObjects = new();
            var connectedObjects = FindConnectedObjects(transform);

            var islandList = allHexagons.Except(_connectedObjects).ToList();

            print(islandList.Count);

            return islandList.OrderBy(o => Vector3.Magnitude(o.transform.position - _transform.position)).ToHashSet();
        }

        private List<Transform> FindConnectedObjects(Transform currentTransform)
        {
            if (!_connectedObjects.Contains(currentTransform))
            {
                _connectedObjects.Add(currentTransform);

                foreach (Vector3 direction in _raycastPoses)
                {
                    if (Physics.Raycast(currentTransform.position + direction + new Vector3(0, 10, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, 1 << 6))
                    {
                        if (hit.transform != currentTransform)
                        {
                            FindConnectedObjects(hit.transform);
                        }
                    }
                }
            }

            return _connectedObjects;
        }
    }
}