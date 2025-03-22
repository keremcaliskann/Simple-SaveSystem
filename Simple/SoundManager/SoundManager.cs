using UnityEngine;

namespace Simple
{
    public enum SoundType { Example }

    [DefaultExecutionOrder(-2)]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        public bool On = true;
        public float MultiplayThrehold = 0.1f;

        [SerializeField] private Sound[] _sounds;

        private AudioSource _audioSource;

        private void OnValidate()
        {
            if (_sounds == null)
                return;
            if (_sounds.Length <= 0)
                return;

            for (int i = 0; i < _sounds.Length; i++)
            {
                _sounds[i].Name = _sounds[i].Type.ToString();
            }
        }

        [System.Serializable]
        public class Sound
        {
            [HideInInspector] public string Name;
            [field: SerializeField] public SoundType Type { get; private set; }
            [Range(0, 3)]
            [SerializeField] public float Volume = 1;
            [SerializeField] private AudioClip[] _clips;

            [HideInInspector] public float LastPlay;
            private int _clipIndex;

            public AudioClip GetClip()
            {
                _clipIndex++;
                if (_clipIndex >= _clips.Length)
                    _clipIndex = 0;

                return _clips[_clipIndex];
            }
        }

        private void Awake()
        {
            Instance = this;

            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }

        public void PlaySound(SoundType sound)
        {
            if (!On)
                return;

            Sound s = GetSound(sound);
            if (s == null)
            {
                Debug.LogWarning(sound + " is not attached to SoundManager.");
                return;
            }
            float currentTime = Time.time;
            if ((currentTime - s.LastPlay) < MultiplayThrehold)
                return;

            s.LastPlay = currentTime;
            _audioSource.PlayOneShot(s.GetClip(), s.Volume);
        }

        public Sound GetSound(SoundType type)
        {
            for (int i = 0; i < _sounds.Length; i++)
            {
                if (_sounds[i].Type == type)
                    return _sounds[i];
            }
            return null;
        }
    }
}