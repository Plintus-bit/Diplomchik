using Cinemachine;
using UnityEngine;

namespace CameraSettings
{
    public class CameraInitializer : MonoBehaviour
    {
        private CinemachineConfiner _confiner;

        public Collider constraintCollider;

        private void Awake()
        {
            _confiner = GetComponent<CinemachineConfiner>();
        }

        private void Start()
        {
            _confiner.m_BoundingVolume = constraintCollider;
        }
    }
}