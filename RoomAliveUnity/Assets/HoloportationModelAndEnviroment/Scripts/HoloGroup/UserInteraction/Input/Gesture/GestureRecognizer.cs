using HoloGroup.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.WSA.Input;
using UnityGestureRecognizer = UnityEngine.XR.WSA.Input.GestureRecognizer;
using UnityInput = UnityEngine.Input;

namespace HoloGroup.UserInteraction.Input.Gesture
{
    public class GestureRecognizer : MonoBehaviour
    {
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #region Editor
#if UNITY_EDITOR
        [Serializable]
        struct EmulationData
        {
            #region Fields
            [SerializeField]
            private KeyCode _emulationKey;

            [SerializeField]
            private UnityEngine.XR.WSA.Input.InteractionSourceKind _emulationSource;

            [SerializeField]
            private float _doubleTapDelay;

            [SerializeField]
            private float _holdDelay;

            [SerializeField]
            private float _navigationZAxisMultiplier;

            [SerializeField]
            private float _navigationXYAxisMultiplier;
            #endregion

            #region Properties
            public KeyCode EmulationKey { get { return _emulationKey; } }

            public UnityEngine.XR.WSA.Input.InteractionSourceKind EmulationSource { get { return _emulationSource; } }

            public float DoubleTapDelay { get { return _doubleTapDelay; } }

            public float HoldDelay { get { return _holdDelay; } }

            public float NavigationZAxisMultiplier { get { return _navigationZAxisMultiplier; } }

            public float NavigationXYAxisMultiplier { get { return _navigationXYAxisMultiplier; } }
            #endregion
        }
#endif
        #endregion
        #endregion

        #region Classes
        [Serializable]
        public class SourceEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind> { }

        [Serializable]
        public class RecognitionStartedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Ray> { }

        [Serializable]
        public class RecognitionEndedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Ray> { }

        [Serializable]
        public class GestureErrorEvent : UnityEvent<string, int> { }

        [Serializable]
        public class TappedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, int, Ray> { }

        [Serializable]
        public class HoldStartedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Ray> { }

        [Serializable]
        public class HoldCompletedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Ray> { }

        [Serializable]
        public class HoldCanceledEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Ray> { }

        [Serializable]
        public class ManipulationStartedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class ManipulationUpdatedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class ManipulationCompletedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class ManipulationCanceledEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class NavigationStartedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class NavigationUpdatedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class NavigationCompletedEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class NavigationCanceledEvent : UnityEvent<UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }
        #endregion

        #region Fields
        #region Editor
#if UNITY_EDITOR
        [SerializeField]
        private EmulationData _editorEmulationData;

        private bool _someGestureStarted;

        private bool _isRecognitionCanceled;

        private bool _isEmulationKeyPressed;

        private Coroutine _doubleTapEmulationRoutine;

        private Coroutine _holdEmulationRoutine;

        private bool _isHolded;

        private bool _isGestureCanceledByOtherReason;

        private Vector2 _mouseStartPosition;

        private bool _isNavigated;

        private Vector3 _navigationHandGlobalOffset;

        private Vector3 _navigationXAxis;

        private Vector3 _navigationYAxis;

        private Vector3 _navigationZAxis;

        private float _handZAxisOffset;

        private bool _isNavigatedRails;

        private GestureSettings _navigationRailsDirection;

        private bool _isNavigationRailsFailed;

        private bool _isManipulated;

        private Vector3 _manipulationHandStartGlobalPosition;

        private static InteractionSourceKind? _startSource;

        private static InteractionSourceKind? _lastSource;

        private static byte _recognizersInActiveState;
#endif
        #endregion

        private UnityGestureRecognizer _gestureRecognizer;

        [SerializeField]
        private bool _startOnAwake;

        [SerializeField]
        [BitMask(typeof(GestureSettings))]
        private GestureSettings _recognizableGestures;

        private bool _isCapturingGestures;
        #endregion

        #region Events

        public SourceEvent SourceDetected = new SourceEvent();

        public SourceEvent SourceLost = new SourceEvent();

        public RecognitionStartedEvent RecognitionStarted = new RecognitionStartedEvent();

        public RecognitionEndedEvent RecognitionEnded = new RecognitionEndedEvent();

        public GestureErrorEvent GestureError = new GestureErrorEvent();

