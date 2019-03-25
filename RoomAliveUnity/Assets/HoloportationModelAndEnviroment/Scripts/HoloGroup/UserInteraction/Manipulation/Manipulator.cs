using HoloGroup.Geometry;
using HoloGroup.Geometry.Extensions;
using HoloGroup.UserInteraction.Input.Gesture;
using HoloGroup.UserInteraction.Manipulation.Gizmos;
using HoloGroup.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Events;

namespace HoloGroup.UserInteraction.Manipulation
{
    public class Manipulator : MonoBehaviour
    {
        #region Entities
        #region Enums
        private enum GestureManipulationType
        {
            None,
            Navigation,
            Manipulation
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #region Events
        [Serializable]
        public class StartedEvent : UnityEvent<Manipulator> { }

        [Serializable]
        public class StopedEvent : UnityEvent<Manipulator> { }
        #endregion

        private class ManipulationMover
        {
            private Manipulator _manipulator;

            private Vector3 _startPosition;

            public ManipulationMover(Manipulator manipulator)
            {
                _manipulator = manipulator;
            }

            #region Move by manipulation
            public void CompleteManipulating(Vector3 cumulativeDelta)
            {
                _manipulator._rotatableBounds.Center = _startPosition + (cumulativeDelta * _manipulator._positionChangeMultiplier);
            }

            public void StartManipulating(Vector3 cumulativeDelta)
            {
                _startPosition = _manipulator._rotatableBounds.Center;

                _manipulator._rotatableBounds.Center = _startPosition + (cumulativeDelta * _manipulator._positionChangeMultiplier);
            }

            public void UpdateManipulating(Vector3 cumulativeDelta)
            {
                _manipulator._rotatableBounds.Center = _startPosition + (cumulativeDelta * _manipulator._positionChangeMultiplier);
            }
            #endregion

            #region Move by Navigation
            public void CompleteNavigating(Vector3 normalizedOffset)
            {
                CalculatePosition(normalizedOffset);
            }

            public void StartNavigating(Vector3 normalizedOffset)
            {
                _startPosition = _manipulator._rotatableBounds.Center;

                CalculatePosition(normalizedOffset);
            }

            public void UpdateNavigating(Vector3 normalizedOffset)
            {
                CalculatePosition(normalizedOffset);
            }

            private void CalculatePosition(Vector3 normalizedOffset)
            {
                Vector3 fromToVector = _startPosition - Camera.main.transform.position;
                Quaternion fromToRotation = Quaternion.LookRotation(fromToVector);

                Quaternion clickerRotation = Quaternion.Euler(-normalizedOffset.y * 45f, normalizedOffset.x * 45f, 0f);

                Quaternion resultRotation = fromToRotation * clickerRotation;

                Vector3 resultVector = (resultRotation * Vector3.forward) * fromToVector.magnitude;

                _manipulator._rotatableBounds.Center = Camera.main.transform.position + resultVector;
            }
            #endregion
        }

        private class ManipulationScaler
        {
            private Manipulator _manipulator;

            private Vector3 _startSize;

            private Vector3 _scaleAxis;

            public ManipulationScaler(Manipulator handManipulator)
            {
                _manipulator = handManipulator;
            }

            #region Maniulation and navigation
            private void SetNewSize(float offsetValue)
            {
                float maxSizeSide = _startSize.Max();

                if (maxSizeSide + offsetValue < _manipulator._minSize)
                {
                    offsetValue = -(Mathf.Abs(maxSizeSide - _manipulator._minSize));
                }

                Vector3 offsetVector = _startSize / maxSizeSide;
                offsetVector *= offsetValue;

                _manipulator._rotatableBounds.Size = _startSize + offsetVector;
            }

            #region Scale by manipulation
            public void CompleteManipulating(Vector3 cumulativeDelta)
            {
                CalculateSizeByManipulation(cumulativeDelta);
            }

            public void StartManipulating(Vector3 cumulativeDelta)
            {
                _startSize = _manipulator._rotatableBounds.Size;
                _scaleAxis = (_manipulator._handledGizmo.transform.position - _manipulator.transform.position).normalized;

                CalculateSizeByManipulation(cumulativeDelta);
            }

            public void UpdateManipulating(Vector3 cumulativeDelta)
            {
                CalculateSizeByManipulation(cumulativeDelta);
            }

            private void CalculateSizeByManipulation(Vector3 cumulativeDelta)
            {
                Vector3 multipliedCumulativeData = cumulativeDelta * _manipulator._scaleChangeMultiplier;

                float offsetValue = Vector3.Dot(multipliedCumulativeData, _scaleAxis);

                SetNewSize(offsetValue);
            }
            #endregion

            #region Scale by navigation
            public void CompleteNavigating(Vector3 normalizedOffset)
            {
                CalculateSizeByNavigation(normalizedOffset);
            }

