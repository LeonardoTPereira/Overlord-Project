using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public struct Audio
        {
            public Audio(AudioSource source, Sound sound)
            {
                Source = source;
                Sound = sound;
            }
            public readonly AudioSource Source;
            public readonly Sound Sound;
        }

        public enum BgmTracks
        {
            DungeonTheme,
            MainMenuTheme,
            LevelSelectTheme,
            VictoryTheme
        }
        
        public enum SfxTracks
        {
            PlayerHit,
            EnemyHit,
            ItemPickup,
            PlayerDeath,
            LockOpen,
            DoorClose,
            EnemyDeath,
            Explosion,
            BulletHit
        }

        [field: SerializeField] private BgmNameToSoundDictionary bgmSoundDictionary;
        [field: SerializeField] private SfxNameToSoundDictionary sfxSoundDictionary;

        private Dictionary<BgmTracks, Audio> bgmAudioDictionary;
        private Dictionary<SfxTracks, Audio> sfxAudioDictionary;

        private void Awake()
        {
            sfxAudioDictionary = new Dictionary<SfxTracks, Audio>();
            bgmAudioDictionary = new Dictionary<BgmTracks, Audio>();
            foreach (var (key, value) in bgmSoundDictionary)
            {
                bgmAudioDictionary.Add(key, new Audio(AddAudioSourceFromSoundSo(value), value));
            }
            foreach (var (key, value) in sfxSoundDictionary)
            {
                sfxAudioDictionary.Add(key, new Audio(AddAudioSourceFromSoundSo(value), value));
            }
        }

        private void OnEnable()
        {
            ISoundEmitter.EmitSoundEventHandler += PlayAudio;
        }
        
        private void OnDisable()
        {
            ISoundEmitter.EmitSoundEventHandler -= PlayAudio;
        }

        private void PlayAudio(object sender, EmitSoundEventArgs eventArgs)
        {
            if (eventArgs.GetType() == typeof(EmitSfxEventArgs))
            {
                PlaySfx((EmitSfxEventArgs)eventArgs);
            }
            else if (eventArgs.GetType() == typeof(PlayBgmEventArgs))
            {
                PlayBgm((PlayBgmEventArgs)eventArgs);
            }
            else if (eventArgs.GetType() == typeof(EmitPitchedSfxEventArgs))
            {
                PlayPitchedSfx((EmitPitchedSfxEventArgs)eventArgs);
            }
        }

        private AudioSource AddAudioSourceFromSoundSo(Sound sound)
        {
            var newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.rolloffMode = sound.AudioMode;
            newAudioSource.outputAudioMixerGroup = sound.Output;
            newAudioSource.clip = sound.Clip;
            newAudioSource.volume = sound.Volume;
            newAudioSource.pitch = sound.Pitch;
            newAudioSource.loop = sound.Loop;
            newAudioSource.playOnAwake = sound.PlayOnAwake;
            newAudioSource.spatialBlend = sound.SpatialBlend;
            newAudioSource.maxDistance = sound.MaxRange;
            return newAudioSource;
        }
        
        private void PlayBgm(PlayBgmEventArgs eventArgs)
        {
            PlaySound(bgmAudioDictionary[eventArgs.BackGroundMusic]);
        }
        private void PlaySfx(EmitSfxEventArgs eventArgs)
        {
            PlayAudioOnce(sfxAudioDictionary[eventArgs.SpecialEffect]);
        }
        private void PlayPitchedSfx(EmitPitchedSfxEventArgs eventArgs)
        {
            PlayAudioOnceRandomizePitch(sfxAudioDictionary[eventArgs.SpecialEffect], eventArgs.Pitch);
        }

        private void PlaySound(Audio sound)
        {
            if (sound.Source.isPlaying) return;
            StopOtherSounds();
            sound.Source.Play();
        }
     
        public void PlayAudioOnce(Audio sound)
        {
            if (sound.Source == null)
                return;
            sound.Source.PlayOneShot(sound.Source.clip);
        }
     
        public void PlayAudioOnceRandomizePitch(Audio sound, int pitchOffset)
        {
            if (sound.Source == null)
                return;
            sound.Source.pitch = sound.Sound.Pitch + pitchOffset;
            sound.Source.PlayOneShot(sound.Source.clip);
            sound.Source.pitch = sound.Sound.Pitch;
        }

        public void FadeOutAudio(Audio sound)
        {
            StartCoroutine(StartFade(sound.Source, 2, 0));
        }
     
        public IEnumerator StartFade(AudioSource fadeSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = fadeSource.volume;
     
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                fadeSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
        }
     
        public void StopAudio(Audio sound)
        {
            if (sound.Source == null)
                return;
            sound.Source.Stop();
        }

        private void StopOtherSounds()
        {
            foreach (var bgmAudio in bgmAudioDictionary)
            {
                StopAudio(bgmAudio.Value);
            }
        }
    }
}