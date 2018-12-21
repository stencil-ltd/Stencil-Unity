using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Physic
{
    public class PositionTracker : MonoBehaviour
    {
        public int HistoryFrames = 20;

        public float Distance => Delta.magnitude;
        public Vector3 Delta => _position - _lastPosition;
        public Vector3[] History => _positions.ToArray();

        private readonly LinkedList<Vector3> _positions = new LinkedList<Vector3>();

        private Vector3 _lastPosition;
        private Vector3 _position;

        public Vector3? GetFrame(int index, bool lookback = false)
        {
            if (lookback)
            {
                var hist = History;
                index = hist.Length - (index + 1);
                if (index < 0) index = 0;
                if (index < hist.Length)
                    return hist[index];
            }
            return _positions.FirstOrDefault();
        }
        
        private void OnEnable()
        {
            _position = transform.position;
            _lastPosition = _position;
        }

        private void Update()
        {
            while (_positions.Count > HistoryFrames)
                _positions.RemoveFirst();

            _lastPosition = _position;
            _position = transform.position;
            _positions.AddLast(_position);
        }
    }
}