using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    protected Sound[] dialogs, forestStep, woodStep, stoneStep, wetStep, dryStep, background, soundEffects, attack, gore, swim, dragon, animal;

    private Sound currentBGM;
    private Sound bgmToChange;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        InitializeSounds(background);
        InitializeSounds(dialogs);
        InitializeSounds(soundEffects);

        SettingsMenu settings = SettingsMenu.Instance;
        if (settings != null) SetVolume(settings.GetVolume());
    }

    private void Update()
    {
        SwitchBGMUpdate();
    }

    private void InitializeSounds(Sound[] sounds)
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.loop = s.loop;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.initialVolume = s.volume;
        }
    }

    public void SetVolume(float volume, string arrName = "")
    {
        bool setAll = false;
        if (arrName.Length <= 0) setAll = true;

        if (arrName == "dialogs" || setAll)
            ChangeVolume(volume, dialogs);
        if (arrName == "forestStep" || setAll)
            ChangeVolume(volume, forestStep);
        if (arrName == "woodStep" || setAll)
            ChangeVolume(volume, woodStep);
        if (arrName == "stoneStep" || setAll)
            ChangeVolume(volume, stoneStep);
        if (arrName == "wetStep" || setAll)
            ChangeVolume(volume, wetStep);
        if (arrName == "background" || setAll)
            ChangeVolume(volume, background);
        if (arrName == "soundEffects" || setAll)
            ChangeVolume(volume, soundEffects);
        if (arrName == "attack" || setAll)
            ChangeVolume(volume, attack);
        if (arrName == "gore" || setAll)
            ChangeVolume(volume, gore);
        if (arrName == "swim" || setAll)
            ChangeVolume(volume, swim);
        if (arrName == "dragon" || setAll)
            ChangeVolume(volume, dragon);
        if (arrName == "animal" || setAll)
            ChangeVolume(volume, animal);
    }

    private void ChangeVolume(float volume, Sound[] sounds)
    {
        foreach (Sound s in sounds)
        {
            if (s.source == null) continue;
            s.source.volume = s.initialVolume * volume;
        }
    }

    public void SoundEffect(string name, bool playOnce = false)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }

        if (playOnce)
        {
            if (s.source.isPlaying)
                return;
        }
        s.source.Play();
    }

    public void Background(string name, bool isBGM = false, bool isMuted = false)
    {
        Sound s = Array.Find(background, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }
        if (s.source != null)
        {
            if (isMuted) s.source.volume = 0;
            s.source.Play();
        }

        if (isBGM) currentBGM = s;
    }

    public void StopBackground(string name)
    {
        Sound s = Array.Find(background, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }
        s.source.Stop();
    }

    public string GetBGM()
    {
        return currentBGM.name;
    }

    public void SwitchBGM(string name)
    {
        bgmToChange = Array.Find(background, sound => sound.name == name);
        Background(name, false, true);
    }

    private void SwitchBGMUpdate()
    {
        if (currentBGM == bgmToChange || bgmToChange == null) return;

        currentBGM.source.volume = Mathf.Lerp(currentBGM.source.volume, 0, 2 * Time.deltaTime);
        bgmToChange.source.volume = Mathf.Lerp(bgmToChange.source.volume, 0.82f, 2 * Time.deltaTime);

        if (bgmToChange.source.volume >= 0.8)
        {
            StopBackground(currentBGM.name);
            currentBGM = bgmToChange;
        }
    }

    public Sound[] GetSounds(string type)
    {
        if (type == "gore")
            return gore;
        else if (type == "soundEffects")
            return soundEffects;
        else if (type == "swim")
            return swim;
        else if (type == "woodStep")
            return woodStep;
        else if (type == "stoneStep")
            return stoneStep;
        else if (type == "wetStep")
            return wetStep;
        else if (type == "attack")
            return attack;
        else if (type == "dragon")
            return dragon;
        else if (type == "animal")
            return animal;
        else if (type == "dryStep")
            return dryStep;
        else if (type == "dialogue")
            return dialogs;
        else
            return forestStep;
    }
}
