using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Linq;
using HoloGroup.UserInteraction.Input.Gaze;
using System;
using HoloGroup.UserInteraction.Input.Gesture;


using MRBuilder.Architecture.Layouts;
using MRBuilder.Architecture.label;
using HoloGroup.Geometry;
using HoloGroup.UserInteraction.Manipulation;
using MRBuilder.UserInteraction.Manipulation;
using HoloGroup.Utility;

namespace MRBuilder.Architecture
{
    public class Building : MonoBehaviour, ITapable
    {
        #region Enums
        public enum VisualMode
        {
            Inside,
            Outside
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        public class Factory
        {
            private const string _floorTag = "_TAG_FLOOR";
            private const string _insideTag = "_TAG_INSIDE";

            private const string _furnitureLayoutName = "FURNITURES";

            public Building MakeBuilding(Model model, float minSizeLimitation, float maxSizeLimitation)
            {
                GameObject buildingGO = new GameObject("Building");
                //buildingGO.layer = LayerMask.NameToLayer("BuildingBound");

                Building building = buildingGO.AddComponent<Building>();
                building.transform.position = model.transform.position;

                building.name = string.Format("Building [{0}]", model.OriginName);
                model.name = "Model";

                #region Attaching model to building
                model.transform.SetParent(building.transform);

                building._model = model;
                #endregion

                #region Adding collider
                building._model.gameObject.SetActive(true);
                Bounds buildingBounds = BoundsUtility.GetGlobalBounds(building.gameObject, BoundsUtility.BoundsCreateOption.Mesh);

                BoxCollider buildingBoxCollider = building.gameObject.AddComponent<BoxCollider>();
                buildingBoxCollider.center = buildingBounds.center - building.transform.position;
                buildingBoxCollider.size = buildingBounds.size;
                #endregion

                #region Find and handle tags
                Transform floor = null;
                Transform insidePoint = null;

                FindTags(building.transform, ref floor, ref insidePoint);

                building._defaultInsidePoint = insidePoint;
                if (building._defaultInsidePoint == null)
                {
                    building._defaultInsidePoint = new GameObject("DefaultInsidePoint").transform;
                    building._defaultInsidePoint.SetParent(model.transform);
                    building._defaultInsidePoint.position = buildingBounds.center;
                }
                #endregion

                #region Adding colliders and BuildingElement script to all
                MeshFilter[] meshFilters = model.GetComponentsInChildren<MeshFilter>();
                BuildingElement.Factory buildingElementFactory = new BuildingElement.Factory();

                foreach (MeshFilter meshFilter in meshFilters)
                {
                    meshFilter.gameObject.AddComponent<MeshCollider>().sharedMesh = meshFilter.mesh;

                    if (floor == null || !meshFilter.transform.IsChildOf(floor))
                    {
                        buildingElementFactory.Create(meshFilter.gameObject, BuildingElement.ElementType.Other);
                    }
                    else
                    {
                        buildingElementFactory.Create(meshFilter.gameObject, BuildingElement.ElementType.Floor);
                    }
                }
                #endregion

                #region Calculate min and max scale
                float _maxSide = Mathf.Max(buildingBounds.size.x, buildingBounds.size.y, buildingBounds.size.z);

                building._minScale = minSizeLimitation / _maxSide;
                building._maxScale = maxSizeLimitation / _maxSide;

                building.transform.localScale = new Vector3(building._minScale, building._minScale, building._minScale);
                #endregion

                #region Handle layouts
                //LayoutManager.Factory layoutManagerFactory = new LayoutManager.Factory();
                //building._layoutManager = layoutManagerFactory.Create(building);
                #endregion

                #region Handle view points
                //ViewPointManager.Factory viewPointManagerFactory = new ViewPointManager.Factory();
                //building._viewPointManager = viewPointManagerFactory.Create(building);
                #endregion

                #region Handle labels
                //LabelManager.Factory labelManagerFactory = new LabelManager.Factory();
                //building._labelManager = labelManagerFactory.Create(building);
                #endregion

                #region Handle furnitures
                //FurnitureManager.Factory furnitureManagerFactory = new FurnitureManager.Factory();
                //building._furnitureManager = furnitureManagerFactory.Create(building, building.LayoutManager.GetLayout(_furnitureLayoutName).gameObject);
                #endregion

                #region Set building layer
                //LayerUtility.SetLayerRecursively(building.gameObject, "Building");
                #endregion

                return building;
            }

            private void FindTags(Transform parent, ref Transform floor, ref Transform insidePoint)
            {
                if (parent.name.Contains(_floorTag))
                {
                    if (!floor)
                    {
                        floor = parent;
                    }
                }

                if (parent.name.Contains(_insideTag))
                {
                    if (!insidePoint)
                    {
                        insidePoint = parent;
                    }
                }

                foreach (Transform child in parent)
                {
                    FindTags(child, ref floor, ref insidePoint);

                    if (floor && insidePoint)
                    {
                        return;
                    }
                }
            }
        }

