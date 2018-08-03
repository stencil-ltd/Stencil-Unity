using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Lobbing
{
    [Serializable]
    public class LobDivision
    {
        [Range(0f, 1f)] 
        public float AmountPerLob = 0.1f;
        [Range(0f, 1f)] 
        public float RandomizeAmount = 0.5f;
        
        public float Interval = 0.2f;
        public float RandomizeInterval = 0f;

        public float RandomizeDuration = 0f;
    }
    
    [Serializable]
    public class LobEvent : UnityEvent<Lob>
    {}

    public class Lobber : MonoBehaviour
    {
        [Header("Objects")] 
        public GameObject Prefab;
        public Transform From;
        public Transform To;

        [Header("Style")]
        public LobStyle Flight;
        public LobDivision Division;

        [Header("Events")] 
        public LobEvent OnLobBegan;
        public LobEvent OnLobEnded;

        public ILobFunction Function = new ClassicTweenLob();

        public IEnumerator LobSingle(long amount, bool applyRandomization = false)
        {
            var obj = Instantiate(Prefab, To.parent);
            obj.transform.position = From.position;
            
            var style = Flight;
            if (applyRandomization)
            {
                style.Duration += Random.Range(-Division.RandomizeDuration, Division.RandomizeDuration);
            }
            
            var lob = new Lob(obj, amount, style);
            OnLobBegan?.Invoke(lob);
            yield return StartCoroutine(Function.Lob(lob, From, To));
            OnLobEnded?.Invoke(lob);
            Destroy(obj);
        }

        public IEnumerator LobMany(long amount)
        {
            var routines = new List<Coroutine>();
            var remaining = amount;
            while (remaining > 0L)
            {
                var div = Division.AmountPerLob + Random.Range(-Division.RandomizeAmount, Division.RandomizeAmount);
                var single = (long) (div * amount);
                if (single < 0) continue;
                if (single > remaining) single = remaining;
                remaining -= single;
                routines.Add(StartCoroutine(LobSingle(single, true)));
                if (remaining > 0)
                {
                    var interval = Division.Interval +
                                   Random.Range(-Division.RandomizeInterval, Division.RandomizeInterval);
                    yield return new WaitForSeconds(interval);
                }
            }

            foreach (var r in routines)
                yield return r;
        }
    }
}