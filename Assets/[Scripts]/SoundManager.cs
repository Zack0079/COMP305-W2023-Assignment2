using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SoundManager : MonoBehaviour
{
  public List<AudioSource> audioSources;
  public List<AudioClip> audioClips;

  private void Awake()
  {
    audioSources = GetComponents<AudioSource>().ToList();
    audioClips = new List<AudioClip>();
    InitializeSoundFX();
  }

  private void InitializeSoundFX()
  {
    //preload sound FX
    audioClips.Add(Resources.Load<AudioClip>("Audio/jump-sound")); //0
    audioClips.Add(Resources.Load<AudioClip>("Audio/hurt-sound")); //1
    audioClips.Add(Resources.Load<AudioClip>("Audio/gem-sound")); //2

    //preload music

  }

  public void PlaySoundFX(Channel channel, SoundFX sound)
  {
    audioSources[(int)channel].clip = audioClips[(int)sound]; // loads the clips
    audioSources[(int)channel].Play();
  }

  public void PlayMusic()
  {
    audioSources[(int)Channel.MUSIC].clip = audioClips[(int)SoundFX.MUSIC]; // loads the clips
    audioSources[(int)Channel.MUSIC].volume = 0.25f;
    audioSources[(int)Channel.MUSIC].loop = true;
    audioSources[(int)Channel.MUSIC].Play();
  }
}
