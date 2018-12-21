using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Physic
{
    public class PositionTracker : MonoBehaviour
    {
        public int HistoryFrames = 20;

        public float Distance => Delta.magnitude;
        public Vector3 Delta => transform.position - _positions.Last.Value;
        public Vector3[] History => _positions.ToArray();

        private readonly LinkedList<Vector3> _positions = new LinkedList<Vector3>();
        private Vector3 _position;

        public Vector3 GetFrame(int index, bool lookback = false)
        {
            if (lookback)
            {
                var hist = History;
                index = hist.Length - (index + 1);
                if (index < 0) index = 0;
                return hist[index];
            }
            return _positions.First.Value;
        }

        private void Awake()
        {
            Tick();
        }

        private void FixedUpdate()
        {
            Tick();
        }

        private void Tick()
        {
            while (_positions.Count > HistoryFrames)
                _positions.RemoveFirst();

            _position = transform.position;
            _positions.AddLast(_position);
        }
    }
}