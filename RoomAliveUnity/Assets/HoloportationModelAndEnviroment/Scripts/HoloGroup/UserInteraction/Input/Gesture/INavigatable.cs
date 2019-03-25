using UnityEngine;
using System.Collections;


namespace HoloGroup.UserInteraction.Input.Gesture
{
    /// <summary>
    /// Use this interface for make your objects interactable with navigate gestures.
    /// </summary>
    public interface INavigatable
    {
        // Methods below is called separately for all axes.

        /// <summary>
        /// called when your finger hold on this object and then start moving.
        /// </summary>
        /// <param name="source">Source which emit this message.</param>
        /// <param name="cumulativeData">Relative to main camera axis vector (camera transform is zero point).</param>
        void OnNavigationStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

        /// <summary>
        /// called when your finger continue moving after start.
        /// </summary>
        /// <param name="source">Source which emit this message.</param>
        /// <param name="cumulativeData">Relative to main camera axis vector (camera transform is zero point).</param>
        void OnNavigationUpdated(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

        /// <summary>
        /// called when your finger release after start.
        /// </summary>
        /// <param name="source">Source which emit this message.</param>
        /// <param name="cumulativeData">Relative to main camera axis vector (camera transform is zero point).</param>
        void OnNavigationCompleted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

        /// <summary>
        /// called when your finger is out of tracking zone.
        /// </summary>
        /// <param name="source">Source which emit this message.</param>
        /// <param name="cumulativeData">Zero vector.</param>
        void OnNavigationCanceled(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);
    }
}