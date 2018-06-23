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

        private Transform _transform;

        private void Awake()
        {
            this.Bind(ref _transform);
        }

        private void Start()
        {
            Mark();
        }

        public void Mark()
        {
            LocalPosition = _transform.localPosition;
            WorldPosition = _transform.position;
            LocalRotation = _transform.localRotation;
            WorldRotation = _transform.rotation;
            Scale = _transform.localScale;
        }

        public void ResetToMark()
        {
            if (UseWorldRotation)
                _transform.rotation = WorldRotation;
            else _transform.localRotation = LocalRotation;

            if (UseWorldPosition)
                _transform.position = WorldPosition;
            else _transform.localPosition = LocalPosition;

            _transform.localScale = Scale;
        }

        public void Translate(Vector3 translate)
        {
            var pos = UseWorldPosition ? WorldPosition : LocalPosition;
            pos += translate;
            if (UseWorldPosition)
                _transform.position = pos;
            else _transform.localPosition = pos;
        }
    }
}