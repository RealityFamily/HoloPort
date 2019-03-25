using HoloGroup.Patterns;
using HoloGroup.UserInteraction.Input.Gaze;
using HoloGroup.UserInteraction.Manipulation;
using HoloGroup.UserInteraction.Manipulation.Gizmos;
using MRBuilder.Architecture;

using MRBuilder.UserInteraction.Input.Gesture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MRBuilder.UserInteraction.Manipulation
{
    public class ManipulatorFacade : MonoSingleton<ManipulatorFacade>
    {
        #region Entities
        #region Enums
        private enum TargetType
        {
            Furniture,
            Building
        }
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
        [SerializeField]
        private ManipulationPanel _manipulationPanel;

        private TargetType _targetType;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        public void StartManipulateBuilding(GameObject target)
        {
            _targetType = TargetType.Building;

            GestureManager.Instance.HandGestureRecognizer.Tapped.AddListener(HandGestureRecognizer_Tapped);

            ManipulatorManager.Instance.Manipulator.StartManipulate(target);
        }

        public void StopManipulateBuilding()
        {
            if (!ManipulatorManager.Instance.Manipulator.IsStarted)
            {
                return;
            }

            GestureManager.Instance.HandGestureRecognizer.Tapped.RemoveListener(HandGestureRecognizer_Tapped);

            ManipulatorManager.Instance.Manipulator.StopManipulate();
        }

        public void StartManipulateFurniture(GameObject target, bool isIncluded)
        {
            if (ManipulatorManager.Instance.Manipulator.IsStarted)
            {
                StopManipulateFurniture();
            }

            _targetType = TargetType.Furniture;

            GestureManager.Instance.HandGestureRecognizer.Tapped.AddListener(HandGestureRecognizer_Tapped);

            ManipulatorManager.Instance.Manipulator.StartManipulate(target, true);

            if (!isIncluded)
            {
                _manipulationPanel.StartTrack();
            }
        }

        public void StopManipulateFurniture()
        {
            if (!ManipulatorManager.Instance.Manipulator.IsStarted)
            {
                return;
            }

            GestureManager.Instance.HandGestureRecognizer.Tapped.RemoveListener(HandGestureRecognizer_Tapped);

            ManipulatorManager.Instance.Manipulator.StopManipulate();
            _manipulationPanel.StopTrack();
        }

        public void StopManipulating()
        {
            StopManipulateBuilding();
            StopManipulateFurniture();
        }
        #endregion

        #region Event Handlers
        private void HandGestureRecognizer_Tapped(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int count, Ray headRay)
        {
            if (_targetType == TargetType.Furniture)
            {
                if (GazeManager.Instance.gazedObject != null &&
                GazeManager.Instance.gazedObject.GetComponent<ManipulatorGizmos>() != null ||
                GazeManager.Instance.gazedObject.GetComponentInParent<ManipulationPanel>() != null)
                {
                    return;
                }

                StopManipulateFurniture();
            }
            else
            {
                if (GazeManager.Instance.gazedObject != null && GazeManager.Instance.gazedObject.GetComponent<ManipulatorGizmos>() != null )
                {
                    return;
                }

                StopManipulateBuilding();
            }
        }

        public void EventsCommutator_BuildingChanged()
        {
            if (_targetType == TargetType.Furniture)
            {
                StopManipulateFurniture();
            }
            else
            {
                StopManipulateBuilding();
            }
        }

        public void ManipulationPanel_RemoveClicked()
        {
            Transform target = ManipulatorManager.Instance.Manipulator.Target;
            StopManipulateFurniture();


        }
        #endregion
        #endregion
    }
}