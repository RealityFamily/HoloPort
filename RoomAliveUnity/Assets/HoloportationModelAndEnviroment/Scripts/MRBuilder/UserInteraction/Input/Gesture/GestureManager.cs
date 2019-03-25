using HoloGroup.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloGroup.UserInteraction.Input.Gesture;

namespace MRBuilder.UserInteraction.Input.Gesture
{
    public class GestureManager : MonoSingleton<GestureManager>
    {
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Fields
        [SerializeField]
        private GestureRecognizer _handGestureRecognizer;

        [SerializeField]
        private GestureRecognizer _clickerGestureRecognizer;
        #endregion

        #region Events
        #endregion

        #region Properties
        public GestureRecognizer HandGestureRecognizer { get { return _handGestureRecognizer; } }

        public GestureRecognizer ClickerGestureRecognizer { get { return _clickerGestureRecognizer; } }
        #endregion

        #region Methods
        public void CancelGesturesOnAllRecognizers()
        {
            _handGestureRecognizer.CancelGestures();
            _clickerGestureRecognizer.CancelGestures();
        }

        public void StartCapturingGesturesOnAllRecognizers()
        {
            _handGestureRecognizer.StartCapturingGestures();
            _clickerGestureRecognizer.StartCapturingGestures();
        }

        public void StopCapturingGesturesOnAllRecognizers()
        {
            _handGestureRecognizer.StopCapturingGestures();
            _clickerGestureRecognizer.StopCapturingGestures();
        }
        #endregion

        #region Event handlers
        #endregion
    }
}