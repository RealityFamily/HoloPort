using HoloGroup.Patterns;
using HoloGroup.UserInteraction.Manipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRBuilder.UserInteraction.Manipulation
{
    public class ManipulatorManager : MonoSingleton<ManipulatorManager>
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
        [SerializeField]
        private Manipulator _manipulator;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public Manipulator Manipulator { get { return _manipulator; } }
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}