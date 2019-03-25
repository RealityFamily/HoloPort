using HoloGroup.Patterns;
using HoloGroup.UserInteraction.Manipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRBuilder.UserInteraction.Manipulation
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ManipulationPanel : MonoSingleton<ManipulationPanel>
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Manipulator _manipulator;

        private Coroutine _startTrackRoutine;

        private Vector3 _startScale;
        #endregion

        #region Events
        public UnityEvent RemoveClicked = new UnityEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _startScale = transform.localScale;
        }

        private void SetCanvasGroupStateTo(bool state)
        {
            if (state)
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.alpha = 1f;
            }
            else
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.alpha = 0f;
            }
        }

        public void StartTrack()
        {
            if (_startTrackRoutine != null)
            {
                return;
            }

            _startTrackRoutine = StartCoroutine(StartTrackRoutine());
        }

        private IEnumerator StartTrackRoutine()
        {
            SetCanvasGroupStateTo(true);

            while (true)
            {
                Vector3 nearestPoint = GetNearestToCameraSideInfo();

                transform.rotation = Quaternion.LookRotation(_manipulator.transform.position - nearestPoint, _manipulator.transform.up);
                transform.localScale = _startScale * (_manipulator.Size.x / 1.2f);

                Vector3 nearestLocalPoint = _manipulator.transform.InverseTransformPoint(nearestPoint);
                nearestLocalPoint.y = -_manipulator.Extents.y + ((RectTransform)transform).rect.height / 2 * transform.localScale.x;

                transform.position = _manipulator.transform.TransformPoint(nearestLocalPoint);

                yield return null;
            }
        }

        private Vector3 GetNearestToCameraSideInfo()
        {
            Vector3[] points = new Vector3[]
            {
                _manipulator.transform.position + _manipulator.transform.forward * _manipulator.Extents.z + _manipulator.transform.forward * 0.01f,
                _manipulator.transform.position - _manipulator.transform.forward * _manipulator.Extents.z - _manipulator.transform.forward * 0.01f,
                _manipulator.transform.position +_manipulator.transform.right * _manipulator.Extents.x + _manipulator.transform.right * 0.01f,
                _manipulator.transform.position - _manipulator.transform.right * _manipulator.Extents.x - _manipulator.transform.right * 0.01f
            };

            Vector3 nearestPoint = points[0];
            float nearestDistance = Vector3.Distance(nearestPoint, Camera.main.transform.position);

            for (int i = 1; i < points.Length; i++)
            {
                float distance = Vector3.Distance(points[i], Camera.main.transform.position);
                if (distance < nearestDistance)
                {
                    nearestPoint = points[i];
                    nearestDistance = distance;
                }
            }

            return nearestPoint;
        }

        public void StopTrack()
        {
            if (_startTrackRoutine == null)
            {
                return;
            }

            SetCanvasGroupStateTo(false);

            StopCoroutine(_startTrackRoutine);
            _startTrackRoutine = null;
        }

        #region Event Handlers
        public void RemoveButton_OnClick()
        {
            RemoveClicked.Invoke();
        }
        #endregion
        #endregion
        #endregion
    }
}