using UnityEngine;

namespace Coroutines
{
    public class WaitForAnim : CustomYieldInstruction
    {
        public readonly Animator Animator;
        public readonly string State;

        public override bool keepWaiting
        {
            get
            {
                var info = Animator.GetCurrentAnimatorStateInfo(0);
                var isAtState = info.IsName(State);
                var trans = Animator.IsInTransition(0);
                return !isAtState || trans;
            }
        }

        public WaitForAnim(Animator animator, string state)
        {
            Animator = animator;
            State = state;
        }
    }
}
