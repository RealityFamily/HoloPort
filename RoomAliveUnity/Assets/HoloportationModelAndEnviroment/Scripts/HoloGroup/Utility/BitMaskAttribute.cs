using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.Utility
{
    public class BitMaskAttribute : PropertyAttribute
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
        private Type _propertyType;
        #endregion

        #region Events
        #endregion

        #region Properties
        public Type PropertyType { get { return _propertyType; } }
        #endregion

        #region Methods
        public BitMaskAttribute(Type propertyType)
        {
            _propertyType = propertyType;
        }
        #endregion

        #region Event handlers
        #endregion
    }
}