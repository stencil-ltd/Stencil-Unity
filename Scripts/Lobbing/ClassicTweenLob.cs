using System.Collections;
using Plugins.Coroutines;
using UnityEngine;

namespace Lobbing
{
    public class ClassicTweenLob : ILobFunction
    {      
        public IEnumerator Lob(Lob lob, Transform origin, Transform target)
        {
            var obj = lob.Projectile;
            obj.transform.position = origin.position;
            var lt = LeanTween.move(obj, target, lob.Style.Duration);
            if (lob.Style.Elastic)
            {
                lt.setEaseInBack();
                lt.setScale(0.5f);
            }
            yield return new WaitForTween(lt);
        }
    }
}