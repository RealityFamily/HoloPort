using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloGroup.UserInteraction.Input.Gaze;
using HoloGroup.Patterns;

namespace HoloGroup.UserInteraction.Input.Gesture
{
    public class GesturesHandler : MonoSingleton<GesturesHandler>, IGesturesHandler
    {
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        [Serializable]
        private struct GestureData
        {
            private UnityEngine.XR.WSA.Input.InteractionSourceKind _source;

            private Vector3 _motionData;

            private Ray _headRay;

            public UnityEngine.XR.WSA.Input.InteractionSourceKind Souce { get { return _source; } set { _source = value; } }

            public Vector3 MotionData { get { return _motionData; } set { _motionData = value; } }

            public Ray HeadRay { get { return _headRay; } set { _headRay = value; } }
        }
        #endregion

        #region Classes
        #endregion

        #region Fields
        private GameObject _targetObject;

        private byte _startGesturesCount;

        [SerializeField]
        private LayerMask _interactMask;

        [SerializeField]
        private float _doubleTapDuration = 0.3f;

        private IHoldable _holdable;

        private IManipulatable _manipulatable;

        private INavigatable _navigatable;

        private GestureData _gestureData = new GestureData();

        private Coroutine _doubleTapRoutine;

        private int _emptyPointTapCount = 0;
        #endregion

        #region Events
        #endregion

        #region Properties
        public LayerMask InteractMask
        {
            get { return _interactMask; }
            set
            {
                if (_interactMask == value)
                {
                    return;
                }

                _interactMask = value;

                if (_targetObject)
                {
                    bool targetObjectMaskDisabled = (_interactMask & LayerMask.GetMask(LayerMask.LayerToName(_targetObject.layer))) == 0;

                    if (targetObjectMaskDisabled)
                    {
                        if (_holdable != null)
                        {
                            _holdable.OnHoldCompleted(_gestureData.Souce, _gestureData.HeadRay);
                            _holdable = null;
                        }
                        else if (_manipulatable != null)
                        {
                            _manipulatable.OnManipulationCompleted(_gestureData.Souce, _gestureData.MotionData, _gestureData.HeadRay);
                            _manipulatable = null;
                        }
                        else if (_navigatable != null)
                        {
                            _navigatable.OnNavigationCompleted(_gestureData.Souce, _gestureData.MotionData, _gestureData.HeadRay);
                            _navigatable = null;
                        }
                    }
                }
            }
        }
        #endregion

        #region Methods
        private bool TargetLayerEnabled()
        {
            return (_interactMask & LayerMask.GetMask(LayerMask.LayerToName(_targetObject.layer))) != 0;
        }

        #region Errors
        public void OnGestureError(string error, int hResult)
        {
            // Just pass.
        }
        #endregion

        #region Recognitions
        public void OnRecognitionStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay)
        {
            if (_startGesturesCount++ == 0)
            {
                _targetObject = GazeManager.Instance.gazedObject;
            }
        }

        public void OnRecognitionEnded(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay)
        {
            if (--_startGesturesCount == 0)
            {
                _targetObject = null;
            }
        }
        #endregion

        #region Tap
        public void OnTapped(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int count, Ray headRay)
        {
            if (_targetObject == null || !TargetLayerEnabled())
            {
                ++_emptyPointTapCount;
                if(_doubleTapRoutine == null)
                {
                    _doubleTapRoutine = StartCoroutine(DoubleTapRoutine());
                }
                return;
            }

            if(_doubleTapRoutine != null)
            {
                StopCoroutine(_doubleTapRoutine);
                _doubleTapRoutine = null;
                _emptyPointTapCount = 0;
            }

            ITapable tapable = _targetObject.GetComponent<ITapable>();

            if (tapable != null)
            {
                tapable.OnTapped(source, count, headRay);
            }
        }
        #endregion