            public void StartNavigating(Vector3 normalizedOffset)
            {
                _startSize = _manipulator._rotatableBounds.Size;
                _scaleAxis = Camera.main.transform.InverseTransformDirection(_manipulator._handledGizmo.transform.position - _manipulator.transform.position).normalized;

                CalculateSizeByNavigation(normalizedOffset);
            }

            public void UpdateNavigating(Vector3 normalizedOffset)
            {
                CalculateSizeByNavigation(normalizedOffset);
            }

            public void CalculateSizeByNavigation(Vector3 normalizedOffset)
            {
                float offsetValue = Vector3.Dot(normalizedOffset * _manipulator._scaleChangeMultiplier, _scaleAxis);

                SetNewSize(offsetValue);
            }
            #endregion
            #endregion
        }

        private class ManipulationRotator
        {
            private Manipulator _manipulator;

            private Quaternion _startRotation;

            private Vector3 _rotateAxis;

            public ManipulationRotator(Manipulator handManipulator)
            {
                _manipulator = handManipulator;
            }

            #region Rotation by manipulation
            public void CompleteManipulating(Vector3 cumulativeDelta)
            {
                CalculateRotationByManipulation(cumulativeDelta);
            }

            public void StartManipulating(Vector3 cumulativeDelta)
            {
                _startRotation = _manipulator._rotatableBounds.Rotation;
                _rotateAxis = Camera.main.transform.right;

                CalculateRotationByManipulation(cumulativeDelta);
            }

            public void UpdateManipulating(Vector3 cumulativeDelta)
            {
                CalculateRotationByManipulation(cumulativeDelta);
            }

            private void CalculateRotationByManipulation(Vector3 cumulativeDelta)
            {
                Vector3 multipliedCumulativeData = cumulativeDelta * _manipulator._rotationChangeMultiplier;

                float offsetValue = -Vector3.Dot(multipliedCumulativeData, _rotateAxis);

                Quaternion offsetRotation = Quaternion.AngleAxis(offsetValue, Vector3.up);
                _manipulator._rotatableBounds.Rotation = _startRotation * offsetRotation;
            }
            #endregion

            #region Rotation by navigation
            public void CompleteNavigating(Vector3 normalizedOffset)
            {
                CalculateRotationByNavigation(normalizedOffset);
            }

            public void StartNavigating(Vector3 normalizedOffset)
            {
                _startRotation = _manipulator._rotatableBounds.Rotation;

                CalculateRotationByNavigation(normalizedOffset);
            }

            public void UpdateNavigating(Vector3 normalizedOffset)
            {
                CalculateRotationByNavigation(normalizedOffset);
            }

            public void CalculateRotationByNavigation(Vector3 normalizedOffset)
            {
                float offsetValue = -(normalizedOffset * _manipulator._rotationChangeMultiplier).x;

                Quaternion offsetRotation = Quaternion.AngleAxis(offsetValue, Vector3.up);
                _manipulator._rotatableBounds.Rotation = _startRotation * offsetRotation;
            }
            #endregion
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #region Geometry
        [SerializeField]
        private GameObject _bounds;

        [SerializeField]
        private Transform _faces;

        #region Corners
        [Header("Corners")]
        [SerializeField]
        private Corner _rightTopFrontCorner;

        [SerializeField]
        private Corner _rightTopBackCorner;

        [SerializeField]
        private Corner _rightBottomFrontCorner;

        [SerializeField]
        private Corner _rightBottomBackCorner;

        [SerializeField]
        private Corner _leftTopFrontCorner;

        [SerializeField]
        private Corner _leftTopBackCorner;

        [SerializeField]
        private Corner _leftBottomFrontCorner;

        [SerializeField]
        private Corner _leftBottomBackCorner;
        #endregion

        #region Edges
        [Header("Edges")]
        [SerializeField]
        private Transform _topFrontEdge;

        [SerializeField]
        private Transform _topBackEdge;

        [SerializeField]
        private Transform _bottomFrontEdge;

        [SerializeField]
        private Transform _bottomBackEdge;

        [SerializeField]
        private Transform _rightFrontEdge;

        [SerializeField]
        private Transform _rightBackEdge;

        [SerializeField]
        private Transform _leftFrontEdge;

        [SerializeField]
        private Transform _leftBackEdge;

        [SerializeField]
        private Transform _rightTopEdge;

        [SerializeField]
        private Transform _rightBottomEdge;

        [SerializeField]
        private Transform _leftTopEdge;

        [SerializeField]
        private Transform _leftBottomEdge;
        #endregion

        #region Rotation points
        [Header("Rotation Points")]
        [SerializeField]
        private RotationPoint _frontRightRotationPoint;

