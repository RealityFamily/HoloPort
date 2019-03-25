using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.UserInteraction.Input.KeyboardInput.Buttons
{
    public class KeyboardAlphabetButton : KeyboardSymbolButton
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
        public bool IsUpper { get { return char.IsUpper(_symbol); } }
        #endregion

        #region Methods
        public void MakeUpper()
        {
            _symbol = char.ToUpper(_symbol);
            _symbolText.text = _symbol.ToString();
        }

        public void MakeLower()
        {
            _symbol = char.ToLower(_symbol);
            _symbolText.text = _symbol.ToString();
        }

        public void SwitchCapital()
        {
            if (IsUpper)
            {
                MakeLower();
            }
            else
            {
                MakeUpper();
            }
        }
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}