namespace CharacterController3D
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class CharacterController3D : MonoBehaviour
    {
        #region Odin Attributes

        private const string COMPONENTS = "Components";
        private const string SETTINGS = "Settings";

        #endregion
        
        [SerializeField, BoxGroup(COMPONENTS)]
        private Rigidbody rigidbody;
        [SerializeField, BoxGroup(COMPONENTS)]
        private CameraController cameraController;
        [SerializeField, BoxGroup(SETTINGS)]
        private float moveSpeed;
        [SerializeField, BoxGroup(SETTINGS)]
        private float lookSpeed;

        private void FixedUpdate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var moveDirection = cameraController.Right * horizontal + cameraController.Forward * vertical;
            moveDirection.Normalize();
            
            if (moveDirection != Vector3.zero)
            {
                var position = transform.position;
                transform.LookAt(Vector3.Lerp(position + transform.forward,
                    position + moveDirection, lookSpeed));
            }
            
            rigidbody.velocity = moveDirection * moveSpeed;
        }

        private void LateUpdate()
        {
            cameraController.transform.position = transform.position;
        }
    }
}
