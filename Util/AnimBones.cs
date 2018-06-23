using UnityEngine;

namespace Util
{
    public static class AnimBones
    {
        public static Transform Head(this Animator anim)
            => anim.GetBoneTransform(HumanBodyBones.Head);

        public static Transform Chest(this Animator anim)
            => anim.GetBoneTransform(HumanBodyBones.Chest);
        
        public static Transform Hips(this Animator anim)
            => anim.GetBoneTransform(HumanBodyBones.Hips);
        
        public static Transform RightHand(this Animator anim) 
            => anim.GetBoneTransform(HumanBodyBones.RightHand);
        
        public static Transform LeftHand(this Animator anim) 
            => anim.GetBoneTransform(HumanBodyBones.LeftHand);

        public static Transform LeftFoot(this Animator anim) 
            => anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        
        public static Transform RightFoot(this Animator anim) 
            => anim.GetBoneTransform(HumanBodyBones.RightFoot);
    }
}