using System.Collections.Generic;
using UnityEngine;

public class Audio3D : MonoBehaviour
{
    [Header("Sounds to use")]
    [SerializeField]
    protected bool forestStepSounds;
    [SerializeField]
    protected bool woodStepSounds, stoneStepSounds, wetStepSounds, swimSounds, dryStepSounds, goreSounds, soundEffectsSounds, attackSounds, animalSounds, dragonSounds, dialogueSounds;

    private List<Sound> footstep;
    protected List<Sound> forestStep, woodStep, stoneStep, wetStep, dryStep, swim, gore, soundEffects, attack, dragon, animal, dialogue;
    protected AudioManager audioManager;
    private float walkTime = 0;
    private int stepIndex = 0;
    private int goreIndex = 0;

    void Start()
    {
        audioManager = AudioManager.Instance;
        if(goreSounds)
            gore = InitializeSounds("gore");
        if(soundEffectsSounds)
            soundEffects = InitializeSounds("soundEffects");
        if (forestStepSounds)
            forestStep = InitializeSounds("forestStep");
        if (woodStepSounds)
            woodStep = InitializeSounds("woodStep");
        if (swimSounds)
            swim = InitializeSounds("swim");
        if (stoneStepSounds)
            stoneStep = InitializeSounds("stoneStep");
        if (wetStepSounds)
            wetStep = InitializeSounds("wetStep");
        if (dryStepSounds)
            dryStep = InitializeSounds("dryStep");
        if (attackSounds)
            attack = InitializeSounds("attack");
        if (animalSounds)
            animal = InitializeSounds("animal");
        if (dragonSounds)
            dragon = InitializeSounds("dragon", true);
        if (dialogueSounds)
            dialogue = InitializeSounds("dialogue");
        footstep = forestStep;
    }

    private List<Sound> InitializeSounds(string arrName, bool isLoud = false)
    {
        List<Sound> soundList = new List<Sound>();

        foreach (Sound sound in audioManager.GetSounds(arrName))
        {
            Sound s = new Sound();
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.spatialBlend = 1f;
            s.source.minDistance = 4f;
            s.source.volume = sound.volume;
            s.source.clip = sound.clip;
            s.name = sound.name;

            if (isLoud) s.source.minDistance = 20f;

            soundList.Add(s);
        }
        return soundList;
    }

    public void ChangeFootstep(LayerMask layer)
    {
        if (layer == LayerMask.NameToLayer("DeepWater"))
            footstep = swim;
        else if (layer == LayerMask.NameToLayer("Rocks"))
            footstep = stoneStep;
        else if (layer == LayerMask.NameToLayer("Wood"))
            footstep = woodStep;
        else if (layer == LayerMask.NameToLayer("Water"))
            footstep = wetStep;
        else if (layer == LayerMask.NameToLayer("Default"))
            footstep = dryStep;
        else
            footstep = forestStep;
    }

    public void Walk()
    {
        if (footstep == null)
        {
            throw new System.Exception("Footstep Array is empty!");
        }

        if (stepIndex >= footstep.Count) stepIndex = 0;
        footstep[stepIndex].source.Play();

        stepIndex++;
    }


    public void Boulder()
    {
        Dragon("Boulder" + Random.Range(1, 5));
    }

    public void SoundEffect(string name)
    {
        Sound sound = null;

        foreach(Sound s in soundEffects)
        {
            if(s.name == name)
            {
                sound = s;
                break;
            }
        }


        if (sound == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }

        sound.source.Play();
    }

    public void Attack(string name)
    {
        Sound sound = null;

        foreach (Sound s in attack)
        {
            if (s.name == name)
            {
                sound = s;
                break;
            }
        }


        if (sound == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }

        sound.source.Play();
    }

    public void Dialogue(string name)
    {
        Sound sound = null;

        foreach (Sound s in dialogue)
        {
            if (s.name == name)
            {
                sound = s;
                break;
            }
        }


        if (sound == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }

        sound.source.Play();
    }

    public void Dragon(string name)
    {
        Sound sound = null;

        foreach (Sound s in dragon)
        {
            if (s.name == name)
            {
                sound = s;
                break;
            }
        }


        if (sound == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }

        sound.source.Play();
    }

    public void Animal(string name)
    {
        Sound sound = null;

        foreach (Sound s in animal)
        {
            if (s.name == name)
            {
                sound = s;
                break;
            }
        }


        if (sound == null)
        {
            Debug.Log("Audio " + name + " Not found!");
            return;
        }

        sound.source.Play();
    }
}
