namespace Player
{
    using CharacterController3D;
    using InputController;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class Player : CharacterController3D
    {
        [SerializeField, BoxGroup(COMPONENTS)]
        private InputController inputController;
        [SerializeField, BoxGroup(COMPONENTS)]
        private CameraController cameraController;
        
        private void Awake()
        {
            // TEMP: Remove this line
            inputController.Initialize();
            
            Initialize();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            Move(cameraController.Right * HorizontalInput.x + cameraController.Forward * HorizontalInput.z);
        }

        protected override void LateUpdate()
        {
            cameraController.transform.position = transform.position;
            
            base.LateUpdate();
        }

        protected override void Initialize()
        {
            base.Initialize();
            inputController.OnMove += OnMoveHandler;
            inputController.OnJumpDown += OnJumpHandler;
        }
    }
}