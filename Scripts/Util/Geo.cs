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
            var ray = new Ray(cam, cam - pos);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, float.MaxValue, mask);
            transform.position = hit.point;
        }
    }
}