using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace UI
{
    public class Frame : MonoBehaviour
    {
        public static float TopSafePadding { get; private set; }
        
        [CanBeNull] public RectMask2D Mask;
        public RectTransform Contents;

        private int lockCount;
        private EventSystem eventSystem;

        private void Awake()
        {
            eventSystem = EventSystem.current;
            ApplyNotch();
        }

        void Start()
        {
            if(Mask != null) Mask.enabled = true;
        }

        public void Lock()
        {
            lockCount++;
            _SetLocked(true);
        }

        private void ApplyNotch()
        {
            var safe = Screen.safeArea;
            
#if UNITY_IOS
            if (Device.generation == DeviceGeneration.iPhoneX 
                && safe.width.IsAbout(Screen.width) 
                && safe.height.IsAbout(Screen.height))
            {
                safe.yMax -= 150;
            }
#endif
            
            TopSafePadding = Screen.height - safe.yMax;
            var offsetMin = new Vector2Int((int) safe.xMin, (int) safe.yMin);
            var offsetMax = new Vector2Int((int) (Screen.width - safe.xMax), (int) (Screen.height - safe.yMax));
            Contents.offsetMin += offsetMin;
            Contents.offsetMax -= offsetMax;
        }

        private void _SetLocked(bool locked)
        {
            eventSystem.enabled = !locked;
        }

        public void Unlock()
        {
            if (--lockCount == 0) _SetLocked(false);
        }
    }
}