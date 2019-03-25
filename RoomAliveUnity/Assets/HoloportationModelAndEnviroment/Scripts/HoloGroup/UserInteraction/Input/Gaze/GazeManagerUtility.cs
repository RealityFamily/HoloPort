using HoloGroup.UserInteraction.Input.Gaze;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.UserInteraction.Input.Gaze
{
    public static class GazeManagerUtility
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
        #endregion

        #region Events
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static Vector3 GetClampedHitPoint(float maxDistance)
        {
            Vector3 vectorToPoint = GazeManager.Instance.hitPoint - Camera.main.transform.position;

            if (vectorToPoint.magnitude > maxDistance)
            {
                vectorToPoint = vectorToPoint.normalized * maxDistance;
            }

            return Camera.main.transform.position + vectorToPoint;
        }

        public static Vector3 GetHitNormalOrDefault()
        {
            if (GazeManager.Instance.hasHit)
            {
                return GazeManager.Instance.hitNormal;
            }
            else
            {
                return -Camera.main.transform.forward;
            }
        }
        #endregion

        #region Event handlers
        #endregion
    }
}