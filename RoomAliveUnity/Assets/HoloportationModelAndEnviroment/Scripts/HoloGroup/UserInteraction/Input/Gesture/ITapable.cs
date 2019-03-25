using UnityEngine;
using System.Collections;


namespace HoloGroup.UserInteraction.Input.Gesture
{
    /// <summary>
    /// Use this interface for make your objects interactable with tap gesture.
    /// </summary>
    public interface ITapable
    {
        /// <summary>
        /// Called when you tap on this object.
        /// </summary>
        void OnTapped(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int count, Ray headRay);
    }
}