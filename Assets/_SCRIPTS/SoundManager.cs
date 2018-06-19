using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Audio;

public enum ambienceIndex
{
    birds,
    wind,
    night,
    spooky
}
public enum songIndex
{
    daySong,
    nightSong,
    latenightSong
}
public class SoundManager : ManagerBase
{
    public static SoundManager Instance;
    public AudioMixerSnapshot songUp, songDown;

    public AudioSource song, ambience, gameover;
    //dynamically added AudioSources
    public List<AudioSource> dynamicSources;
    public List<AudioClip> songClips, ambienceClips;

    [SerializeField]
    AudioClip about_to_die;



    public AudioClip[] footsteps;

    [Header("Radio")]
    public AudioSource radio;
    public AudioClip radioBeginning;
    public AudioClip radioEnd;
    public AudioClip radioDie;
    public AudioClip breathe;
    public float radioBeginningDelay;
    public float radioEndDelay;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameManager.Instance.DeathByEnemy += DieSound;
    }

    void DieSound()
    {
        radio.PlayOneShot(radioDie);
        PlayOneShot(gameover, about_to_die, 1);
        PlayOneShot(gameover, breathe, 10);
    }

    protected override void OnPhaseLoad(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Afternoon:
                // Play radio intro
                PlayOneShot(radio, radioBeginning, radioBeginningDelay);

                AudioSource a = newDynamicSource();
                dynamicSources.Add(a);
                a.clip = ambienceClips[(int)ambienceIndex.birds];
                a.enabled = true;
                break;
            case GamePhase.Dusk:
                //melody grows quieter, pitch lowers
                changePitch(song, song.pitch, 0.5f, 1);
                changeVolume(song, song.volume, song.volume / 2, 3, () =>
                {
                    dynamicSources[0].enabled = false;
                    dynamicSources[0].clip = ambienceClips[(int)ambienceIndex.wind];
                    dynamicSources[0].enabled = true;
                });
                break;
            case GamePhase.Night:
                //melody pitch goes to negative
                changePitch(song, song.pitch, -0.5f, 1,
                    () => changePitchWait(song, -0.5f, -0.6f, 1, 5f,
                        () => changePitchWait(song, -0.6f, -0.75f, 1, 1.6f, () =>
                        {
                            changeSong(songIndex.nightSong, 3);
                            ambience.enabled = false;
                            ambience.clip = ambienceClips[(int)ambienceIndex.night];
                            ambience.enabled = true;
                        })));

                break;
            case GamePhase.Latenight:
                //change melody
                changeSong(songIndex.latenightSong, 2);
                changeVolume(song, song.volume, song.volume - 0.1f, 3, () =>
                {
                    ambience.enabled = false;
                    ambience.clip = ambienceClips[(int)ambienceIndex.spooky];
                    ambience.enabled = true;
                });
                break;
            case GamePhase.Dawn:
                changeSong(songIndex.daySong, 1.5f);
                changeVolume(dynamicSources[0], dynamicSources[0].volume, 0, 6, () =>
                {
                    AudioSource a2 = dynamicSources[0];
                    dynamicSources.RemoveAt(0);
                    Destroy(a2);
                });
                break;
            case GamePhase.End:
                // Play radio end
                PlayOneShot(radio, radioEnd, radioEndDelay);
                break;
        }
    }

    void PlayOneShot(AudioSource source, AudioClip clip, float delay)
    {
        StartCoroutine(_PlayOneShot(source, clip, delay));
    }

    IEnumerator _PlayOneShot(AudioSource source, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.PlayOneShot(clip);
    }

    void changeSong(songIndex si, float wait)
    {
        StartCoroutine(changeSongTime(si, wait));
    }

    IEnumerator changeSongTime(songIndex si, float wait)
    {
        songDown.TransitionTo(wait);
        yield return new WaitForSeconds(wait);
        song.enabled = false;
        song.clip = songClips[(int)si];
        songUp.TransitionTo(wait);
    }

    AudioSource newDynamicSource()
    {
        AudioSource a = ambience.gameObject.AddComponent<AudioSource>();
        a.loop = true;
        a.playOnAwake = true;
        a.spread = 180;
        a.spatialBlend = 1;
        a.enabled = false;
        a.volume = 0.1f;
        return a;
    }

    void changePitchWait(AudioSource a, float one, float two, float maxTime, float wait, Action action = null)
    {
        StartCoroutine(changePitchTimeWait(a, one, two, maxTime, wait, action));
    }

    IEnumerator changePitchTimeWait(AudioSource a, float one, float two, float maxTime, float wait, Action action)
    {
        yield return new WaitForSeconds(wait);
        StartCoroutine(changePitchTime(a, one, two, maxTime, action));

    }

    void changePitch(AudioSource a, float one, float two, float maxTime, Action action = null)
    {
        StartCoroutine(changePitchTime(a, one, two, maxTime, action));
    }

    IEnumerator changePitchTime(AudioSource a, float one, float two, float maxTime, Action action)
    {
        yield return null;
        for (float i = 0; i < maxTime && isActiveAndEnabled; i += Time.deltaTime)
        {
            a.pitch = Mathf.Lerp(one, two, i / maxTime);
            yield return null;
        }
        a.pitch = two;
        if (action != null) action();
    }

    void changeVolume(AudioSource a, float one, float two, float maxTime, Action action = null)
    {
        StartCoroutine(changeVolumeTime(a, one, two, maxTime, action));
    }

    IEnumerator changeVolumeTime(AudioSource a, float one, float two, float maxTime, Action action)
    {
        yield return null;
        for (float i = 0; i < maxTime && isActiveAndEnabled; i += Time.deltaTime)
        {
            a.volume = Mathf.Lerp(one, two, i / maxTime);
            yield return null;
        }
        a.volume = two;
        if (action != null) action();
    }

    protected override void OnPhaseUnload(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Afternoon:
                break;
            case GamePhase.Dusk:
                break;
            case GamePhase.Night:
                break;
            case GamePhase.Latenight:
                break;
            case GamePhase.Dawn:
                break;
            case GamePhase.End:
                break;
        }
    }
}
