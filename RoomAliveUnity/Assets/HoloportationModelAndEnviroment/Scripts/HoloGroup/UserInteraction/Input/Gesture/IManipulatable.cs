using UnityEngine;
using System.Collections;


namespace HoloGroup.UserInteraction.Input.Gesture
{
    /// <summary>
    /// Use this interface for make your objects interactable with manipulate gestures.
    /// </summary>
    public interface IManipulatable
    {
        /// <summary>
        /// called when your finger hold on this object and then start moving.
        /// </summary>
        /// <param name="cumulativeData">Relative vector (from start).</param>
        void OnManipulationStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

        /// <summary>
        /// called when your finger continue moving after start.
        /// </summary>
        /// <param name="cumulativeData">Relative vector (from start).</param>
        void OnManipulationUpdated(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

        /// <summary>
        /// called when your finger release after start.
        /// </summary>
        /// <param name="cumulativeData">Relative vector (from start).</param>
        void OnManipulationCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

        /// <summary>
        /// called when your finger is out of tracking zone.
        /// </summary>
        /// <param name="cumulativeData">Zero vector.</param>
        void OnManipulationCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);
    }
}