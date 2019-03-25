using HoloGroup.UserInteraction.Input.KeyboardInput.Buttons;
using HoloGroup.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HoloGroup.UserInteraction.Input.KeyboardInput
{
    public class Keyboard : Window
    {
        #region Entities
        #region Enums
        public enum Layout
        {
            Qwerty,
            Numeric
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        [Serializable]
        private struct Size
        {
            public RectTransform _target;

            public float position;

            public float width;
        }

        [Serializable]
        private struct Sizes
        {
            public Size manipulationHandle;

            public Size placeholderImage;

            public Size removeButton;

            public Size closeButton;
        }

        [Serializable]
        private struct LayoutSizes
        {
            public Sizes qwerty;

            public Sizes numeric;
        }
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private string _initialString = string.Empty;

        private UnityAction<string> _onInputChangedCallback;

        private UnityAction _onCanceledCallback;

        private UnityAction _onCompletedCallback;

        private string _writenString = string.Empty;

        [SerializeField]
        private Text _placeholderText;

        [SerializeField]
        private LayoutSizes _layoutSizes;

        [SerializeField]
        private RectTransform _headerRectTransform;

        [SerializeField]
        private RectTransform _qwertyRectTransform;

        [SerializeField]
        private RectTransform _numericRectTransform;

        private Layout _currentLayout;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        private string WritenString
        {
            get { return _writenString; }
            set
            {
                _writenString = value;

                if (_writenString.Length > 32)
                {
                    _placeholderText.text = string.Format("..{0}", _writenString.Substring(_writenString.Length - 32, 32));
                }
                else
                {
                    _placeholderText.text = _writenString;
                }

                if (_onInputChangedCallback != null)
                {
                    _onInputChangedCallback.Invoke(_writenString);
                }
            }
        }
        #endregion

        #region Methods
        private void RegisterKeyboardCallbackData(string initialText, UnityAction<string> onChangedCallback, UnityAction onCanceledCallback, UnityAction onCompletedCallback)
        {
            _initialString = initialText;
            WritenString = _initialString;

            _onInputChangedCallback = onChangedCallback;
            _onCanceledCallback = onCanceledCallback;
            _onCompletedCallback = onCompletedCallback;
        }

        private void ShowLayout(Layout layout)
        {
            RectTransform rectTransform = (RectTransform)transform;

            RectTransform targetRectTransform = null;

            Sizes sizes;

            if (layout == Layout.Qwerty)
            {
                targetRectTransform = _qwertyRectTransform;
                sizes = _layoutSizes.qwerty;
                _numericRectTransform.gameObject.SetActive(false);
                _qwertyRectTransform.gameObject.SetActive(true);
            }
            else
            {
                targetRectTransform = _numericRectTransform;
                sizes = _layoutSizes.numeric;
                _numericRectTransform.gameObject.SetActive(true);
                _qwertyRectTransform.gameObject.SetActive(false);
            }

            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = targetRectTransform.sizeDelta.x;
            sizeDelta.y = _headerRectTransform.sizeDelta.y + targetRectTransform.sizeDelta.y;
            rectTransform.sizeDelta = sizeDelta;

            Vector2 manipAnchoredPos = sizes.manipulationHandle._target.anchoredPosition;
            manipAnchoredPos.x = sizes.manipulationHandle.position;
            sizes.manipulationHandle._target.anchoredPosition = manipAnchoredPos;
            Vector2 manipSizeDelta = sizes.manipulationHandle._target.sizeDelta;
            manipSizeDelta.x = sizes.manipulationHandle.width;
            sizes.manipulationHandle._target.sizeDelta = manipSizeDelta;

            Vector2 phAnchoredPos = sizes.placeholderImage._target.anchoredPosition;
            phAnchoredPos.x = sizes.placeholderImage.position;
            sizes.placeholderImage._target.anchoredPosition = phAnchoredPos;
            Vector2 phSizeDelta = sizes.placeholderImage._target.sizeDelta;
            phSizeDelta.x = sizes.placeholderImage.width;
            sizes.placeholderImage._target.sizeDelta = phSizeDelta;

            Vector2 removeAnchoredPos = sizes.removeButton._target.anchoredPosition;
            removeAnchoredPos.x = sizes.removeButton.position;
            sizes.removeButton._target.anchoredPosition = removeAnchoredPos;
            Vector2 removeSizeDelta = sizes.removeButton._target.sizeDelta;
            removeSizeDelta.x = sizes.removeButton.width;
            sizes.removeButton._target.sizeDelta = removeSizeDelta;

            Vector2 closeAnchoredPos = sizes.closeButton._target.anchoredPosition;
            closeAnchoredPos.x = sizes.closeButton.position;
            sizes.closeButton._target.anchoredPosition = closeAnchoredPos;
            Vector2 closeSizeDelta = sizes.closeButton._target.sizeDelta;
            closeSizeDelta.x = sizes.closeButton.width;
            sizes.closeButton._target.sizeDelta = closeSizeDelta;

            _currentLayout = layout;
        }

        public void Open(Layout layout, string initialText, UnityAction<string> onChangedCallback, UnityAction onCanceledCallback, UnityAction onCompletedCallback)
        {
            ShowLayout(layout);

            RegisterKeyboardCallbackData(initialText, onChangedCallback, onCanceledCallback, onCompletedCallback);

            base.Open();
        }

        public void OpenInFrontOfUser(Layout layout, string initialText, UnityAction<string> onChangedCallback, UnityAction onCanceledCallback, UnityAction onCompletedCallback, float distance = 2f)
        {
            ShowLayout(layout);

            RegisterKeyboardCallbackData(initialText, onChangedCallback, onCanceledCallback, onCompletedCallback);

            base.OpenInFrontOfUser(distance);
        }

        public void CancelInput()
        {
            WritenString = _initialString;

            if (_onCanceledCallback != null)
            {
                _onCanceledCallback.Invoke();
            }

            StopInput();
        }

        public void CompleteInput()
        {
            if (_onCompletedCallback != null)
            {
                _onCompletedCallback.Invoke();
            }

            StopInput();
        }

        private void StopInput()
        {
            _initialString = string.Empty;

            _onInputChangedCallback = null;
            _onCanceledCallback = null;
            _onCompletedCallback = null;

            Close();
        }

        #region Event Handlers
        public void KeyboardSymbolButton_Clicked(KeyboardButton keyboardButton)
        {
            char symbol = ((KeyboardSymbolButton)keyboardButton).Symbol;

            if (_currentLayout == Layout.Numeric && symbol == '.' && _writenString.Contains("."))
            {
                return;
            }

            WritenString = string.Format("{0}{1}", WritenString, ((KeyboardSymbolButton)keyboardButton).Symbol);
        }

        public void KeyboardRemoveButton_Clicked(KeyboardButton keyboardRemoveButton)
        {
            if (WritenString.Length == 0)
            {
                return;
            }

            WritenString = WritenString.Substring(0, WritenString.Length - 1);
        }

        public void KeyboardEnterButton_Clicked(KeyboardButton keyboardEnterButton)
        {
            CompleteInput();
        }
        #endregion
        #endregion
        #endregion
    }
}