        [SerializeField]
        private RotationPoint _backRightRotationPoint;

        [SerializeField]
        private RotationPoint _frontLeftRotationPoint;

        [SerializeField]
        private RotationPoint _backLeftRotationPoint;
        #endregion
        #endregion

        #region Parameters
        [Header("Parameters")]
        [SerializeField]
        [Range(0.05f, 1f)]
        private float _cornerSize = 0.1f;

        [SerializeField]
        [Range(0.01f, 0.1f)]
        private float _cornerOverallSize = 0.02f;

        [SerializeField]
        [Range(0.005f, 0.05f)]
        private float _edgeSize = 0.02f;

        [SerializeField]
        [Range(0.05f, 1f)]
        private float _rotationPointSize = 0.1f;

        private float _gizmosSizeScaler = 1f;

        [SerializeField]
        [Range(1f, 16f)]
        private float _sizeScalerDevider = 1f;

        private Coroutine _startManipulateRoutine;
        #endregion

        #region Manipulation and navigation
        [Header("Manipulations")]
        private Transform _target;

        private bool _isBusy;

        [SerializeField]
        private RotatableBounds _rotatableBounds;

        private Vector3 _manipulatorStartSize;

        private Vector3 _targetStartPositionOffset;

        private Vector3 _targetStartScale;

        [SerializeField]
        private float _positionChangeMultiplier = 1f;

        [SerializeField]
        private float _scaleChangeMultiplier = 1f;

        [SerializeField]
        private float _rotationChangeMultiplier = 1f;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float _minSize = 0.1f;

        private ManipulationMover _manipulationMover;
        private ManipulationRotator _manipulationRotator;
        private ManipulationScaler _manipulationScaler;

        private ManipulatorGizmos _handledGizmo;

        private GestureManipulationType _currentGestureManipulationType;

        private UnityEngine.XR.WSA.Input.InteractionSourceKind? _startSource;

        private bool _hasSourceChanged;
        #endregion
        #endregion

        #region Events
        public StartedEvent Started = new StartedEvent();

        public StopedEvent Stoped = new StopedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public bool IsStarted { get { return _isBusy; } }

        public Vector3 Extents { get { return _faces.localScale / 2f; } }

        public Vector3 Size { get { return _faces.localScale; } }

        public Transform Target { get { return _target; } }
        #endregion

        #region Methods
        private void Awake()
        {
            _manipulationMover = new ManipulationMover(this);
            _manipulationRotator = new ManipulationRotator(this);
            _manipulationScaler = new ManipulationScaler(this);
        }

        public void StartManipulate(GameObject target, bool asCubeBounds = false)
        {
            StartManipulate(target.transform, asCubeBounds);
        }

        public void StartManipulate(Transform target, bool asCubeBounds = false)
        {
            if (_isBusy)
            {
                StopManipulate();
            }

            _isBusy = true;

            _target = target;
            _startManipulateRoutine = StartCoroutine(StartManipulateRoutine(asCubeBounds));

            Started.Invoke(this);
        }

        private IEnumerator StartManipulateRoutine(bool asCubeBounds = false)
        {
            _bounds.SetActive(true);

            _rotatableBounds = BoundsUtility.GetLocalBound(_target.gameObject, BoundsUtility.BoundsCreateOption.Mesh);

            if (asCubeBounds)
            {
                float rotatableBoundsMaxSideSize = _rotatableBounds.Size.Max();
                _rotatableBounds.Size = new Vector3(rotatableBoundsMaxSideSize, rotatableBoundsMaxSideSize, rotatableBoundsMaxSideSize);
            }

            _manipulatorStartSize = _rotatableBounds.Size;

            _targetStartPositionOffset = _rotatableBounds.InverseTransformPoint(_target.position);
            _targetStartScale = _target.localScale;

            while (true)
            {
                // This breaks manipulating for cases when furniture object was destroyed
                if (!_isBusy)
                {
                    yield break;
                }

                #region Calculate gizmos
                CalculateBounds();
                #endregion

                #region Manipulate with target
                Vector3 sizeMultiplicity = _rotatableBounds.Size.Devide(_manipulatorStartSize);

                _target.position = _rotatableBounds.TransformPoint(Vector3.Scale(_targetStartPositionOffset, sizeMultiplicity));
                _target.rotation = _rotatableBounds.Rotation;
                _target.localScale = Vector3.Scale(_targetStartScale, sizeMultiplicity);
                #endregion

                yield return null;
            }
        }

        private void CalculateBounds()
        {
            CalculateGizmosSizeScaler();

            CalculateFacesTransform();
            CalculateCornersAndRotationPointsSize();
            CalculateEdgesPositionsAndScales();
            CalculateCornersPositions();
            CalculateRotationPointsPosition();
        }

