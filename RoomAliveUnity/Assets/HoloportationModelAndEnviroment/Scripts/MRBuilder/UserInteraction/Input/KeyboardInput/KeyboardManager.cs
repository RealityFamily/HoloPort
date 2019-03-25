using HoloGroup.UserInteraction.Input.KeyboardInput;
using HoloGroup.Patterns;
using HoloGroup.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MRBuilder.UserInteraction.Input.KeyboardInput
{
    public class KeyboardManager : MonoSingleton<KeyboardManager>
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
        private Keyboard _keyboard;

        private bool _isKeyboardBusy;

        private CanvasGroup _placeholderCanvasGroup;
        #endregion

        #region Events
        public UnityEvent KeyboardOpening = new UnityEvent();

        public UnityEvent KeyboardClosed = new UnityEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        public void InvokeKeyboard(Keyboard.Layout layout, CanvasGroup placeholderCanvasGroup, string initialText, UnityAction<string> onChangedCallback, UnityAction onCanceledCallback, UnityAction onCompletedCallback)
        {
            if (_isKeyboardBusy)
            {
                return;
            }

            KeyboardOpening.Invoke();

            _placeholderCanvasGroup = placeholderCanvasGroup;
            _placeholderCanvasGroup.blocksRaycasts = false;

            float distanceToInput = Vector3.Distance(_placeholderCanvasGroup.transform.position, Camera.main.transform.position);

            if (distanceToInput < 2.2f)
            {
                _keyboard.OpenInFrontOfUser(layout, initialText, onChangedCallback, onCanceledCallback, onCompletedCallback, distanceToInput / 2f);
            }
            else
            {
                _keyboard.OpenInFrontOfUser(layout, initialText, onChangedCallback, onCanceledCallback, onCompletedCallback);
            }
        }

        #region Event Handlers
        public void Keyboard_Opening(Window keyboardWindow)
        {
            _isKeyboardBusy = true;
        }

        public void Keyboard_Closed(Window keyboardWindow)
        {
            _placeholderCanvasGroup.blocksRaycasts = true;

            _isKeyboardBusy = false;

            KeyboardClosed.Invoke();
        }
        #endregion
        #endregion
        #endregion
    }
}