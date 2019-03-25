using UnityEngine;
using System.Collections;
using HoloGroup.Patterns;

namespace HoloGroup.UserInteraction.Input.Gaze
{
    public class Cursor : MonoSingleton<Cursor>
    {
        #region Fields
        private Vector3 _targetPosition;

        private Quaternion _targetRotation;

        [SerializeField]
        [Range(1f, 32f)]
        private float _interpolationWhenIsGraphicHit = 1f;

        [SerializeField]
        private Renderer _cursorRenderer;

        private bool _isShown = true;
        #endregion

        #region Properties
        public bool IsShown
        {
            get { return _isShown; }
            set
            {
                _cursorRenderer.enabled = value;
                _isShown = value;
            }
        }
        #endregion

        #region Methods
        private void LateUpdate()
        {
            UpdateCursor(GazeManagerUtility.GetClampedHitPoint(6f), Quaternion.LookRotation(GazeManagerUtility.GetHitNormalOrDefault()));
        }

        private void UpdateCursor(Vector3 newPosition, Quaternion newRotation)
        {
            if (!GazeManager.Instance.isGraphicHit)
            {
                transform.position = newPosition;
                transform.rotation = newRotation;
            }
            else
            {
                _targetPosition = newPosition;
                _targetRotation = newRotation;

                transform.position = Vector3.Lerp(transform.position, _targetPosition, _interpolationWhenIsGraphicHit * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, _interpolationWhenIsGraphicHit * Time.deltaTime);
            }
        }
        #endregion
    }
}