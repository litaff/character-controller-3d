namespace CharacterController3D
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class IKController : MonoBehaviour
    {
        protected Animator animator;

        public bool ikActive = false;
        public Transform rightHandObj = null;
        public Transform lookObj = null;
        [Range(0, 1)]
        public float weight;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        //a callback for calculating IK
        private void OnAnimatorIK()
        {
            if (animator)
            {

                //if the IK is active, set the position and rotation directly to the goal.
                if (ikActive)
                {

                    /*// Set the look target position, if one has been assigned
                    if (lookObj != null)
                    {
                        animator.SetLookAtWeight(1);
                        animator.SetLookAtPosition(lookObj.position);
                    }*/

                    // Set the right hand target position and rotation, if one has been assigned
                    if (rightHandObj != null)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                        
                    }

                }

                //if the IK is not active, set the position and rotation of the hand and head back to the original position
                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                    animator.SetLookAtWeight(0);
                }
            }
        }
    }
}
