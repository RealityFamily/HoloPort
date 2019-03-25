using HoloGroup.UserInteraction.Input.KeyboardInput.Buttons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.UserInteraction.Input.KeyboardInput
{
    public class KeyboardLayout : MonoBehaviour
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
        private bool _shiftPressed;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        #region Event Handlers
        public void KeyboardShiftButton_Clicked()
        {
            _shiftPressed = !_shiftPressed;

            foreach (KeyboardAlphabetButton alphabetButton in transform.GetComponentsInChildren<KeyboardAlphabetButton>())
            {
                alphabetButton.SwitchCapital();
            }
        }
        #endregion
        #endregion
        #endregion
    }
}