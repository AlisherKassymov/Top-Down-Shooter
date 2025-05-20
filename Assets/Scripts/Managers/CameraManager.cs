using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        [SerializeField] private float _distanceChangeRate;
        
        private CinemachineCamera _cinemachineCamera;
        private CinemachinePositionComposer _positionComposer;
        private float _targetCameraDistance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Two Camera managers were detected on the scene. Destroying the second one.");
                Destroy(gameObject);
            }

            _cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
            _positionComposer = GetComponentInChildren<CinemachinePositionComposer>();
        }

        private void Update()
        {
            UpdateCameraDistance();
        }

        private void UpdateCameraDistance()
        {
            float currentDistance = _positionComposer.CameraDistance;
            if (Mathf.Abs(_targetCameraDistance - currentDistance) > .01f)
            {
                _positionComposer.CameraDistance = Mathf.Lerp(_positionComposer.CameraDistance, _targetCameraDistance,
                    _distanceChangeRate * Time.deltaTime);
            }
        }

        public void ChangeCameraDistance(float distance)
        {
            _targetCameraDistance = distance;
        }
    }
}
