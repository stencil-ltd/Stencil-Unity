using UnityEngine;

namespace Transforms
{
    
    public class TransformHierarchy : MonoBehaviour
    {
        public bool TranslateSelf;
        public GameObject OptionalPrefab;
        
        public Transform Translator { get; private set; }
        public Transform Scaler { get; private set; }
        public Transform Rotator { get; private set; }
        public Transform Subject { get; private set; }

        private void Awake()
        {
            Configure();
        }

        public void Configure()
        {
            Translator = TranslateSelf ? transform : transform.GetChild(0);
            Scaler = Translator.GetChild(0);
            Rotator = Scaler.GetChild(0);

            if (Rotator.childCount > 0)
                Subject = Rotator.GetChild(0);
            if (OptionalPrefab)
            {
                if (Subject != null)
                    DestroyImmediate(Subject);
                Subject = Instantiate(OptionalPrefab, Rotator).transform;
                Subject.transform.localPosition = Vector3.zero;
                Subject.transform.localRotation = Quaternion.identity;
                Subject.transform.localScale = Vector3.one;
            }
        }

        public void Copy(TransformHierarchy other)
        {
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            transform.position = other.transform.position;
            
            Translator.position = other.Translator.position;
            
            Scaler.localScale = Vector3.Scale(other.Scaler.localScale, other.transform.localScale);
            
            Rotator.rotation = other.Rotator.rotation;

            Subject.localPosition = other.Subject.localPosition;
            Subject.localScale = other.Subject.localScale;
            Subject.localRotation = other.Subject.localRotation;
        }
    }
}