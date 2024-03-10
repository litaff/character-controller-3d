namespace CharacterController3D
{
    using System;
    using DG.Tweening;
    using Extensions;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class CharacterController3D : MonoBehaviour
    {
        #region Odin Attributes

        protected const string COMPONENTS = "Components";
        protected const string SETTINGS = "Settings";
        protected const string DEBUG = "Debug";

        #endregion

        [SerializeField, BoxGroup(COMPONENTS)]
        private Rigidbody rigidbody;
        [SerializeField, BoxGroup(COMPONENTS)]
        private Animator animator;        
        [SerializeField, BoxGroup(SETTINGS)]
        private float moveSpeed;
        [SerializeField, BoxGroup(SETTINGS)]
        private float moveForce;
        [SerializeField, BoxGroup(SETTINGS)]
        private float lookSpeed;
        [SerializeField, BoxGroup(SETTINGS)]
        private float jumpForce;
        [SerializeField, BoxGroup(SETTINGS)]
        private LayerMask groundingLayers;
        
        [SerializeField, FoldoutGroup(DEBUG)]
        private bool isDebug;
        [SerializeField, FoldoutGroup(DEBUG), ShowIf("@isDebug")]
        private float rightInput;
        [SerializeField, FoldoutGroup(DEBUG), ShowIf("@isDebug")]
        private float forwardInput;
        [SerializeField, FoldoutGroup(DEBUG), ShowIf("@isDebug")]
        private float requestedRightInput;
        [SerializeField, FoldoutGroup(DEBUG), ShowIf("@isDebug")]
        private float requestedForwardInput;
        [field: SerializeField, FoldoutGroup(DEBUG), ShowIf("@isDebug")]
        public bool IsGrounded { get; private set; }

        private Vector3 normalizedMoveDirection;
        private CharacterController3DInAirBehaviour inAirBehaviour;

        public Vector3 HorizontalVelocity => new(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        public Vector3 HorizontalInput => new(rightInput, 0f, forwardInput);

        protected virtual void FixedUpdate()
        {
            if(IsGrounded && (Math.Abs(rightInput - requestedRightInput) > 0.001f || Math.Abs(forwardInput - requestedForwardInput) > 0.001f))
            {
                rightInput = requestedRightInput;
                forwardInput = requestedForwardInput;
            }
        }

        protected virtual void LateUpdate()
        {
            animator.SetFloat("Speed", (HorizontalVelocity.magnitude / moveSpeed));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(groundingLayers.Contains(collision.gameObject.layer))
            {
                // Triggers jump land animation, which calls OnGroundedHandler
                animator.SetBool("GroundAble", true);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if(groundingLayers.Contains(collision.gameObject.layer))
            {
                OnUnGroundedHandler();
            }
        }

        protected virtual void Initialize()
        {
            inAirBehaviour = animator.GetBehaviour<CharacterController3DInAirBehaviour>();
            inAirBehaviour.OnInAirExit += OnGroundedHandler;

            InitializeDown();
        }

        protected virtual void InitializeDown()
        {
            if (!rigidbody.SweepTest(Vector3.down, out var hit, .01f))
            {
                OnUnGroundedHandler();
                return;
            }
            
            if (groundingLayers.Contains(hit.collider.gameObject.layer))
            {
                OnGroundedHandler();
            }
            else
            {
                OnUnGroundedHandler();
            }
        }
        
        protected virtual void Move(Vector3 input)
        {
            normalizedMoveDirection = input.normalized;
            
            LookTowardsMove(normalizedMoveDirection);
            
            if (!IsGrounded) return;
            
            // Accelerate to move speed
            if (HorizontalVelocity.magnitude < moveSpeed)
            {
                rigidbody.AddForce(normalizedMoveDirection * moveForce, ForceMode.Force);
                return;
            }

            // Don't set velocity if input is zero
            if (normalizedMoveDirection.magnitude <= 0f) return;
            
            rigidbody.velocity = new Vector3(normalizedMoveDirection.x * moveSpeed, rigidbody.velocity.y, normalizedMoveDirection.z * moveSpeed);
        }

        protected virtual void LookTowardsMove(Vector3 normalizedDirection)
        {
            if (normalizedDirection == Vector3.zero) return;
            var position = transform.position;
            transform.DOLookAt(position + normalizedDirection, lookSpeed, AxisConstraint.None, transform.up);
        }

        protected virtual void OnMoveHandler(Vector2 input)
        {
            requestedRightInput = input.x;
            requestedForwardInput = input.y;
            
            if (!IsGrounded) return;
            
            rightInput = requestedRightInput;
            forwardInput = requestedForwardInput;
        }

        protected virtual void OnJumpHandler()
        {
            if (!IsGrounded) return;
            rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("Jump", true);
        }

        /// <summary>
        /// Called when jump land animation ends
        /// </summary>
        protected virtual void OnGroundedHandler()
        {
            IsGrounded = true;
            animator.SetBool("FreeFall", false);
            animator.SetBool("Jump", false);
        }

        /// <summary>
        /// Called on grounding collision exit
        /// </summary>
        protected virtual void OnUnGroundedHandler()
        {
            rightInput = 0;
            forwardInput = 0;
            IsGrounded = false;
            animator.SetBool("GroundAble", false);
            animator.SetBool("FreeFall", true);
            animator.SetBool("Jump", false);
        }
    }
}
