using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRBuilder.Architecture
{
    public class Model : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        public class Factory
        {
            public Model MakeModel(Transform modelTransform, string originName, bool isIncluded)
            {
                Model model = modelTransform.gameObject.AddComponent<Model>();
                model._originName = originName;
                model._isIncluded = isIncluded;

                return model;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields        
        [SerializeField]
        private bool _isIncluded;

        [SerializeField]
        private string _originName;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public bool IsIncluded { get { return _isIncluded; } }

        public string OriginName { get { return _originName; } }
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}