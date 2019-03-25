using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRBuilder.Architecture
{
    public class BuildingElement : MonoBehaviour
    {
        #region Entities
        #region Enums
        public enum ElementType
        {
            Other,
            Floor
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        public class Factory
        {
            public BuildingElement Create(GameObject gameObject, ElementType elementType)
            {
                BuildingElement buildingElement = gameObject.AddComponent<BuildingElement>();
                buildingElement._elementType = elementType;

                return buildingElement;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private ElementType _elementType;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public ElementType Type { get { return _elementType; } }
        #endregion

        #region Methods
        #endregion
        #endregion
    }
}