        #region Hold
        public void OnHoldStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay)
        {
            OnHoldHandler(GestureEventPhase.Started, source, headRay);
        }

        public void OnHoldCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay)
        {
            OnHoldHandler(GestureEventPhase.Completed, source, headRay);
        }

        public void OnHoldCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay)
        {
            OnHoldHandler(GestureEventPhase.Canceled, source, headRay);
        }

        private void OnHoldHandler(GestureEventPhase phase, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay)
        {
            if (_targetObject == null)
            {
                return;
            }

            switch (phase)
            {
                case GestureEventPhase.Started:
                    _gestureData.Souce = source;

                    _holdable = null;
                    if (TargetLayerEnabled())
                    {
                        _holdable = _targetObject.GetComponent<IHoldable>();
                    }

                    if (_holdable != null)
                    {
                        _holdable.OnHoldStarted(source, headRay);
                    }
                    break;
                case GestureEventPhase.Completed:
                    if (_holdable != null)
                    {
                        _holdable.OnHoldCompleted(source, headRay);
                        _holdable = null;
                    }
                    break;
                case GestureEventPhase.Canceled:
                    if (_holdable != null)
                    {
                        _holdable.OnHoldCanceled(source, headRay);
                        _holdable = null;
                    }
                    break;
            }
        }
        #endregion

        #region Navigation
        public void OnNavigationStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            OnNavigationHandler(GestureEventPhase.Started, source, normalizedOffset, headRay);
        }

        public void OnNavigationUpdated(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            OnNavigationHandler(GestureEventPhase.Updated, source, normalizedOffset, headRay);
        }

        public void OnNavigationCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            OnNavigationHandler(GestureEventPhase.Completed, source, normalizedOffset, headRay);
        }

        public void OnNavigationCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            OnNavigationHandler(GestureEventPhase.Canceled, source, normalizedOffset, headRay);
        }

        private void OnNavigationHandler(GestureEventPhase phase, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            if (_targetObject == null)
            {
                return;
            }

            switch (phase)
            {
                case GestureEventPhase.Started:
                    _gestureData.Souce = source;
                    _gestureData.MotionData = normalizedOffset;

                    _navigatable = null;
                    if (TargetLayerEnabled())
                    {
                        _navigatable = _targetObject.GetComponent<INavigatable>();
                    }

                    if (_navigatable != null)
                    {
                        _navigatable.OnNavigationStarted(source, normalizedOffset, headRay);
                    }
                    break;
                case GestureEventPhase.Updated:
                    _gestureData.Souce = source;
                    _gestureData.MotionData = normalizedOffset;

                    if (_navigatable != null)
                    {
                        _navigatable.OnNavigationUpdated(source, normalizedOffset, headRay);
                    }
                    break;
                case GestureEventPhase.Completed:
                    if (_navigatable != null)
                    {
                        _navigatable.OnNavigationCompleted(source, normalizedOffset, headRay);
                        _navigatable = null;
                    }
                    break;
                case GestureEventPhase.Canceled:
                    if (_navigatable != null)
                    {
                        _navigatable.OnNavigationCanceled(source, normalizedOffset, headRay);
                        _navigatable = null;
                    }
                    break;
            }
        }
        #endregion

        #region Manipulation
        public void OnManipulationStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            OnManipulationHandler(GestureEventPhase.Started, source, cumulativeDelta, headRay);
        }

        public void OnManipulationUpdated(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            OnManipulationHandler(GestureEventPhase.Updated, source, cumulativeDelta, headRay);
        }

        public void OnManipulationCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            OnManipulationHandler(GestureEventPhase.Completed, source, cumulativeDelta, headRay);
        }

        public void OnManipulationCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            OnManipulationHandler(GestureEventPhase.Canceled, source, cumulativeDelta, headRay);
        }

        private void OnManipulationHandler(GestureEventPhase phase, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            if (_targetObject == null)
            {
                return;
            }

            switch (phase)
            {
                case GestureEventPhase.Started:
                    _gestureData.Souce = source;
                    _gestureData.MotionData = cumulativeDelta;

                    _manipulatable = null;
                    if (TargetLayerEnabled())
                    {
                        _manipulatable = _targetObject.GetComponent<IManipulatable>();
                    }

                    if (_manipulatable != null)
                    {
                        _manipulatable.OnManipulationStarted(source, cumulativeDelta, headRay);
                    }
                    break;
                case GestureEventPhase.Updated:
                    _gestureData.Souce = source;
                    _gestureData.MotionData = cumulativeDelta;

                    if (_manipulatable != null)
                    {
                        _manipulatable.OnManipulationUpdated(source, cumulativeDelta, headRay);
                    }
                    break;
                case GestureEventPhase.Completed:
                    if (_manipulatable != null)
                    {
                        _manipulatable.OnManipulationCompleted(source, cumulativeDelta, headRay);
                        _manipulatable = null;
                    }
                    break;
                case GestureEventPhase.Canceled:
                    if (_manipulatable != null)
                    {
                        _manipulatable.OnManipulationCanceled(source, cumulativeDelta, headRay);
                        _manipulatable = null;
                    }
                    break;
            }
        }
        #endregion

        #region EmptyPointInteraction

        private IEnumerator DoubleTapRoutine()
        {
            yield return new WaitForSeconds(_doubleTapDuration);

            if(_emptyPointTapCount >= 2)
            {
                //if(WindowsManager.Instance.CurrentWindow != null)
                //{
                //    WindowsManager.Instance.CurrentWindow.OpenInFrontOfUser();
                //}
                //else
                //    WindowsManager.Instance.OpenMainMenuWindow();
            }

            _emptyPointTapCount = 0;
            _doubleTapRoutine = null;
        }
        #endregion
        #endregion

        #region Event handlers
        #endregion
    }
}