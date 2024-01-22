using UnityEngine;

namespace Entity.Turtle
{
    public class CameraRig : MonoBehaviour
    {
        #region Private Fields

        [SerializeField]
        private float mouseSensX, mouseSensY;

        private float _currentX, _currentY;

        private float _mouseDeltaX, _mouseDeltaY;

        private float _mouseCacheX, _mouseCacheY;

        private Vector3 _targetRotation;

        private LayerMask _notTurtleMask;

        private Transform _cachedTransform;

        #endregion

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _notTurtleMask = ~LayerMask.GetMask("Turtle");
            _cachedTransform = transform;
        }

        private void Update()
        {
            _currentX = (Input.GetAxisRaw("Horizontal") * mouseSensX) + _currentX;
            _currentY = Mathf.Clamp((Input.GetAxisRaw("Vertical") * mouseSensY) + _currentY, -90, 90);

            _targetRotation = new Vector3(0, _currentX, _currentY);
            transform.rotation = Quaternion.Euler(_targetRotation.x, _targetRotation.y, _targetRotation.z);

            _cachedTransform.GetChild(0).localPosition =
                Physics.Raycast(new Ray(_cachedTransform.position, -_cachedTransform.right), out var hit, 4f,
                    _notTurtleMask)
                    ? new Vector3(-hit.distance, 0, 0)
                    : new Vector3(-4, 0, 0);
        }
    }
}
