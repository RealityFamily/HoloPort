using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HoloGroup.UserInteraction.Input.KeyboardInput.Buttons
{
    public class KeyboardButton : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class ClickedEvent : UnityEvent<KeyboardButton> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #endregion

        #region Events
        public ClickedEvent Clicked = new ClickedEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        public void Button_OnClick()
        {
            Clicked.Invoke(this);
        }
        #endregion
        #endregion
    }
}