using UnityEngine;

namespace Coroutines
{
    public class WaitForAnim : CustomYieldInstruction
    {
        public readonly Animator Animator;

        public override bool keepWaiting
        {
            get
            {
                var info = Animator.GetCurrentAnimatorStateInfo(0);
                var trans = Animator.IsInTransition(0);
                var beginOrEnd = info.normalizedTime == 1f || info.normalizedTime == 0f;
                return beginOrEnd || trans;
            }
        }

        public WaitForAnim(Animator animator)
        {
            Animator = animator;
        }
    }
}
