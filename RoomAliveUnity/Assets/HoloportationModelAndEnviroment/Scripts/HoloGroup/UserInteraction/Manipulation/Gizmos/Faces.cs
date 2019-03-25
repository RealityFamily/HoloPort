using HoloGroup.UserInteraction.Input.Gesture;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace HoloGroup.UserInteraction.Manipulation.Gizmos
{
    public class Faces : ManipulatorGizmos, IManipulatable, INavigatable
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class ManipulationStartedEvent : UnityEvent<Faces, UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class ManipulationUpdatedEvent : UnityEvent<Faces, UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class ManipulationCompletedEvent : UnityEvent<Faces, UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class ManipulationCanceledEvent : UnityEvent<Faces, UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class NavigationStartedEvent : UnityEvent<Faces, UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class NavigationUpdatedEvent : UnityEvent<Faces, UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class NavigationCompletedEvent : UnityEvent<Faces, UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }

        [Serializable]
        public class NavigationCanceledEvent : UnityEvent<Faces, UnityEngine.XR.WSA.Input.InteractionSourceKind, Vector3, Ray> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #endregion

        #region Events
        public ManipulationStartedEvent ManipulationStarted = new ManipulationStartedEvent();

        public ManipulationUpdatedEvent ManipulationUpdated = new ManipulationUpdatedEvent();

        public ManipulationCompletedEvent ManipulationCompleted = new ManipulationCompletedEvent();

        public ManipulationCanceledEvent ManipulationCanceled = new ManipulationCanceledEvent();

        public NavigationStartedEvent NavigationStarted = new NavigationStartedEvent();

        public NavigationUpdatedEvent NavigationUpdated = new NavigationUpdatedEvent();

        public NavigationCompletedEvent NavigationCompleted = new NavigationCompletedEvent();

        public NavigationCanceledEvent NavigationCanceled = new NavigationCanceledEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        public void OnManipulationStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationStarted.Invoke(this, source, cumulativeDelta, headRay);
        }

        public void OnManipulationUpdated(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationUpdated.Invoke(this, source, cumulativeDelta, headRay);
        }

        public void OnManipulationCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationCompleted.Invoke(this, source, cumulativeDelta, headRay);
        }

        public void OnManipulationCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            ManipulationCanceled.Invoke(this, source, cumulativeDelta, headRay);
        }

        public void OnNavigationStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationStarted.Invoke(this, source, normalizedOffset, headRay);
        }

        public void OnNavigationUpdated(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationUpdated.Invoke(this, source, normalizedOffset, headRay);
        }

        public void OnNavigationCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationCompleted.Invoke(this, source, normalizedOffset, headRay);
        }

        public void OnNavigationCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            NavigationCanceled.Invoke(this, source, normalizedOffset, headRay);
        }
        #endregion
        #endregion
    }
}