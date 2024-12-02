using UnityEngine;

namespace Characters
{
    public class AnimatorProperties
    {
        public static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public static readonly int SpeedModifier = Animator.StringToHash("SpeedModifier");
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        public static readonly int IsPunching = Animator.StringToHash("IsPunching");
        
        
        public static readonly int IdleTrigger = Animator.StringToHash("Idle");
        public static readonly int IsJumping = Animator.StringToHash("IsJumping");
        public static readonly int PunchTrigger = Animator.StringToHash("Punch");
        public static readonly int WalkTrigger = Animator.StringToHash("Walk");
        public static readonly int IsAiming = Animator.StringToHash("IsAiming");
        public static readonly int Shoot = Animator.StringToHash("Shoot");
    }
}