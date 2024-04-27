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
            SearchAllHexagons();
        }

        public void SearchAllHexagons()
        {
            var allHexagons = FindObjectsByType<Hexagon>(FindObjectsSortMode.None).Where(h => h != this).Select(h => h.transform);

            var connectedObjects = FindConnectedObjects(transform);

            var islandList = allHexagons.Except(connectedObjects).ToList();

            foreach (var obj in islandList)
            {
                Debug.Log("island", obj.gameObject);
            }
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