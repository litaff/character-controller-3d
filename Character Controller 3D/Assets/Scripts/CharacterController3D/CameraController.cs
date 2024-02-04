namespace CharacterController3D
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        #region Odin Attributes

        private const string SETTINGS = "Settings";

        #endregion
        
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

        private void LateUpdate()
        {
            var horizontalMouse = Input.GetAxis("Mouse X");
            var verticalMouse= Input.GetAxis("Mouse Y");

            horizontalRotationRoot.Rotate(Vector3.up, horizontalMouse * rotationSpeed);
            var eulerAnglesX = verticalRotationRoot.eulerAngles.x - verticalMouse *  rotationSpeed;
            if (eulerAnglesX < maxVerticalAngle && eulerAnglesX > minVerticalAngle)
            {
                verticalRotationRoot.Rotate(Vector3.right, -verticalMouse * rotationSpeed);
            }
        }
    }
}