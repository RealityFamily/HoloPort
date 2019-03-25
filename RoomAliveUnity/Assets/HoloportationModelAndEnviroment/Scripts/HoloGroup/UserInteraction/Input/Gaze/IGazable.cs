using UnityEngine;
using System.Collections;

namespace HoloGroup.UserInteraction.Input.Gaze
{
    /// <summary>
    /// Use this interface for make your objects interactable with gaze.
    /// </summary>
    public interface IGazable
    {
        /// <summary>
        /// Called when gaze enter to this object.
        /// </summary>
        void OnGazeEnter();

        /// <summary>
        /// Called when gaze leave from this object.
        /// </summary>
        void OnGazeLeave();
    }
}