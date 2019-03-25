using UnityEngine;
using System.Collections;


namespace HoloGroup.UserInteraction.Input.Gesture
{
    /// <summary>
    /// Use this interface for make your objects interactable with hold gestures.
    /// </summary>
    public interface IHoldable
    {
        /// <summary>
        /// Called when you hold your finger on this object some time.
        /// </summary>
        void OnHoldStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay);

        /// <summary>
        /// Called when you release your finger on this object.
        /// </summary>
        void OnHoldCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay);

        /// <summary>
        /// Called when your finger is out of tracking zone.
        /// </summary>
        void OnHoldCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Ray headRay);
    }
}