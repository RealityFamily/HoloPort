using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.Utility
{
    public static class LayerUtility
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
        public static void SetLayerRecursively(GameObject parent, string layerName)
        {
            SetLayerRecursively(parent, LayerMask.NameToLayer(layerName));
        }

        public static void SetLayerRecursively(GameObject parent, int layer)
        {
            parent.layer = layer;

            foreach (Transform child in parent.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }
        #endregion
        #endregion
    }
}