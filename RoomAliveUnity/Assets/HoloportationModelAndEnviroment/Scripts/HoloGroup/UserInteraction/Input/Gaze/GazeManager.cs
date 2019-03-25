using HoloGroup.Patterns;
using System;
using UnityEngine;

namespace HoloGroup.UserInteraction.Input.Gaze
{
    /// <summary>
    /// StandartGazeManager used to find object (with collider and without if UI) which currently in our gaze.
    /// In case when raycast to graphic, need understand, that your graphic should not be overriden by other graphic.
    /// </summary>
    public class GazeManager : MonoSingleton<GazeManager>
    {
        #region Properties
        /// <summary>
        /// Layer mask for raycasting.
        /// </summary>
        [Tooltip("Mask for raycasting")]
        public LayerMask collideMask;

        [SerializeField]
        [Tooltip("Mask for interact with gazable objects")]
        public LayerMask interactMask;

        /// <summary>
        /// Max distance for raycasting.
        /// </summary>
        [SerializeField]
        [Range(1f, 128f)]
        [Tooltip("The length of ray used for casting")]
        private float _maxRayDistance = 4f;
        public float MaxRayDistance { get { return _maxRayDistance; } }

        /// <summary>
        /// HasHit used in external code for react on hit.
        /// </summary>
        private bool _hasHit;
        public bool hasHit { get { return _hasHit; } }

        /// <summary>
        /// Represent position of hit point
        /// </summary>
        private Vector3 _hitPoint;
        public Vector3 hitPoint { get { return _hitPoint; } }

        /// <summary>
        /// Represent normal of hit point
        /// </summary>
        private Vector3 _hitNormal;
        public Vector3 hitNormal { get { return _hitNormal; } }

        /// <summary>
        /// GazedObject is the object we are looking at.
        /// </summary>
        private GameObject _gazedObject;
        public GameObject gazedObject { get { return _gazedObject; } }

        /// <summary>
        /// Is need raycast to graphic?
        /// </summary>
        [SerializeField]
        private bool _raycastToGraphic;
        public bool raycastToGraphic { get { return _raycastToGraphic; } set { _raycastToGraphic = value; } }

        /// <summary>
        /// Is raycast to graphic object occure?
        /// </summary>
        private bool _isGraphicHit;
        public bool isGraphicHit { get { return _isGraphicHit; } }

        /// <summary>
        /// Strategy object, which implement all gaze alghoritm.
        /// can be changed in runtime.
        /// </summary>
        private IGazeManagerStrategy _gazeStrategy;
        public IGazeManagerStrategy gazeStrategy { set { _gazeStrategy = value; } }

        #region Events
        public event Action<GameObject> GazedObjectChangedEvent;
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Initialize requared properties.
        /// </summary>
        private void Awake()
        {
            // Initialize standart strategy for gazing.
            _gazeStrategy = new GazeManagerDefaultStrategy();
        }

        /// <summary>
        /// In update we delegate gaze alghoritm to self strategy object.
        /// </summary>
        private void Update()
        {
            ProcessGaze();
        }

         void ProcessGaze()
        {
            GameObject oldGazedObject = _gazedObject;

            // Delegate gaze processing to self strategy object.
            _gazeStrategy.ProcessGaze(interactMask, _maxRayDistance, collideMask, _raycastToGraphic, ref _hasHit, ref _isGraphicHit, ref _hitPoint, ref _hitNormal, ref _gazedObject);

            if (oldGazedObject != _gazedObject)
            {
                if (GazedObjectChangedEvent != null)
                {
                    GazedObjectChangedEvent.Invoke(_gazedObject);
                }
            }
        }

        public bool Raycast(LayerMask layerMask, out RaycastHit hitInfo)
        {
            Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            return Physics.Raycast(cameraRay, out hitInfo, _maxRayDistance, layerMask);
        }

        public bool Raycast(out RaycastHit hitInfo)
        {
            return Raycast(collideMask, out hitInfo);
        }

        public RaycastHit[] RaycastAll(LayerMask layerMask)
        {
            Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            return Physics.RaycastAll(cameraRay, _maxRayDistance, layerMask);
        }

        public RaycastHit[] RaycastAll()
        {
            Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            return Physics.RaycastAll(cameraRay, _maxRayDistance, collideMask);
        }
        #endregion
    }
}