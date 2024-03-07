namespace CharacterController3D
{
    using System;
    using UnityEngine;

    public class CharacterController3DInAirBehaviour : StateMachineBehaviour
    {
        public event Action OnInAirExit;
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            OnInAirExit?.Invoke();
        }
    }
}