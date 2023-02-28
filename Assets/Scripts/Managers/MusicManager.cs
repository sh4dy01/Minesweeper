using UnityEngine;

namespace Managers
{
    public class MusicManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        
        #region Singleton
        public static MusicManager Instance { get; private set; }
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                Instance._audioSource = GetComponent<AudioSource>();
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
        #endregion

        public void ChangeMusic(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }
    }
}
