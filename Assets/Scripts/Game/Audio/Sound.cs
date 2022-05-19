//Thanks to https://twitter.com/TheSarahChimera for the Script!
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    [CreateAssetMenu(menuName = "Game Systems/Audio/Sound")]
    public class Sound : ScriptableObject
    {
        [SerializeField] private float pitchMin;
        [SerializeField] private float pitchMax;
        [SerializeField] private float maxRange = 3000f;
        [SerializeField] private AudioRolloffMode audioMode;
        [SerializeField]
        [Header("3D Sound")]
        [Range(0, 1f)] private float spatialBlend = 1f;
        [SerializeField] [Range(-3f, 3f)] private float pitch = 1f;
        [SerializeField]
        [Header("Volume and Pitch")]
        [Range(0f, 1f)] private float volume = 0.5f;
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool loop;
        [SerializeField] private AudioMixerGroup output;
        [SerializeField] [Header("Clip and Output")] private AudioClip clip;

        public AudioClip Clip => clip;
        public AudioMixerGroup Output => output;
        public bool Loop => loop;
        public bool PlayOnAwake => playOnAwake;
        public float Volume => volume;
        public float Pitch => pitch;
        public float PitchMin => pitchMin;
        public float PitchMax => pitchMax;
        public float SpatialBlend => spatialBlend;
        public AudioRolloffMode AudioMode => audioMode;
        public float MaxRange => maxRange;
    }
}