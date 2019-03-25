using HoloGroup.UserInteraction.Input.Gaze;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.UserInteraction.Manipulation.Gizmos
{
    public class ManipulatorGizmos : MonoBehaviour, IGazable
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
        private MeshRenderer _meshRenderer;

        private Color _defaultColor;

        [SerializeField]
        private Color _gazedColor = Color.yellow;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _defaultColor = _meshRenderer.sharedMaterial.color;
        }

        public void OnGazeEnter()
        {
            _meshRenderer.material.color = _gazedColor;
        }

        public void OnGazeLeave()
        {
            _meshRenderer.material.color = _defaultColor;
        }
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}