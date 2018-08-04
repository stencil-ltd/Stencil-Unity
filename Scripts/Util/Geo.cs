using UI;
using UnityEngine;

namespace Util
{
    public static class Geo
    {
        public static void CastIntoUi(this Transform transform)
        {
            var mask = LayerMask.GetMask("UI");
            var pos = transform.position;
            var cam = Camera.main.transform.position;
            var ray = new Ray(pos, cam - pos);
            RaycastHit hit;
            var success = Physics.Raycast(ray, out hit, float.MaxValue, mask);
            if (success)
                transform.position = hit.point;
        }
    }
}