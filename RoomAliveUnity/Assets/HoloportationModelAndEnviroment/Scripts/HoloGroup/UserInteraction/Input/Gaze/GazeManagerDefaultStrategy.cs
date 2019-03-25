using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HoloGroup.UserInteraction.Input.Gaze
{
    /// <summary>
    /// This strategy work wirh physic and graphic IGazable objects.
    /// </summary>
    public class GazeManagerDefaultStrategy : IGazeManagerStrategy
    {
        private class AverageRaycaster
        {
            private List<Ray> _rays = new List<Ray>();

            private int _maxRayCountForAverage;

            public AverageRaycaster(int maxRayCountForAverage)
            {
                _rays = new List<Ray>();

                _maxRayCountForAverage = maxRayCountForAverage;
            }

            public void Enqueue(Vector3 origin, Vector3 direction)
            {
                Enqueue(new Ray(origin, direction));
            }

            public void Enqueue(Ray ray)
            {
                _rays.Add(ray);

                if (_rays.Count > _maxRayCountForAverage)
                {
                    _rays.RemoveAt(0);
                }
            }

            private void Dequeue()
            {
                _rays.RemoveAt(0);
            }

            public Ray GetAverageRay()
            {
                if (_rays.Count == 0)
                {
                    throw new System.Exception();
                }

                Vector3 averageOrigin = _rays[0].origin;
                Vector3 averageDirection = _rays[0].direction;

                for (int i = 1; i < _rays.Count; i++)
                {
                    averageOrigin += _rays[i].origin;
                    averageDirection += _rays[i].direction;
                }

                averageOrigin /= _rays.Count;
                averageDirection /= _rays.Count;

                return new Ray(averageOrigin, averageDirection);
            }
        }

        private Camera _mainCamera;

        private AverageRaycaster _averageRaycaster = new AverageRaycaster(6);

        // Represent mouse input data.
        private PointerEventData _pointerEventData = new PointerEventData(EventSystem.current);

        private IGazable _gazableObject;

        public void ProcessGaze(LayerMask interactMask, float maxRayDistance, LayerMask collideMask, bool raycastToGraphic, ref bool hasHit, ref bool isGraphicHit, ref Vector3 hitPoint, ref Vector3 hitNormal, ref GameObject gazedObject)
        {
            _mainCamera = Camera.main;
            _pointerEventData.position = new Vector2(Screen.width / 2f, Screen.height / 2f);

            // First we need to simple raycast.
            //Ray rayFromCamera = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
            _averageRaycaster.Enqueue(_mainCamera.transform.position, _mainCamera.transform.forward);
            Ray averageRay = _averageRaycaster.GetAverageRay();

            // Container to save physic hit information.
            RaycastHit physicHitInfo;

            // Container to save nearest graphic hit information.
            RaycastResult? graphicHitInfo = null;

            // Here we raycast to physic objects.
            // raycastState is bitmask for save information about what type of raycast is occure.
            // 0: no raycast;
            // 1: physic raycast occure;
            // 2: graphic raycast occure;
            // 3: both raycasts occure.
            // Right now we save to raycastState 0 or 1.
            byte raycastState = Physics.Raycast(averageRay, out physicHitInfo, maxRayDistance, collideMask) ? (byte)1 : (byte)0;

            // Check for allowing to graphic raycast.
            // If allow, then we raycast to all canvases through EventSystem
            // and get nearest raycastResult.
            if (raycastToGraphic)
            {
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(_pointerEventData, results);

                // Results have graphic element which placed on all layers, but we need only on allowed.
                results.RemoveAll((x) => { return collideMask.value != (collideMask.value | (1 << x.gameObject.layer)); });

                // If hit occure.
                if (results.Count > 0)
                {
                    // Find nearest graphic that we hit.
                    RaycastResult nearestGraphicHit = results[0];

                    // If nearestResult is null, then just save new raycastResult.
                    if (graphicHitInfo == null)
                    {
                        // Get the nearest (on zero position) graphic hit in list.
                        graphicHitInfo = nearestGraphicHit;

                        // Save graphic raycast flag to raycastState (we use this later).
                        raycastState |= 2;
                    }
                    // If we already have graphic raycast result in previous steps, 
                    // then we need compare which of this two is nearest.
                    else
                    {
                        // If new is nearest than old, then we save new as nearest.
                        if (results[0].distance < graphicHitInfo.Value.distance)
                        {
                            graphicHitInfo = nearestGraphicHit;
                        }
                    }
                }
            }

            // Now we need compare physic (if occure) and graphic (if occure) raycasts to which of they both is nearest.
            switch (raycastState)
            {
                // No raycast occure.
                case 0:
                    // Mark that we not have hit.
                    hasHit = false;
                    isGraphicHit = false;

                    // Save default hit point and normal.
                    hitPoint = averageRay.GetPoint(maxRayDistance);
                    hitNormal = -_mainCamera.transform.forward;

                    // If we look at any object before.
                    if (gazedObject != null)
                    {
                        IGazable oldGazable = gazedObject.GetComponent<IGazable>();

                        // If old gazed object is gaze-sensetive, we need to call OnGazeLeave method.
                        if (oldGazable != null)
                        {
                            oldGazable.OnGazeLeave();
                        }
                    }

                    _gazableObject = null;
                    gazedObject = null;
                    break;
                // Physic raycast occure.
                case 1:
                    HandleGazeWithPhysic(interactMask, physicHitInfo, ref hasHit, ref isGraphicHit, ref hitPoint, ref hitNormal, ref gazedObject);
                    break;
                // Graphic raycast occure.
                case 2:
                    HandleGazeWithGraphic(interactMask, graphicHitInfo.Value, ref hasHit, ref isGraphicHit, ref hitPoint, ref hitNormal, ref gazedObject);
                    break;
                // Both raycast occure.
                case 3:
                    // In this case we need select one of two raycast data and handle it.
                    // We choose the one that is closer.
                    if (physicHitInfo.distance < graphicHitInfo.Value.distance)
                    {
                        HandleGazeWithPhysic(interactMask, physicHitInfo, ref hasHit, ref isGraphicHit, ref hitPoint, ref hitNormal, ref gazedObject);
                    }
                    else
                    {
                        HandleGazeWithGraphic(interactMask, graphicHitInfo.Value, ref hasHit, ref isGraphicHit, ref hitPoint, ref hitNormal, ref gazedObject);
                    }
                    break;
            }
        }

        /// <summary>
        /// Physic gaze handler
        /// </summary>
        /// <param name="physicHitInfo">Physic hit information.</param>
        /// <param name="hasHit">Hit flag.</param>
        /// <param name="isGraphicHit">Graphic hit flag.</param>
        /// <param name="hitPoint">Hit point in world space.</param>
        /// <param name="hitNormal">Hit normal in world space.</param>
        /// <param name="gazedObject">Gazed object.</param>
        private void HandleGazeWithPhysic(LayerMask interactMask, RaycastHit physicHitInfo, ref bool hasHit, ref bool isGraphicHit, ref Vector3 hitPoint, ref Vector3 hitNormal, ref GameObject gazedObject)
        {
            // Mark that we have hit.
            hasHit = true;
            isGraphicHit = false;

            // Handle with physic data.
            HandleGaze(interactMask, physicHitInfo.point, physicHitInfo.normal, physicHitInfo.collider.gameObject, ref hitPoint, ref hitNormal, ref gazedObject);
        }

        /// <summary>
        /// Graphic gaze handler
        /// </summary>
        /// <param name="graphicHitInfo">Graphic hit information</param>
        /// <param name="hasHit">Hit flag.</param>
        /// <param name="isGraphicHit">Graphic hit flag.</param>
        /// <param name="hitPoint">Hit point in world space.</param>
        /// <param name="hitNormal">Hit normal in world space.</param>
        /// <param name="gazedObject">Gazed object.</param>
        private void HandleGazeWithGraphic(LayerMask interactMask, RaycastResult graphicHitInfo, ref bool hasHit, ref bool isGraphicHit, ref Vector3 hitPoint, ref Vector3 hitNormal, ref GameObject gazedObject)
        {
            // mark that we have graphic hit.
            hasHit = true;
            isGraphicHit = true;

            // TODO: standart graphicHitInfo.Value.worldPosition and graphicHitInfo.Value.worldNormal 
            // fields have incorrect values, maybe need change method below in future.
            // Calculate hit point and normal from hand.
            Vector3 point = _mainCamera.transform.TransformPoint(0f, 0f, graphicHitInfo.distance);
            Vector3 normal = -graphicHitInfo.gameObject.transform.forward;

            // Handle with graphic data.
            HandleGaze(interactMask, point, normal, graphicHitInfo.gameObject, ref hitPoint, ref hitNormal, ref gazedObject);
        }

        /// <summary>
        /// Common gaze handler of StandartGazeManagerDefaultStrategy.
        /// </summary>
        /// <param name="newHitPoint">Point that we are looking at right now.</param>
        /// <param name="newHitNormal">Normal of polygon that we are looking at right now.</param>
        /// <param name="newGazedObject">Gazed object that we are looking at right now.</param>
        /// <param name="hitPoint">Last point that we are looking at.</param>
        /// <param name="hitNormal">Last normal that we are looking at.</param>
        /// <param name="gazedObject">Last gazed object that we are looking at.</param>
        private void HandleGaze(LayerMask interactMask, Vector3 newHitPoint, Vector3 newHitNormal, GameObject newGazedObject, ref Vector3 hitPoint, ref Vector3 hitNormal, ref GameObject gazedObject)
        {
            // Update position and normal if hit point
            hitPoint = newHitPoint;
            hitNormal = newHitNormal;

            IGazable newGazableObject = null;

            bool isInteractable = (interactMask.value & LayerMask.GetMask(LayerMask.LayerToName(newGazedObject.layer))) != 0;
            if (isInteractable)
            {
                newGazableObject = newGazedObject.GetComponent<IGazable>();
            }


            // If gazed object is gaze-sensitive.
            if (newGazableObject != null)
            {
                // If old gazed object and currently gazed object is different,
                // we need  to call OnGazeEnter method on currently gazed object.
                if (_gazableObject != newGazableObject)
                {
                    // If old gaze object exist.
                    if (_gazableObject != null)
                    {
                        _gazableObject.OnGazeLeave();
                    }

                    newGazableObject.OnGazeEnter();
                }
            }
            // If gazed object is default object.
            else
            {
                // If old gazed object and currently gazed object is different,
                // we need to check old gazed object on gaze-sensetive behaviour.
                if (_gazableObject != newGazableObject)
                {
                    // If old gaze object exist.
                    if (_gazableObject != null)
                    {
                        _gazableObject.OnGazeLeave();
                    }
                }
            }

            _gazableObject = newGazableObject;
            gazedObject = newGazedObject;
        }
    }
}