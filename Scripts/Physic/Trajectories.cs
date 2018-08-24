using System;
using UnityEngine;

namespace Physic
{
    public static class Trajectories
    {
        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        
        private static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
        
        // https://www.haverford.k12.pa.us/cms/lib5/PA01001043/Centricity/Domain/518/Finding%20a%20Quadratic%20Equation%20with%20the%20x%20intercepts%20and%20Another%20Point.pdf
        public static float ParabolicYByHeight(float x, float length, float height)
        {
            var mid = length * 0.5f;
            var a = height / (mid * (mid - length));
            return a * x * (x - length);
        }
        
        public static float ParabolicYByNorm(float norm, float length, float angle)
        {
            return ParabolicY(norm * length, length, angle);
        }

        public static float ParabolicY(float x, float length, float angle)
        {
            return ParabolicYByHeight(x, length, FindHeight(length, angle));
        }

        private static float FindHeight(float length, float theta)
        {
            var rads = DegreeToRadian(theta);
            return (float) (Math.Tan(rads) * length);
        }
        
        // https://tech.spaceapegames.com/2016/07/05/trajectory-prediction-with-unity-physics
        public static Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
        {
            var results = new Vector2[steps];
            var timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
            var gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;
            var drag = 1f - timestep * rigidbody.drag;
            var moveStep = velocity * timestep;
            for (var i = 0; i < steps; ++i)
            {
                moveStep += gravityAccel;
                moveStep *= drag;
                pos += moveStep;
                results[i] = pos;
            }
            return results;
        }
        
        // Modified from above blog post.
        public static Vector3[] Plot(Rigidbody rigidbody, Vector3 pos, Vector3 velocity, int steps)
        {
            var results = new Vector3[steps];
            var timestep = Time.fixedDeltaTime / UnityEngine.Physics.defaultSolverVelocityIterations;
            var gravityAccel = UnityEngine.Physics.gravity * timestep * timestep;
            var drag = 1f - timestep * rigidbody.drag;
            var moveStep = velocity * timestep;
            for (var i = 0; i < steps; ++i)
            {
                moveStep += gravityAccel;
                moveStep *= drag;
                pos += moveStep;
                results[i] = pos;
            }
            return results;
        }
    }
}