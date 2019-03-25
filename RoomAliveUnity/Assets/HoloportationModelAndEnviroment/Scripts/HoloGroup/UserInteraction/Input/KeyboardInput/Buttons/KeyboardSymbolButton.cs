using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HoloGroup.UserInteraction.Input.KeyboardInput.Buttons
{
    public class KeyboardSymbolButton : KeyboardButton
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
        protected char _symbol;

        [SerializeField]
        protected Text _symbolText;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public char Symbol { get { return _symbol; } }
        #endregion

        #region Methods
        #region Event Handlers
        #endregion
        #endregion
        #endregion
    }
}