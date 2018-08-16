using UnityEngine;

namespace Physic
{
    public static class Trajectories
    {
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