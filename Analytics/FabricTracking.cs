﻿#if STENCIL_FABRIC
using System.Collections.Generic;
using Analytics;
using Fabric.Answers;
using Fabric.Crashlytics;
using Fabric.Internal.ThirdParty.MiniJSON;
using Facebook.MiniJSON;

namespace Util.Analytics
{
    public class FabricTracking : ITracker
    {
        public FabricTracking()
        {
            
        }

        public void Track(string name, Dictionary<string, object> eventData)
        {
            Answers.LogCustom(name, eventData);
            Crashlytics.Log($"{name}: {Json.Serialize(eventData)}");
        }

        public void SetUserProperty(string name, object value)
        {
            Crashlytics.Log($"Set Property {name} = {value}");
            Crashlytics.SetKeyValue(name, value?.ToString());
        }
    }
}
#endif