        private void CalculateFacesTransform()
        {
            transform.position = _rotatableBounds.Center;
            transform.rotation = _rotatableBounds.Rotation;

            _faces.localScale = _rotatableBounds.Size + new Vector3(_cornerSize * 2, _cornerSize * 2, _cornerSize * 2);
        }

        private void CalculateCornersPositions()
        {
            float halfScaledCornerSize = (_cornerSize * _gizmosSizeScaler) / 2f;
            float halfCornerOverallSize = (_cornerOverallSize * _gizmosSizeScaler) / 2f;

            _rightTopFrontCorner.transform.position = _rotatableBounds.RightTopFront;
            _rightTopFrontCorner.transform.localPosition += new Vector3(_cornerSize, _cornerSize, _cornerSize);
            _rightTopFrontCorner.transform.localPosition -= new Vector3(halfScaledCornerSize, halfScaledCornerSize, halfScaledCornerSize);
            _rightTopFrontCorner.transform.localPosition += new Vector3(halfCornerOverallSize, halfCornerOverallSize, halfCornerOverallSize);

            _rightTopBackCorner.transform.position = _rotatableBounds.RightTopBack;
            _rightTopBackCorner.transform.localPosition += new Vector3(_cornerSize, _cornerSize, -_cornerSize);
            _rightTopBackCorner.transform.localPosition -= new Vector3(halfScaledCornerSize, halfScaledCornerSize, -halfScaledCornerSize);
            _rightTopBackCorner.transform.localPosition += new Vector3(halfCornerOverallSize, halfCornerOverallSize, -halfCornerOverallSize);

            _rightBottomFrontCorner.transform.position = _rotatableBounds.RightBottomFront;
            _rightBottomFrontCorner.transform.localPosition += new Vector3(_cornerSize, -_cornerSize, _cornerSize);
            _rightBottomFrontCorner.transform.localPosition -= new Vector3(halfScaledCornerSize, -halfScaledCornerSize, halfScaledCornerSize);
            _rightBottomFrontCorner.transform.localPosition += new Vector3(halfCornerOverallSize, -halfCornerOverallSize, halfCornerOverallSize);

            _rightBottomBackCorner.transform.position = _rotatableBounds.RightBottomBack;
            _rightBottomBackCorner.transform.localPosition += new Vector3(_cornerSize, -_cornerSize, -_cornerSize);
            _rightBottomBackCorner.transform.localPosition += new Vector3(-halfScaledCornerSize, halfScaledCornerSize, halfScaledCornerSize);
            _rightBottomBackCorner.transform.localPosition += new Vector3(halfCornerOverallSize, -halfCornerOverallSize, -halfCornerOverallSize);

            _leftTopFrontCorner.transform.position = _rotatableBounds.LeftTopFront;
            _leftTopFrontCorner.transform.localPosition += new Vector3(-_cornerSize, _cornerSize, _cornerSize);
            _leftTopFrontCorner.transform.localPosition -= new Vector3(-halfScaledCornerSize, halfScaledCornerSize, halfScaledCornerSize);
            _leftTopFrontCorner.transform.localPosition += new Vector3(-halfCornerOverallSize, halfCornerOverallSize, halfCornerOverallSize);

            _leftTopBackCorner.transform.position = _rotatableBounds.LeftTopBack;
            _leftTopBackCorner.transform.localPosition += new Vector3(-_cornerSize, _cornerSize, -_cornerSize);
            _leftTopBackCorner.transform.localPosition += new Vector3(halfScaledCornerSize, -halfScaledCornerSize, halfScaledCornerSize);
            _leftTopBackCorner.transform.localPosition += new Vector3(-halfCornerOverallSize, halfCornerOverallSize, -halfCornerOverallSize);

            _leftBottomFrontCorner.transform.position = _rotatableBounds.LeftBottomFront;
            _leftBottomFrontCorner.transform.localPosition += new Vector3(-_cornerSize, -_cornerSize, _cornerSize);
            _leftBottomFrontCorner.transform.localPosition += new Vector3(halfScaledCornerSize, halfScaledCornerSize, -halfScaledCornerSize);
            _leftBottomFrontCorner.transform.localPosition += new Vector3(-halfCornerOverallSize, -halfCornerOverallSize, halfCornerOverallSize);

            _leftBottomBackCorner.transform.position = _rotatableBounds.LeftBottomBack;
            _leftBottomBackCorner.transform.localPosition += new Vector3(-_cornerSize, -_cornerSize, -_cornerSize);
            _leftBottomBackCorner.transform.localPosition += new Vector3(halfScaledCornerSize, halfScaledCornerSize, halfScaledCornerSize);
            _leftBottomBackCorner.transform.localPosition += new Vector3(-halfCornerOverallSize, -halfCornerOverallSize, -halfCornerOverallSize);
        }

