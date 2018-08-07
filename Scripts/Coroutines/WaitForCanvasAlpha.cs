using System;
using UnityEngine;

namespace Coroutines
{
    public class WaitForCanvasAlpha : CustomYieldInstruction
    {
        public readonly float Alpha;
        public readonly CanvasRenderer Renderer;

        public override bool keepWaiting => Math.Abs(Renderer.GetColor().a - Alpha) > 0.01f;    

        public WaitForCanvasAlpha(CanvasRenderer render, float alpha)
        {
            Alpha = alpha;
            Renderer = render;
        }
    }
}