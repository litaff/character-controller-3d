namespace Player
{
    using InputController;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        #region Odin Attributes

        private const string SETTINGS = "Settings";
        private const string COMPONENTS = "Components";

        #endregion
        
        [SerializeField, BoxGroup(COMPONENTS)]
        private InputController inputController;
        [SerializeField, BoxGroup(SETTINGS)]
        private Transform horizontalRotationRoot;
        [SerializeField, BoxGroup(SETTINGS)]
        private Transform verticalRotationRoot;
        [SerializeField, BoxGroup(SETTINGS)]
        private float rotationSpeed;
        [SerializeField, BoxGroup(SETTINGS)]
        private float maxVerticalAngle;
        [SerializeField, BoxGroup(SETTINGS)]
        private float minVerticalAngle;
        
        
        public Vector3 Right => horizontalRotationRoot.right;
        public Vector3 Forward => horizontalRotationRoot.forward;

        private void Awake()
        {
            inputController.OnLook += OnLookHandler;
        }

        private void OnLookHandler(Vector2 obj)
        {
            horizontalRotationRoot.Rotate(Vector3.up, obj.x * rotationSpeed);
            var eulerAnglesX = verticalRotationRoot.eulerAngles.x - obj.y *  rotationSpeed;
            if (eulerAnglesX < maxVerticalAngle && eulerAnglesX > minVerticalAngle)
            {
                verticalRotationRoot.Rotate(Vector3.right, -obj.y * rotationSpeed);
            }
        }
    }
}