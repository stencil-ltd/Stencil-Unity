using UnityEngine;

namespace Util
{
    public static class AnimExtensions
    {
        public static YieldInstruction WaitForFinish(this Animator anim)
        {
            return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length+anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
    }
}