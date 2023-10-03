using UnityEngine;
using UnityEngine.Audio;

namespace Game.MenuManager
{
    public class SettingsMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;

        private void Start()
        {
            SetVolume(PlayerPrefs.GetFloat("MainVolume", 0f));
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0f));
            SetEffectsVolume(PlayerPrefs.GetFloat("SfxVolume", 0f));
        }

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("MainVolume", volume);
            PlayerPrefs.SetFloat("MainVolume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume", volume);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        public void SetEffectsVolume(float volume)
        {
            audioMixer.SetFloat("SfxVolume", volume);
            PlayerPrefs.SetFloat("SfxVolume", volume);
        }
    }
}