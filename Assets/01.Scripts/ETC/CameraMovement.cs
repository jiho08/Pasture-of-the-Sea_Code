using Code.Input;
using UnityEngine;

namespace Code.ETC
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        
        [Header("Camera Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private float radius;
        [SerializeField] private float rotateSpeed = 50f;
        [SerializeField] private float moveSmoothTime = 0.1f;
        
        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 1f;
        [SerializeField] private float zoomMin = -9f;
        [SerializeField] private float zoomMax = -2.5f;
        
        private float _currentAngle;
        private Vector3 _currentVelocity;
        private Vector3 _desiredPosition;
        
        private void Update()
        {
            _currentAngle += -playerInput.MoveInput.x * rotateSpeed * Time.deltaTime;

            if (Mathf.Abs(playerInput.ScrollInput.y) > 0.01f)
            {
                radius += playerInput.ScrollInput.y * zoomSpeed;
                radius = Mathf.Clamp(radius, zoomMin, zoomMax);
            }
            
            var rad = _currentAngle * Mathf.Deg2Rad;
            var offset = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * radius;
            
            _desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, target.position.z + offset.z);
        }

        private void LateUpdate()
        {
            transform.position = Vector3.SmoothDamp(transform.position, _desiredPosition, ref _currentVelocity, moveSmoothTime);
            transform.LookAt(target);
        }
    }
}