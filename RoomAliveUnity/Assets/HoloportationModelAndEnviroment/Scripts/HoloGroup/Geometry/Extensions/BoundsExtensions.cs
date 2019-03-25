using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.Geometry.Extensions
{
    public static class BoundsExtension
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        public static float MaxSide(this Bounds bounds)
        {
            return bounds.size.Max();
        }

        public static float MinSide(this Bounds bounds)
        {
            return bounds.size.Min();
        }
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}