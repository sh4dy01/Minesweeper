using UnityEngine;

namespace Managers
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _lobbyMusic;
        [SerializeField] private AudioClip _gameMusic;
        [SerializeField] private AudioClip _alarmMusic;
        
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

        private void ChangeMusic(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        public void SetLobbyMusic() => ChangeMusic(_lobbyMusic);
        public void SetGameMusic() => ChangeMusic(_gameMusic);
        public void SetAlarmMusic() => ChangeMusic(_alarmMusic);
    }
}
