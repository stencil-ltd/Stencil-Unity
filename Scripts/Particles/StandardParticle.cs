using System.Linq;
using Binding;
using Standard.States;
using State;
using UnityEngine;
using UnityEngine.Playables;

namespace Particles
{
    [RequireComponent(typeof(ParticleSystem))]
    [ExecuteInEditMode]
    public class StandardParticle : MonoBehaviour
    {
        public enum DoOnFinish
        {
            Nothing,
            Deactivate,
            Destroy
        }
        
        public DoOnFinish OnFinish = DoOnFinish.Destroy;
        public PlayStates.State[] ActiveStates = { };
        
        [Bind]
        public ParticleSystem ParticleSystem { get; set; }
        private ParticleSystem[] _particles;

        private void Awake()
        {
            this.Bind();
            _particles = GetComponentsInChildren<ParticleSystem>();
            if (Application.isPlaying && ActiveStates.Length > 0)
            {
                PlayStates.Instance.OnChange += OnState;
                OnState(null, new StateChange<PlayStates.State>(null, PlayStates.Instance.State));
            }
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
                PlayStates.Instance.OnChange -= OnState;
        }

        private void OnState(object sender, StateChange<PlayStates.State> e)
        {
            gameObject.SetActive(ActiveStates.Contains(e.New));   
        }

        public void SetColor(Color color)
        {
            foreach (var particle in _particles)
            {
                var main = particle.main;
                main.startColor = color;
            }
        }

        private void Start()
        {
            var top = _particles.FirstOrDefault();
            foreach (var particle in _particles)
            {
                particle.GetComponent<Renderer>().sortingLayerName = "Particles";
                var main = particle.main;
                if (Application.isPlaying)
                    main.startColor = top.main.startColor;
            }
        }

        private void Update()
        {
            if (!Application.isPlaying) return;
            if (ParticleSystem.isStopped)
                PerformFinish();
        }

        private void OnDisable()
        {
            PerformFinish();
        }

        private void PerformFinish()
        {
            if (!Application.isPlaying) return;
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