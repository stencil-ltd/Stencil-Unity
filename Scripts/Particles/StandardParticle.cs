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
        
        public DoOnFinish OnFinish = DoOnFinish.Destroy;

        [Bind]
        public ParticleSystem ParticleSystem { get; set; }
        private ParticleSystem[] _particles;

        public void SetColor(Color color)
        {
            foreach (var particle in _particles)
            {
                var main = particle.main;
                main.startColor = color;
            }
        }

        private void Awake()
        {
            this.Bind();
            _particles = GetComponentsInChildren<ParticleSystem>();
        }

        private void Start()
        {
            var top = _particles.FirstOrDefault();
            foreach (var particle in _particles)
            {
                particle.GetComponent<Renderer>().sortingLayerName = "Particles";
                var main = particle.main;
                main.startColor = top.main.startColor;
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
                }
            }
        }
    }
}