using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGroup.Control
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Fields
        [SerializeField]
        private KeyCode _catchKey = KeyCode.LeftShift;

        [SerializeField]
        [Range(1f, 8f)]
        private float _movementSpeed = 1f;

        [SerializeField]
        [Range(1f, 8f)]
        private float _rotationSpeed = 1f;
        #endregion

        #region Events
        #endregion

        #region Properties
        #endregion

        #region Methods
        private void Update()
        {
            if (!UnityEngine.Input.GetKey(_catchKey))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            ProcessMovement();
            ProcessRotation();
        }

        private void ProcessMovement()
        {
            float vertical = UnityEngine.Input.GetAxis("Vertical");
            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            float diagonal = UnityEngine.Input.GetAxis("Diagonal");

            transform.Translate(new Vector3(horizontal, diagonal, vertical) * _movementSpeed * Time.deltaTime);
        }

        private void ProcessRotation()
        {
            float mouseX = UnityEngine.Input.GetAxis("Mouse X");
            float mouseY = UnityEngine.Input.GetAxis("Mouse Y");

            Quaternion quatX = Quaternion.Euler(0f, mouseX, 0f);
            Quaternion quatY = Quaternion.Euler(-mouseY, 0f, 0f);

            Quaternion resultRotation = quatX * transform.rotation * quatY;
            Vector3 lookDirection = resultRotation * Vector3.forward;

            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
        #endregion

        #region Event handlers
        #endregion
    }
}