        [Serializable]
        public class BusyStateChangedEvent : UnityEvent<Building, bool> { }

        [Serializable]
        public class JumpingToViewPointEvent : UnityEvent<Building, string> { }
        #endregion

        #region Fields
        private Model _model;

        private float _minScale;

        private float _maxScale;

        private Transform _defaultInsidePoint;

        private LayoutManager _layoutManager;

        //private ViewPointManager _viewPointManager;

        private LabelManager _labelManager;

        //private FurnitureManager _furnitureManager;

        private VisualMode _currentVisualMode = VisualMode.Outside;

        private bool _isBusy;

        private Sequence _animationSequence;
        #endregion

        #region Events
        public event Action<Building> Insiding;

        public event Action<Building> Insided;

        public event Action<Building> Outsiding;

        public event Action<Building> Outsided;

        public JumpingToViewPointEvent JumpingToViewPoint = new JumpingToViewPointEvent();

        public BusyStateChangedEvent BusyStateChanged = new BusyStateChangedEvent();
        #endregion

        #region Properties
        public Model Model { get { return _model; } }

        public float MinScale { get { return _minScale; } }

        public float MaxScale { get { return _maxScale; } }

        public Transform DefaultInsidePoint { get { return _defaultInsidePoint; } }

        public LayoutManager LayoutManager { get { return _layoutManager; } }

       // public ViewPointManager ViewPointManager { get { return _viewPointManager; } }

        public LabelManager LabelManager { get { return _labelManager; } }

       // public FurnitureManager FurnitureManager { get { return _furnitureManager; } }

        public VisualMode CurrentVisualMode
        {
            get { return _currentVisualMode; }
            private set
            {
                if (_currentVisualMode == value)
                {
                    return;
                }

                _currentVisualMode = value;

                if (_currentVisualMode == VisualMode.Inside)
                {
                    if (Insided != null)
                    {
                        Insided.Invoke(this);
                    }
                }
                else
                {
                    if (Outsided != null)
                    {
                        Outsided.Invoke(this);
                    }
                }
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;

                BusyStateChanged.Invoke(this, _isBusy);
            }
        }

        public object ManipulationFacade { get; private set; }
        #endregion

        #region Methods
        public void GoInInsideMode(Vector3 insidePoint, float transitionDuration)
        {
            if (IsBusy || _currentVisualMode == VisualMode.Inside)
            {
                return;
            }

            ForceGoInInsideMode(insidePoint, transitionDuration);
        }

        private void ForceGoInInsideMode(Vector3 insidePoint, float transitionDuration)
        {
            if (Insiding != null)
            {
                Insiding.Invoke(this);
            }

            IsBusy = true;

            Vector3 centerToPointVector = insidePoint - transform.position;
            Vector3 centerToScaledPointVector = centerToPointVector * (1f / transform.localScale.x);
            Vector3 cameraToScaledPointVector = transform.position + centerToScaledPointVector - Camera.main.transform.position;

            Vector3 targetInsidePosition = transform.position - cameraToScaledPointVector;

            _animationSequence = DOTween.Sequence();

            _animationSequence.Append(transform.DOMove(targetInsidePosition, transitionDuration).SetEase(Ease.Linear));


            var tween = DOTween.To(
                () => { return transform.localScale.x; },
                (x) => { transform.localScale = new Vector3(x, x, x); },
                1f,
                transitionDuration).SetEase(Ease.Linear);

            tween.OnComplete(() =>
            {
                IsBusy = false;
                CurrentVisualMode = VisualMode.Inside;
            });

            _animationSequence.Insert(0f, tween);

            _animationSequence.SetAutoKill();
            _animationSequence.Play();
        }