        private void CalculateEdgesPositionsAndScales()
        {
            float edgeHalfSize = (_edgeSize * _gizmosSizeScaler) / 2f;

            #region X axis
            _topFrontEdge.localPosition = new Vector3(0f, _rotatableBounds.Extents.y - edgeHalfSize, _rotatableBounds.Extents.z - edgeHalfSize) + new Vector3(0f, _cornerSize, _cornerSize);
            _topBackEdge.localPosition = new Vector3(0f, _rotatableBounds.Extents.y - edgeHalfSize, -_rotatableBounds.Extents.z + edgeHalfSize) + new Vector3(0f, _cornerSize, -_cornerSize);
            _bottomFrontEdge.localPosition = new Vector3(0f, -_rotatableBounds.Extents.y + edgeHalfSize, _rotatableBounds.Extents.z - edgeHalfSize) + new Vector3(0f, -_cornerSize, _cornerSize);
            _bottomBackEdge.localPosition = new Vector3(0f, -_rotatableBounds.Extents.y + edgeHalfSize, -_rotatableBounds.Extents.z + edgeHalfSize) + new Vector3(0f, -_cornerSize, -_cornerSize);

            SetAxisEdgesLocalScale(_topFrontEdge, _topBackEdge, _bottomFrontEdge, _bottomBackEdge, new Vector3(_rotatableBounds.Size.x + _cornerSize * 2f, _edgeSize * _gizmosSizeScaler, _edgeSize * _gizmosSizeScaler));
            #endregion

            #region Y axis
            _rightFrontEdge.localPosition = new Vector3(_rotatableBounds.Extents.x - edgeHalfSize, 0f, _rotatableBounds.Extents.z - edgeHalfSize) + new Vector3(_cornerSize, 0f, _cornerSize);
            _rightBackEdge.localPosition = new Vector3(_rotatableBounds.Extents.x - edgeHalfSize, 0f, -_rotatableBounds.Extents.z + edgeHalfSize) + new Vector3(_cornerSize, 0f, -_cornerSize);
            _leftFrontEdge.localPosition = new Vector3(-_rotatableBounds.Extents.x + edgeHalfSize, 0f, _rotatableBounds.Extents.z - edgeHalfSize) + new Vector3(-_cornerSize, 0f, _cornerSize);
            _leftBackEdge.localPosition = new Vector3(-_rotatableBounds.Extents.x + edgeHalfSize, 0f, -_rotatableBounds.Extents.z + edgeHalfSize) + new Vector3(-_cornerSize, 0f, -_cornerSize);

            SetAxisEdgesLocalScale(_rightFrontEdge, _rightBackEdge, _leftFrontEdge, _leftBackEdge, new Vector3(_edgeSize * _gizmosSizeScaler, _rotatableBounds.Size.y + _cornerSize * 2f, _edgeSize * _gizmosSizeScaler));
            #endregion

            #region Z axis
            _rightTopEdge.localPosition = new Vector3(_rotatableBounds.Extents.x - edgeHalfSize, _rotatableBounds.Extents.y - edgeHalfSize, 0f) + new Vector3(_cornerSize, _cornerSize, 0f);
            _rightBottomEdge.localPosition = new Vector3(_rotatableBounds.Extents.x - edgeHalfSize, -_rotatableBounds.Extents.y + edgeHalfSize, 0f) + new Vector3(_cornerSize, -_cornerSize, 0f);
            _leftTopEdge.localPosition = new Vector3(-_rotatableBounds.Extents.x + edgeHalfSize, _rotatableBounds.Extents.y - edgeHalfSize, 0f) + new Vector3(-_cornerSize, _cornerSize, 0f);
            _leftBottomEdge.localPosition = new Vector3(-_rotatableBounds.Extents.x + edgeHalfSize, -_rotatableBounds.Extents.y + edgeHalfSize, 0f) + new Vector3(-_cornerSize, -_cornerSize, 0f);

            SetAxisEdgesLocalScale(_rightTopEdge, _rightBottomEdge, _leftTopEdge, _leftBottomEdge, new Vector3(_edgeSize * _gizmosSizeScaler, _edgeSize * _gizmosSizeScaler, _rotatableBounds.Size.z + _cornerSize * 2f));
            #endregion
        }

