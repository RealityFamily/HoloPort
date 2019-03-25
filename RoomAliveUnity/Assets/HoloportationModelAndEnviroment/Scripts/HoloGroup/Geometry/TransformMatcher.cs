using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.Geometry
{
    [ExecuteInEditMode]
    public class TransformMatcher : MonoBehaviour
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
        protected Transform _target;

        [SerializeField]
        private bool _matchPosition;

        [SerializeField]
        private bool _matchRotation;

        [SerializeField]
        private bool _matchScale;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        protected virtual void LateUpdate()
        {
            if (_target == null)
            {
                return;
            }

            Track();
        }

        protected virtual void Track()
        {
            if (_matchPosition)
            {
                transform.position = _target.position;
            }

            if (_matchRotation)
            {
                transform.rotation = _target.rotation;
            }

            if (_matchScale)
            {
                transform.localScale = _target.localScale;
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            LateUpdate();
        }
#endif
        #endregion
        #endregion
    }
}