        public void GoInOutsideMode(Vector3 outsidePosition, Quaternion outsideRotation, float outsideScale, float transitionDuration)
        {
            if (IsBusy || _currentVisualMode == VisualMode.Outside)
            {
                return;
            }

            if (Outsiding != null)
            {
                Outsiding.Invoke(this);
            }

            IsBusy = true;

            _animationSequence = DOTween.Sequence();

            _animationSequence.Append(transform.DOMove(outsidePosition, transitionDuration).SetEase(Ease.Linear));
            _animationSequence.Insert(0f, transform.DORotateQuaternion(outsideRotation, transitionDuration).SetEase(Ease.Linear));

            var tween = DOTween.To(
                () => { return transform.localScale.x; },
                (x) => { transform.localScale = new Vector3(x, x, x); },
                outsideScale,
                transitionDuration).SetEase(Ease.Linear);

            tween.OnComplete(() =>
            {
                IsBusy = false;
                CurrentVisualMode = VisualMode.Outside;
            });

            _animationSequence.Insert(0f, tween);

            _animationSequence.Play();
        }

        public void GoInOutsideMode(float transitionDuration = 0f)
        {
            GoInOutsideMode(transform.position, transform.rotation, _minScale, transitionDuration);
        }

        private void ImmediateGoInOutsideMode()
        {
            if (Outsiding != null)
            {
                Outsiding.Invoke(this);
            }

            transform.localScale = new Vector3(_minScale, _minScale, _minScale);
            CurrentVisualMode = VisualMode.Outside;
        }

        public void ResetState()
        {
            _layoutManager.ResetAllLayoutsKinds();

            StopTransitionAnimation();
            ImmediateGoInOutsideMode();
        }

        private void StopTransitionAnimation()
        {
            if (IsBusy)
            {
                _animationSequence.Kill();
                _animationSequence = null;

                IsBusy = false;
            }
        }

        public void JumpToPoint(Vector3 point, float duration = 0f)
        {
            if (_isBusy)
            {
                return;
            }

            ForceJumpToPoint(point, duration);
        }

        private void ForceJumpToPoint(Vector3 point, float duration = 0f)
        {
            IsBusy = true;

            Vector3 resultPoint = Camera.main.transform.position + transform.position - point;

            transform.DOMove(resultPoint, duration).OnComplete(() =>
            {
                IsBusy = false;
            }).Play();
        }

        public void JumpToViewPoint(string viewPointName, float duration = 0f)
        {
            if (_isBusy)
            {
                return;
            }

            ForceJumpToViewPoint(viewPointName, duration);
        }

        private void ForceJumpToViewPoint(string viewPointName, float duration = 0f)
        {
            //Transform viewPoint = _viewPointManager.GetViewPointAndSetAsCurrent(viewPointName);

            //Vector3 fromBuildingToViewPointVector = viewPoint.position - transform.position;

            //Vector3 pointDirection = viewPoint.forward;
            //pointDirection.y = 0f;

            //Vector3 viewDirection = Camera.main.transform.forward;
            //viewDirection.y = 0f;

            //Quaternion fromToRotation = Quaternion.identity;

            //if (Mathf.Approximately(Vector3.Angle(pointDirection, viewDirection), 180f))
            //{
            //    fromToRotation = Quaternion.AngleAxis(180f, Vector3.up);
            //}
            //else
            //{
            //    fromToRotation = Quaternion.FromToRotation(pointDirection, viewDirection);
            //} 
            
            //Vector3 rotatedFromBuildingToViewPointVector = fromToRotation * fromBuildingToViewPointVector;

            //Vector3 targetPoint = transform.position + rotatedFromBuildingToViewPointVector;

            //if (_currentVisualMode != VisualMode.Inside)
            //{
            //    ForceGoInInsideMode(targetPoint, duration);
            //}
            //else
            //{
            //    ForceJumpToPoint(targetPoint, duration);
            //}

            //Quaternion endRotation = transform.rotation * fromToRotation;
            //transform.DORotateQuaternion(endRotation, duration).Play();

            //JumpingToViewPoint.Invoke(this, viewPointName);
        }

        public void JumpToNextViewPoint()
        {
            if (_isBusy)
            {
                return;
            }

          //  ForceJumpToViewPoint(_viewPointManager.GetNextViewPointName(), 1f);
        }

        public void OnTapped(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int count, Ray headRay)
        {
            ManipulatorFacade.Instance.StartManipulateBuilding(gameObject);
        }
        #endregion

        #region Events handlers
        #endregion
    }
}