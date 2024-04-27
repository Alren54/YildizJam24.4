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

        public List<Transform> FindAllIslands()
        {
            var allHexagons = FindObjectsByType<Hexagon>(FindObjectsSortMode.None).Where(h => h != this).Select(h => h.transform);

            var connectedObjects = FindConnectedObjects(transform);

            var islandList = allHexagons.Except(connectedObjects).ToList();

            return islandList.OrderBy(o => Vector3.Magnitude(o.transform.position - _transform.position)).ToList();
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