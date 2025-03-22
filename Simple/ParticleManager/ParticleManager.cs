using System.Collections.Generic;
using UnityEngine;

namespace Simple
{
    public enum ParticleType { Example }

    [DefaultExecutionOrder(-2)]
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance { get; private set; }

        [SerializeField] private ParticleClass[] _particleClasses;

        private void Awake()
        {
            Instance = this;
        }

        private void OnValidate()
        {
            if (_particleClasses == null)
                return;
            if (_particleClasses.Length <= 0)
                return;

            for (int i = 0; i < _particleClasses.Length; i++)
            {
                _particleClasses[i].name = _particleClasses[i].particleType.ToString();
            }
        }

        private void Start()
        {
            for (int i = 0; i < _particleClasses.Length; i++)
            {
                _particleClasses[i].Init(transform);
            }
        }

        public void PlayParticle(ParticleType particleType, Vector3 worldPosition, Transform parent = null)
        {
            ParticleSystem particle = GetParticleClass(particleType).GetParticle();
            if (particle == null)
            {
                Debug.LogWarning(particleType + " is not attached to ParticleManager.");
                return;
            }
            particle.transform.position = worldPosition;
            particle.Play();
            if (parent != null)
                particle.transform.parent = parent;
        }

        public void PlayParticle(ParticleType particleType, Vector3 worldPosition, Quaternion worldRotation, Transform parent = null)
        {
            ParticleSystem particle = GetParticleClass(particleType).GetParticle();
            if (particle == null)
            {
                Debug.LogWarning(particleType + " is not attached to ParticleManager.");
                return;
            }
            particle.transform.position = worldPosition;
            particle.transform.rotation = worldRotation;
            particle.Play();
            if (parent != null)
                particle.transform.parent = parent;
        }

        private ParticleClass GetParticleClass(ParticleType particleType)
        {
            for (int i = 0; i < _particleClasses.Length; i++)
            {
                if (_particleClasses[i].particleType == particleType)
                {
                    return _particleClasses[i];
                }
            }
            return null;
        }

        [System.Serializable]
        public class ParticleClass
        {
            [HideInInspector]
            public string name;
            public ParticleType particleType;
            public ParticleSystem[] particlePrefabs;
            [Min(1)] public int poolCount = 1;
            [HideInInspector]
            public List<ParticleSystem> particles;
            private int _index = 0;

            public void Init(Transform parent)
            {
                for (int i = 0; i < poolCount; i++)
                {
                    for (int j = 0; j < particlePrefabs.Length; j++)
                    {
                        particles.Add(Instantiate(particlePrefabs[j], parent));
                    }
                }
            }

            public ParticleSystem GetParticle()
            {
                if (particles.Count <= 0)
                    return null;

                ParticleSystem p = particles[_index];
                _index++;
                if (_index >= particles.Count)
                    _index = 0;
                return p;
            }
        }
    }
}