using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Physic
{
    public class VelocityTracker : MonoBehaviour
    {
        public int VelocityHistoryFrames = 20;

        public Vector3 Velocity { get; private set; }
        public float MagnitudeRough { get; private set; }
        
        public Vector3[] History => _velocities.ToArray();

        private readonly LinkedList<Vector3> _velocities = new LinkedList<Vector3>();
        private Vector3 _position;

        private void OnEnable()
        {
            _position = transform.position;
        }

        private void LateUpdate()
        {
            while (_velocities.Count > VelocityHistoryFrames)
                _velocities.RemoveFirst();
            
            var old = _position;
            _position = transform.position;
            var velocity = (_position - old) / Time.deltaTime;
            
            _velocities.AddLast(velocity);

            Velocity = Vector3.zero;
            MagnitudeRough = 0f;
            foreach (var v in _velocities)
            {
                Velocity += v;
                MagnitudeRough += v.sqrMagnitude;
            }

            Velocity /= _velocities.Count;
            MagnitudeRough = Mathf.Sqrt(MagnitudeRough / _velocities.Count);
        }
    }
}