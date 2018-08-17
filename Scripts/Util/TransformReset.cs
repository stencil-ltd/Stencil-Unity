using UnityEngine;

namespace Util
{
    [RequireComponent(typeof(Transform))]
    public class TransformReset : MonoBehaviour
    {
        public bool UseWorldPosition;
        public bool UseWorldRotation;
        
        public Vector3 WorldPosition { get; private set; }
        public Vector3 LocalPosition { get; private set; }
        public Quaternion WorldRotation { get; private set; }
        public Quaternion LocalRotation { get; private set; }
        public Vector3 Scale { get; private set; }

        public Transform Transform;

        private void Start()
        {
            Mark();
        }

        public void Mark()
        {
            LocalPosition = Transform.localPosition;
            WorldPosition = Transform.position;
            LocalRotation = Transform.localRotation;
            WorldRotation = Transform.rotation;
            Scale = Transform.localScale;
        }

        public void ResetToMark()
        {
            if (UseWorldRotation)
                Transform.rotation = WorldRotation;
            else Transform.localRotation = LocalRotation;

            if (UseWorldPosition)
                Transform.position = WorldPosition;
            else Transform.localPosition = LocalPosition;

            Transform.localScale = Scale;
        }

        public void Translate(Vector3 translate)
        {
            var pos = UseWorldPosition ? WorldPosition : LocalPosition;
            pos += translate;
            if (UseWorldPosition)
                Transform.position = pos;
            else Transform.localPosition = pos;
        }
    }
}