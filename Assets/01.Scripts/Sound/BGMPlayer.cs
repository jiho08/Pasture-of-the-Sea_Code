using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Sound
{
    public class BGMPlayer : MonoBehaviour
    {
        public AudioClip[] bgmClips;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = false;
        }

        private void Start()
        {
            PlayRandomBGM();
        }

        private void Update()
        {
            if (!_audioSource.isPlaying)
                PlayRandomBGM();
        }
        
        private void PlayRandomBGM()
        {
            if (bgmClips.Length == 0)
                return;

            var index = Random.Range(0, bgmClips.Length);
            _audioSource.clip = bgmClips[index];
            _audioSource.Play();
        }
    }
}