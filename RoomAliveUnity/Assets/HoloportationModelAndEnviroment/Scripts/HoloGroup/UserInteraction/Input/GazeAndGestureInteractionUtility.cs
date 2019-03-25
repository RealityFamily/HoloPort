using HoloGroup.UserInteraction.Input.Gaze;
using HoloGroup.UserInteraction.Input.Gesture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.UserInteraction.Input
{
    public static class GazeAndGestureInteractionUtility
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
        private static LayerMask _oldGazeInteractMask;

        private static LayerMask _oldGestureHandlerInteractMask;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        public static void SaveCurrentInteractionsStates()
        {
            _oldGazeInteractMask = GazeManager.Instance.interactMask;
            _oldGestureHandlerInteractMask = GesturesHandler.Instance.InteractMask;
        }

        public static void SaveAndChangeCurrentInteractionStates(LayerMask newInteractMask)
        {
            SaveCurrentInteractionsStates();
            ChangeGazeAndGestureInteractMaskTo(newInteractMask);
        }

        public static void ChangeGazeAndGestureInteractMaskTo(LayerMask newInteractMask)
        {
            GazeManager.Instance.interactMask = newInteractMask;
            GesturesHandler.Instance.InteractMask = newInteractMask;
        }

        public static void RestoreSavedInteractionsStates()
        {
            GazeManager.Instance.interactMask = _oldGazeInteractMask;
            GesturesHandler.Instance.InteractMask = _oldGestureHandlerInteractMask;
        }
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}