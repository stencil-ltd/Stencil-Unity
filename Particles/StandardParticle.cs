using System;
using System.Linq;
using Binding;
using UnityEngine;

namespace Particles
{
    public class StandardParticle : MonoBehaviour
    {
        public enum DoOnFinish
        {
            Nothing,
            Deactivate,
            Destroy
        }
        
        [Obsolete]
        public bool DestroyOnKill = true;
        public DoOnFinish OnFinish = DoOnFinish.Destroy;

        [Bind]
        public ParticleSystem ParticleSystem;
        private ParticleSystem[] _particles;

        public void SetColor(Color color)
        {
            foreach (var particle in _particles)
            {
                particle.startColor = color;
            }
        }

        private void Awake()
        {
            this.Bind();
            _particles = GetComponentsInChildren<ParticleSystem>();
            if (OnFinish == DoOnFinish.Destroy && DestroyOnKill == false)
                OnFinish = DoOnFinish.Nothing;
        }

        private void Start()
        {
            var top = _particles.FirstOrDefault();
            foreach (var particle in _particles)
            {
                particle.GetComponent<Renderer>().sortingLayerName = "Particles";
                particle.startColor = top.startColor;
            }
        }

        private void Update()
        {
            if (ParticleSystem.isStopped)
            {
                switch (OnFinish)
                {
                    case DoOnFinish.Deactivate:
                        gameObject.SetActive(false);
                        break;
                    case DoOnFinish.Destroy:
                        Destroy(gameObject);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}