        public TappedEvent Tapped = new TappedEvent();

        public HoldStartedEvent HoldStarted = new HoldStartedEvent();

        public HoldCompletedEvent HoldCompleted = new HoldCompletedEvent();

        public HoldCanceledEvent HoldCanceled = new HoldCanceledEvent();

        public ManipulationStartedEvent ManipulationStarted = new ManipulationStartedEvent();

        public ManipulationUpdatedEvent ManipulationUpdated = new ManipulationUpdatedEvent();

        public ManipulationCompletedEvent ManipulationCompleted = new ManipulationCompletedEvent();

        public ManipulationCanceledEvent ManipulationCanceled = new ManipulationCanceledEvent();

        public NavigationStartedEvent NavigationStarted = new NavigationStartedEvent();

        public NavigationUpdatedEvent NavigationUpdated = new NavigationUpdatedEvent();

        public NavigationCompletedEvent NavigationCompleted = new NavigationCompletedEvent();

        public NavigationCanceledEvent NavigationCanceled = new NavigationCanceledEvent();
        #endregion

        #region Properties
        #endregion

        #region Methods
        #region Editor
#if UNITY_EDITOR
        private void Update()
        {
            if (!_someGestureStarted && !_isCapturingGestures)
            {
                return;
            }

            #region Canceling by call CancelGestures method
            if (_isRecognitionCanceled)
            {
                CancelHold();

                if (_isNavigated)
                {
                    Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);

                    Vector3 normalizedOffset = GetNavigationVector(handPosition, false);

                    _isNavigated = false;

                    NavigationCanceledHandler(_editorEmulationData.EmulationSource, Vector3.zero, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    //NavigationCanceled.Invoke(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                }
                else if (_isNavigatedRails)
                {
                    Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);

                    Vector3 normalizedOffset = GetNavigationVector(handPosition, true);

                    _isNavigatedRails = false;
                    _isNavigationRailsFailed = false;

                    NavigationCanceledHandler(_editorEmulationData.EmulationSource, Vector3.zero, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    //NavigationCanceled.Invoke(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                }
                else if (_isManipulated)
                {
                    Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);
                    Vector3 handPositionInCameraSpace = Camera.main.transform.InverseTransformPoint(handPosition);
                    Vector3 handPositionWithZInGlobalSpace = Camera.main.transform.TransformPoint(handPositionInCameraSpace + new Vector3(0f, 0f, _handZAxisOffset));

                    Vector3 cumulativeDelta = handPositionWithZInGlobalSpace - _manipulationHandStartGlobalPosition;

                    _isManipulated = false;

                    ManipulationCanceledHandler(_editorEmulationData.EmulationSource, Vector3.zero, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    //ManipulationCanceled.Invoke(_editorEmulationData.EmulationSource, cumulativeDelta, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                }

                _isRecognitionCanceled = false;
                _isEmulationKeyPressed = false;
                _isGestureCanceledByOtherReason = false;

                _someGestureStarted = false;

                RecognitionEndedHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                //RecognitionEnded.Invoke(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
            }
            #endregion

            if (UnityInput.GetKeyUp(_editorEmulationData.EmulationKey))
            {
                if (!_isHolded && !_isGestureCanceledByOtherReason)
                {
                    #region Tap
                    if ((_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.Tap) != 0 && _doubleTapEmulationRoutine == null && !_isNavigated && !_isNavigatedRails && !_isManipulated)
                    {
                        TappedHandler(_editorEmulationData.EmulationSource, 1, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        //Tapped.Invoke(_editorEmulationData.EmulationSource, 1, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    }
                    #endregion

                    #region DoubleTap
                    if ((_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.DoubleTap) != 0 && !_isNavigated && !_isNavigatedRails && !_isManipulated)
                    {
                        if (_doubleTapEmulationRoutine == null)
                        {
                            _doubleTapEmulationRoutine = StartCoroutine(DoubleTapEmulationRoutine());
                        }
                        else
                        {
                            StopCoroutine(_doubleTapEmulationRoutine);
                            _doubleTapEmulationRoutine = null;

                            TappedHandler(_editorEmulationData.EmulationSource, 2, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            //Tapped.Invoke(_editorEmulationData.EmulationSource, 2, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        }
                    }
                    #endregion
                }

                if (_holdEmulationRoutine != null)
                {
                    StopCoroutine(_holdEmulationRoutine);

                    _holdEmulationRoutine = null;
                }

                if (_isHolded)
                {
                    #region Hold completed
                    _isHolded = false;

                    if (!_isGestureCanceledByOtherReason && !_isNavigationRailsFailed)
                    {
                        HoldCompletedHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        //HoldCompleted.Invoke(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    }
                    #endregion
                }

                if (_isNavigated)
                {
                    #region Navigation completed
                    _isNavigated = false;

                    Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);

                    Vector3 normalizedOffset = GetNavigationVector(handPosition, false);

                    NavigationCompletedHandler(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    //NavigationCompleted.Invoke(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    #endregion
                }

                if (_isNavigatedRails)
                {
                    #region Navigation rails completed
                    _isNavigatedRails = false;

                    Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);

                    Vector3 normalizedOffset = GetNavigationVector(handPosition, true);

                    NavigationCompletedHandler(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    //NavigationCompleted.Invoke(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    #endregion
                }

                if (_isManipulated)
                {
                    #region Manipulation completed
                    _isManipulated = false;

                    Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);
                    Vector3 handPositionInCameraSpace = Camera.main.transform.InverseTransformPoint(handPosition);
                    Vector3 handPositionWithZInGlobalSpace = Camera.main.transform.TransformPoint(handPositionInCameraSpace + new Vector3(0f, 0f, _handZAxisOffset));

                    Vector3 cumulativeDelta = handPositionWithZInGlobalSpace - _manipulationHandStartGlobalPosition;

                    ManipulationCompletedHandler(_editorEmulationData.EmulationSource, cumulativeDelta, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    //ManipulationCompleted.Invoke(_editorEmulationData.EmulationSource, cumulativeDelta, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    #endregion
                }

                _isNavigationRailsFailed = false;

                _isEmulationKeyPressed = false;

                #region Recognition ended
                if (_someGestureStarted && !_isGestureCanceledByOtherReason)
                {
                    _someGestureStarted = false;

                    RecognitionEndedHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                }
                else
                {
                    RecognitionEndedHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                }

                _isGestureCanceledByOtherReason = false;
                #endregion
            }

            if (UnityInput.GetKeyDown(_editorEmulationData.EmulationKey))
            {
                #region Recognition started
                if (_recognizableGestures != 0)
                {
                    RecognitionStartedHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    //RecognitionStarted.Invoke(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));

                    _someGestureStarted = true;
                }
                #endregion

                _isEmulationKeyPressed = true;

                _mouseStartPosition = UnityInput.mousePosition;
                _handZAxisOffset = 0f;

                #region Hold started
                if ((_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.Hold) != 0)
                {
                    _holdEmulationRoutine = StartCoroutine(HoldEmulationRoutine());
                }
                #endregion
            }

            if (_isEmulationKeyPressed)
            {
                #region Hold canceled by viewport
                if (_isHolded)
                {
                    if (!IsMouseInViewport())
                    {
                        _isHolded = false;
                        _isGestureCanceledByOtherReason = true;

                        HoldCanceledHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        //HoldCanceled.Invoke(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        RecognitionEndedHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        //RecognitionEnded.Invoke(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                    }
                }
                #endregion

                #region Navigation rails
                if ((_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.NavigationRailsX) != 0 ||
                    (_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.NavigationRailsY) != 0 ||
                    (_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.NavigationRailsZ) != 0)
                {
                    if (!_isNavigationRailsFailed)
                    {
                        _handZAxisOffset += UnityInput.mouseScrollDelta.y * _editorEmulationData.NavigationZAxisMultiplier;

                        Vector2 mouseXYdelta = (Vector2)UnityInput.mousePosition - _mouseStartPosition;

                        if (!_isNavigatedRails && !_isGestureCanceledByOtherReason && (mouseXYdelta != Vector2.zero || _handZAxisOffset != 0f))
                        {
                            _navigationRailsDirection = UnityEngine.XR.WSA.Input.GestureSettings.None;

                            if (mouseXYdelta.x != 0f)
                            {
                                _navigationRailsDirection = UnityEngine.XR.WSA.Input.GestureSettings.NavigationRailsX;
                            }
                            else if (mouseXYdelta.y != 0f)
                            {
                                _navigationRailsDirection = UnityEngine.XR.WSA.Input.GestureSettings.NavigationRailsY;
                            }
                            else
                            {
                                _navigationRailsDirection = UnityEngine.XR.WSA.Input.GestureSettings.NavigationRailsZ;
                            }

                            _isNavigationRailsFailed = (_navigationRailsDirection & _recognizableGestures) == 0;

                            if (!_isNavigationRailsFailed)
                            {
                                CancelHold();

                                _isNavigatedRails = true;

                                Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);

                                _navigationHandGlobalOffset = handPosition - Camera.main.transform.position;

                                _navigationXAxis = Camera.main.transform.right;
                                _navigationYAxis = Camera.main.transform.up;
                                _navigationZAxis = Camera.main.transform.forward;

                                Vector3 normalizedOffset = GetNavigationVector(handPosition, true);

                                NavigationStartedHandler(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                                //NavigationStarted.Invoke(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            }
                        }
                    }

                    if (_isNavigatedRails)
                    {
                        #region Navigation rails updated and canceled by viewport
                        Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);

                        Vector3 normalizedOffset = GetNavigationVector(handPosition, true);

                        if (IsMouseInViewport())
                        {
                            NavigationUpdatedHandler(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            //NavigationUpdated.Invoke(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        }
                        else
                        {
                            _isNavigatedRails = false;
                            _isGestureCanceledByOtherReason = true;

                            NavigationCanceledHandler(_editorEmulationData.EmulationSource, Vector3.zero, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            //NavigationCanceled.Invoke(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            RecognitionEndedHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            //RecognitionEnded.Invoke(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        }
                        #endregion
                    }
                }
                #endregion

                #region Navigation
                if (!_isNavigatedRails)
                {
                    if ((_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.NavigationX) != 0 ||
                        (_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.NavigationY) != 0 ||
                        (_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.NavigationZ) != 0)
                    {
                        _handZAxisOffset += UnityInput.mouseScrollDelta.y * _editorEmulationData.NavigationZAxisMultiplier;

                        if (!_isNavigated && !_isGestureCanceledByOtherReason && ((Vector2)UnityInput.mousePosition != _mouseStartPosition || _handZAxisOffset != 0f))
                        {
                            CancelHold();

                            #region Navigation started
                            _isNavigated = true;

                            Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);

                            _navigationHandGlobalOffset = handPosition - Camera.main.transform.position;

                            _navigationXAxis = Camera.main.transform.right;
                            _navigationYAxis = Camera.main.transform.up;
                            _navigationZAxis = Camera.main.transform.forward;

                            Vector3 normalizedOffset = GetNavigationVector(handPosition, false);

                            NavigationStartedHandler(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            //NavigationStarted.Invoke(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            #endregion
                        }

                        if (_isNavigated)
                        {
                            #region Navigation updated and canceled by viewport
                            Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);

                            Vector3 normalizedOffset = GetNavigationVector(handPosition, false);

                            if (IsMouseInViewport())
                            {
                                NavigationUpdatedHandler(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                                //NavigationUpdated.Invoke(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            }
                            else
                            {
                                _isNavigated = false;
                                _isGestureCanceledByOtherReason = true;

                                NavigationCanceledHandler(_editorEmulationData.EmulationSource, Vector3.zero, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                                //NavigationCanceled.Invoke(_editorEmulationData.EmulationSource, normalizedOffset, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                                RecognitionEndedHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                                //RecognitionEnded.Invoke(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            }
                            #endregion
                        }
                    }
                }
                #endregion

                #region Manipulation
                if ((_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.ManipulationTranslate) != 0)
                {
                    _handZAxisOffset = Mathf.Clamp(_handZAxisOffset + UnityInput.mouseScrollDelta.y * _editorEmulationData.NavigationZAxisMultiplier, -0.3f, 0.7f);

                    if (!_isManipulated && !_isGestureCanceledByOtherReason && ((Vector2)UnityInput.mousePosition != _mouseStartPosition || _handZAxisOffset != 0f))
                    {
                        CancelHold();

                        #region Manipulation started
                        _isManipulated = true;

                        _manipulationHandStartGlobalPosition = Camera.main.ScreenPointToRay(_mouseStartPosition).GetPoint(1f);

                        Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);
                        Vector3 handPositionInCameraSpace = Camera.main.transform.InverseTransformPoint(handPosition);
                        Vector3 handPositionWithZInGlobalSpace = Camera.main.transform.TransformPoint(handPositionInCameraSpace + new Vector3(0f, 0f, _handZAxisOffset));

                        Vector3 cumulativeDelta = handPositionWithZInGlobalSpace - _manipulationHandStartGlobalPosition;

                        ManipulationStartedHandler(_editorEmulationData.EmulationSource, cumulativeDelta, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        //ManipulationStarted.Invoke(_editorEmulationData.EmulationSource, cumulativeDelta, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        #endregion
                    }

                    if (_isManipulated)
                    {
                        #region Manipulation updated and canceled by viewport
                        Vector3 handPosition = Camera.main.ScreenPointToRay(UnityInput.mousePosition).GetPoint(1f);
                        Vector3 handPositionInCameraSpace = Camera.main.transform.InverseTransformPoint(handPosition);
                        Vector3 handPositionWithZInGlobalSpace = Camera.main.transform.TransformPoint(handPositionInCameraSpace + new Vector3(0f, 0f, _handZAxisOffset));

                        Vector3 cumulativeDelta = handPositionWithZInGlobalSpace - _manipulationHandStartGlobalPosition;

                        if (IsMouseInViewport())
                        {
                            ManipulationUpdatedHandler(_editorEmulationData.EmulationSource, cumulativeDelta, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            //ManipulationUpdated.Invoke(_editorEmulationData.EmulationSource, cumulativeDelta, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        }
                        else
                        {
                            _isManipulated = false;
                            _isGestureCanceledByOtherReason = true;

                            ManipulationCanceledHandler(_editorEmulationData.EmulationSource, Vector3.zero, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            //ManipulationCanceled.Invoke(_editorEmulationData.EmulationSource, cumulativeDelta, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            RecognitionEndedHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                            //RecognitionEnded.Invoke(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                        }
                        #endregion
                    }
                }
                #endregion
            }
        }

        private bool IsMouseInViewport()
        {
            return (new Rect(0f, 0f, Screen.width, Screen.height).Contains(UnityInput.mousePosition));
        }

        private Vector3 GetNavigationVector(Vector3 handPosition, bool isRails)
        {
            Vector3 startHandPosition = Camera.main.transform.position + _navigationHandGlobalOffset;
            Quaternion startHandRotation = Quaternion.LookRotation(_navigationZAxis, _navigationYAxis);

            Vector3 normalizedOffset = Quaternion.Inverse(startHandRotation) * (handPosition - startHandPosition) * _editorEmulationData.NavigationXYAxisMultiplier;

            if (!isRails)
            {
                normalizedOffset.x = (_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.NavigationX) != 0 ? Mathf.Clamp(normalizedOffset.x, -1f, 1f) : 0f;
                normalizedOffset.y = (_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.NavigationY) != 0 ? Mathf.Clamp(normalizedOffset.y, -1f, 1f) : 0f;
                normalizedOffset.z = (_recognizableGestures & UnityEngine.XR.WSA.Input.GestureSettings.NavigationZ) != 0 ? Mathf.Clamp(_handZAxisOffset, -1f, 1f) : 0f;
            }
            else
            {
                normalizedOffset.x = (_navigationRailsDirection & UnityEngine.XR.WSA.Input.GestureSettings.NavigationRailsX) != 0 ? Mathf.Clamp(normalizedOffset.x, -1f, 1f) : 0f;
                normalizedOffset.y = (_navigationRailsDirection & UnityEngine.XR.WSA.Input.GestureSettings.NavigationRailsY) != 0 ? Mathf.Clamp(normalizedOffset.y, -1f, 1f) : 0f;
                normalizedOffset.z = (_navigationRailsDirection & UnityEngine.XR.WSA.Input.GestureSettings.NavigationRailsZ) != 0 ? Mathf.Clamp(_handZAxisOffset, -1f, 1f) : 0f;
            }

            return normalizedOffset;
        }

        private void CancelHold()
        {
            if (_holdEmulationRoutine != null)
            {
                StopCoroutine(_holdEmulationRoutine);

                _holdEmulationRoutine = null;
            }

            #region Hold canceled by navigation
            if (_isHolded)
            {
                _isHolded = false;
                _isGestureCanceledByOtherReason = true;

                HoldCanceledHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
                //HoldCanceled.Invoke(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
            }
            #endregion
        }

        private IEnumerator DoubleTapEmulationRoutine()
        {
            yield return new WaitForSeconds(_editorEmulationData.DoubleTapDelay);

            _doubleTapEmulationRoutine = null;
        }

        private IEnumerator HoldEmulationRoutine()
        {
            yield return new WaitForSeconds(_editorEmulationData.HoldDelay);

            _isHolded = true;

            HoldStartedHandler(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
            //HoldStarted.Invoke(_editorEmulationData.EmulationSource, new Ray(Camera.main.transform.position, Camera.main.transform.forward));
        }

        private void OnValidate()
        {
            if (_gestureRecognizer != null)
            {
                SetRecognizableGestures(_recognizableGestures);
            }
        }

        #region Gestures handlers
        #region Recognition
        private void RecognitionStartedHandler(InteractionSourceKind source, Ray headRay)
        {
            RecognitionStart(source);

            RecognitionHandler(GestureEventPhase.Started, source, headRay);
        }

        private void RecognitionEndedHandler(InteractionSourceKind source, Ray headRay)
        {
            RecognitionHandler(GestureEventPhase.Completed, source, headRay);

            RecognitionEnd();
        }

        private void RecognitionStart(InteractionSourceKind source)
        {
            if (_recognizersInActiveState == 0)
            {
                _startSource = source;
                _lastSource = source;
            }
            else
            {
                _lastSource = source;
            }

            _recognizersInActiveState += 1;
        }

        private void RecognitionEnd()
        {
            _recognizersInActiveState -= 1;

            if (_recognizersInActiveState == 0)
            {
                _startSource = null;
                _lastSource = null;
            }
        }

        private void RecognitionHandler(GestureEventPhase phase, InteractionSourceKind source, Ray headRay)
        {
            if (_startSource.HasValue && _startSource != source)
            {
                return;
            }

            InteractionSourceKind lastSource = _lastSource.Value;

            switch (phase)
            {
                case GestureEventPhase.Started:
                    RecognitionStarted.Invoke(lastSource, headRay);
                    break;
                case GestureEventPhase.Completed:
                    RecognitionEnded.Invoke(lastSource, headRay);
                    break;
            }
        }
        #endregion

        #region Tapped
        private void TappedHandler(InteractionSourceKind source, int count, Ray headRay)
        {
            if (_startSource.HasValue && _startSource != source)
            {
                return;
            }

            InteractionSourceKind lastSource = _lastSource.Value;

            Tapped.Invoke(lastSource, count, headRay);
        }
        #endregion

        #region Hold
        private void HoldStartedHandler(InteractionSourceKind source, Ray headRay)
        {
            HoldHandler(GestureEventPhase.Started, source, headRay);
        }

        private void HoldCompletedHandler(InteractionSourceKind source, Ray headRay)
        {
            HoldHandler(GestureEventPhase.Completed, source, headRay);
        }

        private void HoldCanceledHandler(InteractionSourceKind source, Ray headRay)
        {
            HoldHandler(GestureEventPhase.Canceled, source, headRay);
        }

        private void HoldHandler(GestureEventPhase phase, InteractionSourceKind source, Ray headRay)
        {
            if (_startSource.HasValue && _startSource != source)
            {
                return;
            }

            InteractionSourceKind lastSource = _lastSource.Value;

            switch (phase)
            {
                case GestureEventPhase.Started:
                    HoldStarted.Invoke(lastSource, headRay);
                    break;
                case GestureEventPhase.Completed:
                    HoldCompleted.Invoke(lastSource, headRay);
                    break;
                case GestureEventPhase.Canceled:
                    HoldCanceled.Invoke(lastSource, headRay);
                    break;
            }
        }
        #endregion

        #region Manipulation
        private void ManipulationStartedHandler(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationHandler(GestureEventPhase.Started, source, cumulativeDelta, headRay);
        }

        private void ManipulationUpdatedHandler(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationHandler(GestureEventPhase.Updated, source, cumulativeDelta, headRay);
        }

        private void ManipulationCompletedHandler(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationHandler(GestureEventPhase.Completed, source, cumulativeDelta, headRay);
        }

        private void ManipulationCanceledHandler(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationHandler(GestureEventPhase.Canceled, source, cumulativeDelta, headRay);
        }

        private void ManipulationHandler(GestureEventPhase phase, InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            if (_startSource.HasValue && _startSource != source)
            {
                return;
            }

            InteractionSourceKind lastSource = _lastSource.Value;

            switch (phase)
            {
                case GestureEventPhase.Started:
                    ManipulationStarted.Invoke(lastSource, cumulativeDelta, headRay);
                    break;
                case GestureEventPhase.Updated:
                    ManipulationUpdated.Invoke(lastSource, cumulativeDelta, headRay);
                    break;
                case GestureEventPhase.Completed:
                    ManipulationCompleted.Invoke(lastSource, cumulativeDelta, headRay);
                    break;
                case GestureEventPhase.Canceled:
                    ManipulationCanceled.Invoke(lastSource, cumulativeDelta, headRay);
                    break;
            }
        }
        #endregion

        #region navigation
        private void NavigationStartedHandler(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationHandler(GestureEventPhase.Started, source, normalizedOffset, headRay);
        }

        private void NavigationUpdatedHandler(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationHandler(GestureEventPhase.Updated, source, normalizedOffset, headRay);
        }

        private void NavigationCompletedHandler(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationHandler(GestureEventPhase.Completed, source, normalizedOffset, headRay);
        }

        private void NavigationCanceledHandler(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationHandler(GestureEventPhase.Canceled, source, normalizedOffset, headRay);
        }

        private void NavigationHandler(GestureEventPhase phase, InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            if (_startSource.HasValue && _startSource != source)
            {
                return;
            }

            InteractionSourceKind lastSource = _lastSource.Value;

            switch (phase)
            {
                case GestureEventPhase.Started:
                    NavigationStarted.Invoke(lastSource, normalizedOffset, headRay);
                    break;
                case GestureEventPhase.Updated:
                    NavigationUpdated.Invoke(lastSource, normalizedOffset, headRay);
                    break;
                case GestureEventPhase.Completed:
                    NavigationCompleted.Invoke(lastSource, normalizedOffset, headRay);
                    break;
                case GestureEventPhase.Canceled:
                    NavigationCanceled.Invoke(lastSource, normalizedOffset, headRay);
                    break;
            }
        }
        #endregion
        #endregion
#endif
        #endregion

        private void Awake()
        {
            RecreateGestureRecognizer();

            SetRecognizableGestures(_recognizableGestures);

            if (_startOnAwake)
            {
                StartCapturingGestures();
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            if (_gestureRecognizer == null)
            {
                return;
            }

            StopCapturingGestures();

            _gestureRecognizer.Dispose();

            _gestureRecognizer = null;

            InteractionManager.InteractionSourceDetected -= InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceLost -= InteractionManager_InteractionSourceLost;
        }

        public void RecreateGestureRecognizer()
        {
            Dispose();

            _gestureRecognizer = new UnityGestureRecognizer();

            #region Subscribe on events
            InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;

            _gestureRecognizer.RecognitionStartedEvent += GestureRecognizer_RecognitionStarted;
            _gestureRecognizer.RecognitionEndedEvent += GestureRecognizer_RecognitionEnded;

            _gestureRecognizer.GestureErrorEvent += GestureRecognizer_GestureError;

            _gestureRecognizer.TappedEvent += GestureRecognizer_Tapped;

            _gestureRecognizer.HoldStartedEvent += GestureRecognizer_HoldStarted;
            _gestureRecognizer.HoldCompletedEvent += GestureRecognizer_HoldCompleted;
            _gestureRecognizer.HoldCanceledEvent += GestureRecognizer_HoldCanceled;

            _gestureRecognizer.ManipulationStartedEvent += GestureRecognizer_ManipulationStarted;
            _gestureRecognizer.ManipulationUpdatedEvent += GestureRecognizer_ManipulationUpdated;
            _gestureRecognizer.ManipulationCompletedEvent += GestureRecognizer_ManipulationCompleted;
            _gestureRecognizer.ManipulationCanceledEvent += GestureRecognizer_ManipulationCanceled;

            _gestureRecognizer.NavigationStartedEvent += GestureRecognizer_NavigationStarted;
            _gestureRecognizer.NavigationUpdatedEvent += GestureRecognizer_NavigationUpdated;
            _gestureRecognizer.NavigationCompletedEvent += GestureRecognizer_NavigationCompleted;
            _gestureRecognizer.NavigationCanceledEvent += GestureRecognizer_NavigationCanceled;

            #endregion
        }

        public void StartCapturingGestures()
        {
            if (!_isCapturingGestures)
            {
                _gestureRecognizer.StartCapturingGestures();

                _isCapturingGestures = true;
#if UNITY_EDITOR
                _isRecognitionCanceled = false;
#endif
            }
        }

        public void CancelGestures()
        {
            if (_isCapturingGestures)
            {
                _gestureRecognizer.CancelGestures();

                _isCapturingGestures = false;

#if UNITY_EDITOR
                _isRecognitionCanceled = true;
#endif
            }
        }

        public void StopCapturingGestures()
        {
            if (_isCapturingGestures)
            {
                if (_gestureRecognizer != null)
                {
                    _gestureRecognizer.StopCapturingGestures();
                }

                _isCapturingGestures = false;
            }
        }

        public void SetRecognizableGestures(GestureSettings gestureSettings)
        {
            GestureSettings navigationGestures =
                GestureSettings.NavigationX | GestureSettings.NavigationY |
                GestureSettings.NavigationZ | GestureSettings.NavigationRailsX |
                GestureSettings.NavigationRailsY | GestureSettings.NavigationRailsZ;

            if ((gestureSettings & GestureSettings.ManipulationTranslate) != 0 && (gestureSettings & navigationGestures) != 0)
            {
                return;
            }

            if (_gestureRecognizer == null)
            {
                return;
            }

            _recognizableGestures = gestureSettings;
            _gestureRecognizer.SetRecognizableGestures(_recognizableGestures);
        }

        public GestureSettings GetRecognizableGestures()
        {
            return _recognizableGestures;
        }

        public bool IsCapturingGestures()
        {
            return _isCapturingGestures;
        }

        public void IsCapturingGestures(bool state)
        {
            if (state)
            {
                StartCapturingGestures();
            }
            else
            {
                StopCapturingGestures();
            }
        }
        #endregion

        #region Event handlers

        private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs obj)
        {
            SourceDetected.Invoke(obj.state.source.kind);
        }

        private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs obj)
        {
            SourceLost.Invoke(obj.state.source.kind);
        }

        private void GestureRecognizer_RecognitionStarted(InteractionSourceKind source, Ray headRay)
        {
            RecognitionStarted.Invoke(source, headRay);
        }

        private void GestureRecognizer_RecognitionEnded(InteractionSourceKind source, Ray headRay)
        {
            RecognitionEnded.Invoke(source, headRay);
        }

        private void GestureRecognizer_GestureError(string error, int hResult)
        {
            GestureError.Invoke(error, hResult);
        }

        private void GestureRecognizer_Tapped(InteractionSourceKind source, int count, Ray headRay)
        {
            Tapped.Invoke(source, count, headRay);
        }

        private void GestureRecognizer_HoldStarted(InteractionSourceKind source, Ray headRay)
        {
            HoldStarted.Invoke(source, headRay);
        }

        private void GestureRecognizer_HoldCompleted(InteractionSourceKind source, Ray headRay)
        {
            HoldCompleted.Invoke(source, headRay);
        }

        private void GestureRecognizer_HoldCanceled(InteractionSourceKind source, Ray headRay)
        {
            HoldCanceled.Invoke(source, headRay);
        }

        private void GestureRecognizer_ManipulationStarted(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationStarted.Invoke(source, cumulativeDelta, headRay);
        }

        private void GestureRecognizer_ManipulationUpdated(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationUpdated.Invoke(source, cumulativeDelta, headRay);
        }

        private void GestureRecognizer_ManipulationCompleted(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationCompleted.Invoke(source, cumulativeDelta, headRay);
        }

        private void GestureRecognizer_ManipulationCanceled(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationCanceled.Invoke(source, cumulativeDelta, headRay);
        }

        private void GestureRecognizer_NavigationStarted(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationStarted.Invoke(source, normalizedOffset, headRay);
        }

        private void GestureRecognizer_NavigationUpdated(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationUpdated.Invoke(source, normalizedOffset, headRay);
        }

        private void GestureRecognizer_NavigationCompleted(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationCompleted.Invoke(source, normalizedOffset, headRay);
        }

        private void GestureRecognizer_NavigationCanceled(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationCanceled.Invoke(source, normalizedOffset, headRay);
        }
        #endregion
    }
}