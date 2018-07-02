using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Storage
{
    public struct PrefMetadata
    {
        public DateTime? Expiration;
        public DateTime? LastUpdate;

        public override string ToString()
        {
            return $"{nameof(Expiration)}: {Expiration}, {nameof(LastUpdate)}: {LastUpdate}";
        }

        public static PrefMetadata? FromDict(Dictionary<string, object> meta)
        {
            if (meta == null)
            {
                Debug.LogWarning("Could not read metadata dict.");
                return null;
            }

            DateTime? expire = null;
            if (meta.ContainsKey("expire"))
            {
                Debug.Log($"Expire = {meta["expire"]}");
//                expire = DateTime.FromBinary((long) meta["expire"]);
            }

            DateTime? update = null;
            if (meta.ContainsKey("last_update"))
            {
                Debug.Log($"LastUpdate = {meta["last_update"]}");
//                update = DateTime.FromBinary((long) meta["last_update"]);
            }
           
            return new PrefMetadata
            {
                Expiration = expire,
                LastUpdate = update
            };
        }
        
        [CanBeNull]
        public Dictionary<string, object> ToDict()
        {
            var retval = new Dictionary<string, object>();
            if (Expiration.HasValue)
                retval["expire"] = Expiration.Value.ToBinary();
            if (LastUpdate.HasValue)
                retval["last_update"] = LastUpdate.Value.ToBinary();
            return retval;
        }
    }
}