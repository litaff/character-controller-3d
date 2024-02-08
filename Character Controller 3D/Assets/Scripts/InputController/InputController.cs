namespace InputController
{
    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class InputController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;
        
        public event Action<Vector2> OnMove;
        public event Action OnJumpDown;
        public event Action<Vector2> OnLook; 

        public void Initialize()
        {
            playerInput.currentActionMap.FindAction("Jump").performed += OnJumpDownHandler;
            playerInput.currentActionMap.FindAction("Move").performed += OnMoveHandler;
            playerInput.currentActionMap.FindAction("Look").performed += OnLookHandler;
        }

        public void Dispose()
        {
            playerInput.currentActionMap.FindAction("Jump").performed -= OnJumpDownHandler;
            playerInput.currentActionMap.FindAction("Move").performed -= OnMoveHandler;
            playerInput.currentActionMap.FindAction("Look").performed -= OnLookHandler;
        }

        private void OnLookHandler(InputAction.CallbackContext obj)
        {
            var look = obj.ReadValue<Vector2>();
            OnLook?.Invoke(look);
        }

        private void OnMoveHandler(InputAction.CallbackContext obj)
        {
            var move = obj.ReadValue<Vector2>();
            OnMove?.Invoke(move);
        }

        private void OnJumpDownHandler(InputAction.CallbackContext obj)
        {
            OnJumpDown?.Invoke();
        }
    }
}
