using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }

        private AudioSource _audioSource;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                _audioSource = GetComponent<AudioSource>();
                if (!_audioSource)
                {
                    _audioSource = gameObject.AddComponent<AudioSource>();
                }

                _audioSource.loop = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayMusic(AudioClip clip, float volume = 0.5f)
        {
            if (_audioSource.isPlaying)
            {
                return;
            }

            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.Play();
        }

        public void StopMusic()
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }

}
