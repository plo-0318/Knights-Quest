using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    public AudioReferences audioRefs;

    [Range(0.01f, 1f)]
    public float musicVolume = 0.1f;

    [Range(0.01f, 1f)]
    public float sfxVolume = 0.05f;

    private void Awake()
    {
        int soundManagerCounts = FindObjectsOfType<SoundManager>().Length;

        if (soundManagerCounts > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            GameManager.RegisterSoundManager(this);
            DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioRefs.musicMainMenu;
    }

    //TODO: UNCOMMENT THIS WHEN VOLUME UI IS IMPLEMENTED
    private void Start()
    {
        musicVolume = .5f;
        sfxVolume = .5f;

        // musicVolume = PlayerPrefsController.GetMusicVolume();

        audioSource.volume = musicVolume;
        audioSource.Play();

        // Invoke("TEST_ChangeMusic", 0f);
    }

    public AudioClip GetRandomActionClip()
    {
        List<AudioClip> clips = new List<AudioClip>();

        clips.Add(audioRefs.musicAction1);
        clips.Add(audioRefs.musicAction2);
        clips.Add(audioRefs.musicAction3);

        return clips[Random.Range(0, clips.Count)];
    }

    public void PlayMusic(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlayClip(AudioClip audioClip)
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, sfxVolume);
    }

    private void SetMusicVolume(float value)
    {
        musicVolume = value;
        audioSource.volume = musicVolume;
    }

    private void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }

    public void UpdateVolume()
    {
        SetMusicVolume(PlayerPrefsController.GetMusicVolume());
        SetSFXVolume(PlayerPrefsController.GetSFXVolume());
    }

    public void TEST_ChangeMusic()
    {
        PlayMusic(GetRandomActionClip());
    }
}
