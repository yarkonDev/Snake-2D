using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Плейлист")]
    public AudioClip[] playlist;
    public AudioSource audioSource;

    [Header("Настройки")]
    public float fadeDuration = 1.5f;
    private float _targetVolume = 0.5f;

    private int _currentTrackIndex = -1;
    private bool _isMusicMuted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        _isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
    }

    void Start()
    {
        if (playlist == null || playlist.Length == 0) return;
        _currentTrackIndex = Random.Range(0, playlist.Length);

        if (!_isMusicMuted)
        {
            StartCoroutine(PlayTrackWithFade(_currentTrackIndex));
        }
    }

    void Update()
    {
        if (!_isMusicMuted && audioSource != null && !audioSource.isPlaying && _currentTrackIndex != -1)
        {
            PlayNextTrack();
        }
    }

    public void PlayNextTrack()
    {
        if (playlist == null || playlist.Length == 0) return;
        _currentTrackIndex = (_currentTrackIndex + 1) % playlist.Length;

        StopAllCoroutines();
        StartCoroutine(PlayTrackWithFade(_currentTrackIndex));
    }
    IEnumerator PlayTrackWithFade(int trackIndex)
    {
        if (audioSource == null || playlist == null || playlist[trackIndex] == null) yield break;
        if (audioSource.isPlaying)
        {
            float startVol = audioSource.volume;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVol * (Time.unscaledDeltaTime / fadeDuration);
                yield return null;
            }
        }
        audioSource.clip = playlist[trackIndex];
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < _targetVolume)
        {
            audioSource.volume += _targetVolume * (Time.unscaledDeltaTime / fadeDuration);
            yield return null;
        }
        audioSource.volume = _targetVolume;
    }
    public void ToggleMute()
    {
        _isMusicMuted = !_isMusicMuted;
        PlayerPrefs.SetInt("MusicMuted", _isMusicMuted ? 1 : 0);
        PlayerPrefs.Save();

        if (_isMusicMuted)
        {
            StopAllCoroutines();
            audioSource.Stop();
        }
        else
        {
            PlayNextTrack();
        }
    }
}