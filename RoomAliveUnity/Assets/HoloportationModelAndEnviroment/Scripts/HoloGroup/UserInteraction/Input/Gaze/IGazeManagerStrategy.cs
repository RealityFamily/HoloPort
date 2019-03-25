using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace HoloGroup.UserInteraction.Input.Gaze
{
    /// <summary>
    /// This interface give you ability to create your own gaze alghoritm if you wish,
    /// just pass required parameters in ProcessGaze method and handle gaze.
    /// </summary>
    public interface IGazeManagerStrategy
    {
        /// <summary>
        /// Method-strategy for gazing. In process gaze you need correct fill ref values.
        /// </summary>
        /// <param name="_maxRayDistance">Max distance for raycasting, can be changed in main class.</param>
        /// <param name="collideMask">Layer mask for raycasting, can be changed in main class.</param>
        /// <param name="raycastToGraphic">Is need raycast to graphic?</param>
        /// <param name="graphicRaycasters">List of graphic raycasters which need use in raycast system.</param>
        /// <param name="hasHit">Represent hit flag.</param>
        /// <param name="isGraphicHit">Have we hit with graphic?</param>
        /// <param name="hitPoint">The point where ray collide.</param>
        /// <param name="hitNormal">The direction where polygon look at.</param>
        /// <param name="gazedObject">Object with which we are collide.</param>
        void ProcessGaze(LayerMask interactMask, float _maxRayDistance, LayerMask collideMask, bool raycastToGraphic, ref bool hasHit, ref bool isGraphicHit, ref Vector3 hitPoint, ref Vector3 hitNormal, ref GameObject gazedObject);
    }
}