using UnityEngine;
using System.Collections;


namespace HoloGroup.UserInteraction.Input.Gesture
{
    public interface IGesturesHandler
    {
        void OnRecognitionStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay);

        void OnRecognitionEnded(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay);

        void OnGestureError(string error, int hResult);

        void OnTapped(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int count, Ray headRay);

        void OnHoldStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay);

        void OnHoldCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay);

        void OnHoldCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay);

        void OnManipulationStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

        void OnManipulationUpdated(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

        void OnManipulationCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

        void OnManipulationCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

        void OnNavigationStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

        void OnNavigationUpdated(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

        void OnNavigationCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

        void OnNavigationCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);
    }
}