        private void CalculateRotationPointsPosition()
        {
            Vector3 frontRightLocalPosition = new Vector3(_rotatableBounds.Extents.x + _cornerSize, 0f, _rotatableBounds.Extents.z + _cornerSize);
            Vector3 backRightLocalPosition = new Vector3(_rotatableBounds.Extents.x + _cornerSize, 0f, -_rotatableBounds.Extents.z - _cornerSize);
            Vector3 frontLeftLocalPosition = new Vector3(-_rotatableBounds.Extents.x - _cornerSize, 0f, _rotatableBounds.Extents.z + _cornerSize);
            Vector3 backLeftLocalPosition = new Vector3(-_rotatableBounds.Extents.x - _cornerSize, 0f, -_rotatableBounds.Extents.z - _cornerSize);

            _frontRightRotationPoint.transform.position = _rotatableBounds.TransformPoint(frontRightLocalPosition);
            _backRightRotationPoint.transform.position = _rotatableBounds.TransformPoint(backRightLocalPosition);
            _frontLeftRotationPoint.transform.position = _rotatableBounds.TransformPoint(frontLeftLocalPosition);
            _backLeftRotationPoint.transform.position = _rotatableBounds.TransformPoint(backLeftLocalPosition);
        }

        private void CalculateCornersAndRotationPointsSize()
        {
            CalculateCornersSize();
            CalculateRotationPointsSize();
        }

        private void CalculateCornersSize()
        {
            float cornerSize = _cornerSize * _gizmosSizeScaler + _cornerOverallSize * _gizmosSizeScaler;
            Vector3 cornerSizeVector = new Vector3(cornerSize, cornerSize, cornerSize);

            _rightTopFrontCorner.transform.localScale = cornerSizeVector;
            _rightTopBackCorner.transform.localScale = cornerSizeVector;
            _rightBottomFrontCorner.transform.localScale = cornerSizeVector;
            _rightBottomBackCorner.transform.localScale = cornerSizeVector;

            _leftTopFrontCorner.transform.localScale = cornerSizeVector;
            _leftTopBackCorner.transform.localScale = cornerSizeVector;
            _leftBottomFrontCorner.transform.localScale = cornerSizeVector;
            _leftBottomBackCorner.transform.localScale = cornerSizeVector;
        }

        private void CalculateRotationPointsSize()
        {
            float rotationPointSize = _rotationPointSize * _gizmosSizeScaler;
            Vector3 rotationPointSizeVector = new Vector3(rotationPointSize, rotationPointSize, rotationPointSize);

            _frontRightRotationPoint.transform.localScale = rotationPointSizeVector;
            _backRightRotationPoint.transform.localScale = rotationPointSizeVector;
            _frontLeftRotationPoint.transform.localScale = rotationPointSizeVector;
            _backLeftRotationPoint.transform.localScale = rotationPointSizeVector;
        }

        public void StopManipulate()
        {
            if (!_isBusy)
            {
                return;
            }

            StopCoroutine(_startManipulateRoutine);
            _startManipulateRoutine = null;
            _target = null;
            _isBusy = false;

            _bounds.SetActive(false);

            Stoped.Invoke(this);
        }

        private void SetAxisEdgesLocalScale(Transform first, Transform second, Transform third, Transform fourth, Vector3 localScale)
        {
            first.localScale = localScale;
            second.localScale = localScale;
            third.localScale = localScale;
            fourth.localScale = localScale;
        }

