using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

namespace HoloGroup.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Window : MonoBehaviour
    {
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class OpeningEvent : UnityEvent<Window> { }

        [Serializable]
        public class OpenedEvent : UnityEvent<Window> { }

        [Serializable]
        public class ClosedEvent : UnityEvent<Window> { }

        [Serializable]
        public class ClosingEvent : UnityEvent<Window> { }
        #endregion

        #region Fields
        protected bool _isBusy;

        protected bool _isOpen;

        protected CanvasGroup _canvasGroup;

        [SerializeField]
        [Range(0f, 1f)]
        protected float _fadeDuration = 1f;

        protected float _shiftDistance = 0.25f;
        #endregion

        #region Events
        public OpeningEvent Opening = new OpeningEvent();

        public OpenedEvent Opened = new OpenedEvent();

        public ClosingEvent Closing = new ClosingEvent();

        public ClosedEvent Closed = new ClosedEvent();
        #endregion

        #region Properties
        public bool IsOpen { get { return _isOpen; } }

        public float FadeDuration { get { return _fadeDuration; } }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Open()
        {
            if (_isBusy || _isOpen)
            {
                return;
            }

            _isBusy = true;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(DOTween.To(() => { return _canvasGroup.alpha; }, (x) => { _canvasGroup.alpha = x; }, 1f, _fadeDuration));

            Vector3 originPosition = transform.position;

            transform.Translate(0f, 0f, _shiftDistance);

            sequence.Insert(0f, transform.DOMove(originPosition, _fadeDuration));

            sequence.OnComplete(() =>
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;

                _isOpen = true;
                _isBusy = false;

                Opened.Invoke(this);
            });

            sequence.Play();

            Opening.Invoke(this);
        }

        public void Open(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;

            Open();
        }

        public void OpenInFrontOfUser(float distance = 2f)
        {
            transform.position = Camera.main.transform.TransformPoint(0f, 0f, distance);
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

            Open();
        }

        public virtual void Close()
        {
            if (_isBusy || !_isOpen)
            {
                return;
            }

            _isBusy = true;

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(DOTween.To(() => { return _canvasGroup.alpha; }, (x) => { _canvasGroup.alpha = x; }, 0f, _fadeDuration));

            Vector3 originPosition = transform.position;

            Vector3 targetPosition = transform.position + transform.TransformDirection(0f, 0f, _shiftDistance);

            sequence.Insert(0f, transform.DOMove(targetPosition, _fadeDuration));

            sequence.OnComplete(() =>
            {
                transform.position = originPosition;

                _isOpen = false;
                _isBusy = false;

                Closed.Invoke(this);
            });

            sequence.Play();

            Closing.Invoke(this);
        }

        public void MoveToFrontOfUser(float distance = 2f)
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;

            Sequence sequence = DOTween.Sequence();

            Vector3 frontPosition = Camera.main.transform.TransformPoint(new Vector3(0f, 0f, distance));
            sequence.Append(transform.DOMove(frontPosition, _fadeDuration));

            Quaternion frontRotation = Quaternion.LookRotation(Camera.main.transform.forward);
            sequence.Insert(0f, transform.DORotateQuaternion(frontRotation, _fadeDuration));

            sequence.AppendCallback(() => { _isBusy = false; });

            sequence.Play();
        }
        #endregion

        #region Event handlers
        #endregion
    }
}