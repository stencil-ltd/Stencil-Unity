using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dev;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Analytics
{
    public class Tracking : ITracker
    {
        public static readonly Tracking Instance = new Tracking();

        public bool Enabled => !Developers.Enabled;
//        public bool Enabled => true;

        private readonly ITracker[] _trackers = {
            new UnityTracking(),
        };

        public ITracker Track(string name, Dictionary<string, object> eventData = null)
        {
            if (!Enabled) return this;
            var json = eventData == null ? "[]" : string.Join(", ", eventData.ToList());
            Debug.LogWarning($"Track Event {name}\n{json}");
            foreach (var tracker in _trackers) tracker.Track(name, eventData);
            return this;
        }

        public ITracker SetUserProperty(string name, object value)
        {
            if (!Enabled) return this;
            foreach (var tracker in _trackers) tracker.SetUserProperty(name, value);
            return this;
        }
    }
}