        private void CalculateGizmosSizeScaler()
        {
            bool isResizeByCorner = _cornerSize > _rotationPointSize;
            float maxSize = isResizeByCorner ? _cornerSize : _rotationPointSize;

            if (isResizeByCorner)
            {
                _gizmosSizeScaler = Mathf.Min(_rotatableBounds.Size.Min() / (maxSize * _sizeScalerDevider), 1f);
            }
            else
            {
                _gizmosSizeScaler = Mathf.Min(_rotatableBounds.Size.Min() / (maxSize / 2f * _sizeScalerDevider), 1f);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            CalculateBounds();
        }
#endif

        #region Manipulation
        private void StartManipulating(ManipulationType manipulationType, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta)
        {
            Manipulate(manipulationType, GestureEventPhase.Started, source, cumulativeDelta);
        }

        private void UpdateManipulating(ManipulationType manipulationType, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta)
        {
            Manipulate(manipulationType, GestureEventPhase.Updated, source, cumulativeDelta);
        }

        private void CompleteManipulating(ManipulationType manipulationType, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta)
        {
            Manipulate(manipulationType, GestureEventPhase.Completed, source, cumulativeDelta);
        }

        private void CancelManipulating(ManipulationType manipulationType, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta)
        {
            Manipulate(manipulationType, GestureEventPhase.Canceled, source, cumulativeDelta);
        }

        private void Manipulate(ManipulationType manipulationType, GestureEventPhase phase, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta)
        {
            if (_currentGestureManipulationType == GestureManipulationType.Navigation)
            {
                return;
            }

            if (phase == GestureEventPhase.Started)
            {
                _startSource = source;
                _currentGestureManipulationType = GestureManipulationType.Manipulation;

                switch (manipulationType)
                {
                    case ManipulationType.Move:
                        _manipulationMover.StartManipulating(cumulativeDelta);
                        break;
                    case ManipulationType.Rotate:
                        _manipulationRotator.StartManipulating(cumulativeDelta);
                        break;
                    case ManipulationType.Scale:
                        _manipulationScaler.StartManipulating(cumulativeDelta);
                        break;
                    default:
                        break;
                }

                return;
            }

            if (_hasSourceChanged)
            {
                if (phase == GestureEventPhase.Completed || phase == GestureEventPhase.Canceled)
                {
                    _currentGestureManipulationType = GestureManipulationType.None;
                    _startSource = null;

                    _hasSourceChanged = false;
                }

                return;
            }

            if (source != _startSource)
            {
                _hasSourceChanged = true;

                // Cancel invokation not needed more..

                return;
            }

            if (phase == GestureEventPhase.Updated)
            {
                switch (manipulationType)
                {
                    case ManipulationType.Move:
                        _manipulationMover.UpdateManipulating(cumulativeDelta);
                        break;
                    case ManipulationType.Rotate:
                        _manipulationRotator.UpdateManipulating(cumulativeDelta);
                        break;
                    case ManipulationType.Scale:
                        _manipulationScaler.UpdateManipulating(cumulativeDelta);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (phase == GestureEventPhase.Completed)
                {
                    switch (manipulationType)
                    {
                        case ManipulationType.Move:
                            _manipulationMover.CompleteManipulating(cumulativeDelta);
                            break;
                        case ManipulationType.Rotate:
                            _manipulationRotator.CompleteManipulating(cumulativeDelta);
                            break;
                        case ManipulationType.Scale:
                            _manipulationScaler.CompleteManipulating(cumulativeDelta);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    // Cancel invokation not needed more..
                }

                _currentGestureManipulationType = GestureManipulationType.None;
                _startSource = null;
            }
        }
        #endregion

        #region Navigation
        private void StartNavigating(ManipulationType manipulationType, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset)
        {
            Navigate(manipulationType, GestureEventPhase.Started, source, normalizedOffset);
        }

        private void UpdateNavigating(ManipulationType manipulationType, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset)
        {
            Navigate(manipulationType, GestureEventPhase.Updated, source, normalizedOffset);
        }

        private void CompleteNavigating(ManipulationType manipulationType, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset)
        {
            Navigate(manipulationType, GestureEventPhase.Completed, source, normalizedOffset);
        }

        private void CancelNavigating(ManipulationType manipulationType, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset)
        {
            Navigate(manipulationType, GestureEventPhase.Canceled, source, normalizedOffset);
        }

        private void Navigate(ManipulationType manipulationType, GestureEventPhase phase, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset)
        {
            if (_currentGestureManipulationType == GestureManipulationType.Manipulation)
            {
                return;
            }

            if (phase == GestureEventPhase.Started)
            {
                _startSource = source;
            }

            if (_startSource != UnityEngine.XR.WSA.Input.InteractionSourceKind.Controller)
            {
                return;
            }
            else
            {
                if (phase == GestureEventPhase.Started)
                {
                    _currentGestureManipulationType = GestureManipulationType.Navigation;

                    switch (manipulationType)
                    {
                        case ManipulationType.Move:
                            _manipulationMover.StartNavigating(normalizedOffset);
                            break;
                        case ManipulationType.Rotate:
                            _manipulationRotator.StartNavigating(normalizedOffset);
                            break;
                        case ManipulationType.Scale:
                            _manipulationScaler.StartNavigating(normalizedOffset);
                            break;
                        default:
                            break;
                    }

                    return;
                }

                if (_hasSourceChanged)
                {
                    if (phase == GestureEventPhase.Completed || phase == GestureEventPhase.Canceled)
                    {
                        _currentGestureManipulationType = GestureManipulationType.None;
                        _startSource = null;

                        _hasSourceChanged = false;
                    }

                    return;
                }

                if (source != _startSource)
                {
                    _hasSourceChanged = true;

                    // Cancel invokation not needed more..

                    return;
                }

                if (phase == GestureEventPhase.Updated)
                {
                    switch (manipulationType)
                    {
                        case ManipulationType.Move:
                            _manipulationMover.UpdateNavigating(normalizedOffset);
                            break;
                        case ManipulationType.Rotate:
                            _manipulationRotator.UpdateNavigating(normalizedOffset);
                            break;
                        case ManipulationType.Scale:
                            _manipulationScaler.UpdateNavigating(normalizedOffset);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (phase == GestureEventPhase.Completed)
                    {
                        switch (manipulationType)
                        {
                            case ManipulationType.Move:
                                _manipulationMover.CompleteNavigating(normalizedOffset);
                                break;
                            case ManipulationType.Rotate:
                                _manipulationRotator.CompleteNavigating(normalizedOffset);
                                break;
                            case ManipulationType.Scale:
                                _manipulationScaler.CompleteNavigating(normalizedOffset);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        // Cancel invokation not needed more..
                    }

                    _currentGestureManipulationType = GestureManipulationType.None;
                    _startSource = null;
                }
            }
        }
        #endregion

        #region Event handlers
        #region Faces
        public void Faces_ManipulationStarted(Faces faces, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            _handledGizmo = faces;
            StartManipulating(ManipulationType.Move, source, cumulativeDelta);
        }

        public void Faces_ManipulationUpdated(Faces faces, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            UpdateManipulating(ManipulationType.Move, source, cumulativeDelta);
        }

        public void Faces_ManipulationCompleted(Faces faces, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            CompleteManipulating(ManipulationType.Move, source, cumulativeDelta);
            _handledGizmo = null;
        }

        public void Faces_ManipulationCanceled(Faces faces, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            CancelManipulating(ManipulationType.Move, source, cumulativeDelta);
            _handledGizmo = null;
        }

        public void Faces_NavigationStarted(Faces faces, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            _handledGizmo = faces;
            StartNavigating(ManipulationType.Move, source, normalizedOffset);
        }

        public void Faces_NavigationUpdated(Faces faces, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            UpdateNavigating(ManipulationType.Move, source, normalizedOffset);
        }

        public void Faces_NavigationCompleted(Faces faces, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            CompleteNavigating(ManipulationType.Move, source, normalizedOffset);
            _handledGizmo = null;
        }

        public void Faces_NavigationCanceled(Faces faces, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            CancelNavigating(ManipulationType.Move, source, normalizedOffset);
            _handledGizmo = null;
        }
        #endregion

        #region Corners
        public void Corner_ManipulationStarted(Corner corner, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            _handledGizmo = corner;
            StartManipulating(ManipulationType.Scale, source, cumulativeDelta);
        }

        public void Corner_ManipulationUpdated(Corner corner, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            UpdateManipulating(ManipulationType.Scale, source, cumulativeDelta);
        }

        public void Corner_ManipulationCompleted(Corner corner, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            CompleteManipulating(ManipulationType.Scale, source, cumulativeDelta);
            _handledGizmo = null;
        }

        public void Corner_ManipulationCanceled(Corner corner, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            CancelManipulating(ManipulationType.Scale, source, cumulativeDelta);
            _handledGizmo = null;
        }

        public void Corner_NavigationStarted(Corner corner, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            _handledGizmo = corner;
            StartNavigating(ManipulationType.Scale, source, normalizedOffset);
        }

        public void Corner_NavigationUpdated(Corner corner, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            UpdateNavigating(ManipulationType.Scale, source, normalizedOffset);
        }

        public void Corner_NavigationCompleted(Corner corner, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            CompleteNavigating(ManipulationType.Scale, source, normalizedOffset);
            _handledGizmo = null;
        }

        public void Corner_NavigationCanceled(Corner corner, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            CancelNavigating(ManipulationType.Scale, source, normalizedOffset);
            _handledGizmo = null;
        }
        #endregion

        #region RotationPoint
        public void RotationPoint_ManipulationStarted(RotationPoint rotationPoint, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            _handledGizmo = rotationPoint;
            StartManipulating(ManipulationType.Rotate, source, cumulativeDelta);
        }

        public void RotationPoint_ManipulationUpdated(RotationPoint rotationPoint, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            UpdateManipulating(ManipulationType.Rotate, source, cumulativeDelta);
        }

        public void RotationPoint_ManipulationCompleted(RotationPoint rotationPoint, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            CompleteManipulating(ManipulationType.Rotate, source, cumulativeDelta);
            _handledGizmo = null;
        }

        public void RotationPoint_ManipulationCanceled(RotationPoint rotationPoint, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            CancelManipulating(ManipulationType.Rotate, source, cumulativeDelta);
            _handledGizmo = null;
        }

        public void RotationPoint_NavigationStarted(RotationPoint rotationPoint, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            _handledGizmo = rotationPoint;
            StartNavigating(ManipulationType.Rotate, source, normalizedOffset);
        }

        public void RotationPoint_NavigationUpdated(RotationPoint rotationPoint, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            UpdateNavigating(ManipulationType.Rotate, source, normalizedOffset);
        }

        public void RotationPoint_NavigationCompleted(RotationPoint rotationPoint, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            CompleteNavigating(ManipulationType.Rotate, source, normalizedOffset);
            _handledGizmo = null;
        }

        public void RotationPoint_NavigationCanceled(RotationPoint rotationPoint, UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            CancelNavigating(ManipulationType.Rotate, source, normalizedOffset);
            _handledGizmo = null;
        }
        #endregion
        #endregion
        #endregion
        #endregion
    }
}