using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.Geometry
{
    [ExecuteInEditMode]
    public class RectTransformMatcher : TransformMatcher
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
        private bool _matchPivot;

        [SerializeField]
        private bool _matchSize;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        protected override void LateUpdate()
        {
            if (_target == null)
            {
                return;
            }

            Track();
        }

        protected override void Track()
        {
            base.Track();

            RectTransform rectTransform = (RectTransform)transform;
            RectTransform target = (RectTransform)_target;

            if (_matchPivot)
            {
                rectTransform.pivot = target.pivot;
            }

            if (_matchSize)
            {
                rectTransform.sizeDelta = target.sizeDelta;
            }
        }
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}