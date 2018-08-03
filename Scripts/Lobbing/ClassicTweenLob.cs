using System.Collections;
using Plugins.Coroutines;
using UnityEngine;

namespace Lobbing
{
    public class ClassicTweenLob : ILobFunction
    {      
        public IEnumerator Lob(Lob lob, GameObject origin, GameObject target)
        {
            var obj = lob.Projectile;
            obj.transform.position = origin.transform.position;
            var lt = LeanTween.move(obj, target.transform, lob.Style.Duration);
            if (lob.Style.Elastic)
            {
                lt.setEaseInBack();
                lt.setScale(0.5f);
            }
            yield return new WaitForTween(lt);
        }
    }
}