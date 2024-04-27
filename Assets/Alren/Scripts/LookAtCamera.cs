using UnityEngine;

namespace ProjectLittle.GameplayRelated
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _camera;

        void Awake()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            transform.rotation = _camera.transform.rotation;
        }
    }
}