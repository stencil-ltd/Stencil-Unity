using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Physic
{
    public class PositionTracker : MonoBehaviour
    {
        public int HistoryFrames = 20;

        public float Distance => Vector3.Distance(transform.position, _positions.Last.Value);
        public Vector3 Delta => transform.position - _positions.Last.Value;
        public Vector3[] History => _positions.ToArray();

        private readonly LinkedList<Vector3> _positions = new LinkedList<Vector3>();
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
        }

        private void FixedUpdate()
        {
            while (_positions.Count > HistoryFrames)
                _positions.RemoveFirst();
            
            _position = transform.position;
            _positions.AddLast(_position);
        }
    }
}