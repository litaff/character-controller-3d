namespace CharacterController3D
{
    using System;
    using InputController;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class CharacterController3D : MonoBehaviour
    {
        #region Odin Attributes

        private const string COMPONENTS = "Components";
        private const string SETTINGS = "Settings";
        private const string DEBUG = "Debug";

        #endregion
        
        [SerializeField, BoxGroup(COMPONENTS)]
        private Rigidbody rigidbody;
        [SerializeField, BoxGroup(COMPONENTS)]
        private CameraController cameraController;
        [SerializeField, BoxGroup(COMPONENTS)]
        private InputController inputController;
        [SerializeField, BoxGroup(SETTINGS)]
        private float moveForce;
        [SerializeField, BoxGroup(SETTINGS)]
        private float lookSpeed;
        [SerializeField, BoxGroup(SETTINGS)]
        private float jumpForce;
        
        [SerializeField, FoldoutGroup(DEBUG)]
        private bool isDebug;
        [SerializeField, FoldoutGroup(DEBUG), ShowIf("@isDebug")]
        private float horizontal;
        [SerializeField, FoldoutGroup(DEBUG), ShowIf("@isDebug")]
        private float vertical;
        [SerializeField, FoldoutGroup(DEBUG), ShowIf("@isDebug")]
        private bool isGrounded;

        private void Awake()
        {
            // TEMP: Remove this line
            inputController.Initialize();
            
            inputController.OnMove += OnMoveHandler;
            inputController.OnJumpDown += OnJumpHandler;
        }

        private void FixedUpdate()
        {
            var moveDirection = cameraController.Right * horizontal + cameraController.Forward * vertical;
            moveDirection.Normalize();
            
            if (moveDirection != Vector3.zero)
            {
                var position = transform.position;
                transform.LookAt(Vector3.Lerp(position + transform.forward,
                    position + moveDirection, lookSpeed));
            }

            rigidbody.AddForce(moveDirection * moveForce, ForceMode.Force);
        }

        private void LateUpdate()
        {
            cameraController.transform.position = transform.position;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                isGrounded = true;
            }
        }
        
        private void OnCollisionExit(Collision collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                isGrounded = false;
            }
        }

        private void OnMoveHandler(Vector2 input)
        {
            if (!isGrounded) return;
            horizontal = input.x;
            vertical = input.y;
        }
        
        private void OnJumpHandler()
        {
            if (!isGrounded) return;
            rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
