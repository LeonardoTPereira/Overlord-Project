using UnityEngine;
using UnityEngine.UI;

namespace Game.MenuManager
{
    public class VolumePanel : MonoBehaviour
    {
        [SerializeField] private Slider masterVolumeSlider, effectsVolumeSlider, musicVolumeSlider;

        private void Awake()
        {
            var audioMixer = FindObjectOfType<SettingsMenu>().audioMixer;

            audioMixer.GetFloat("main-volume", out var masterValue);
            masterVolumeSlider.value = masterValue;

            audioMixer.GetFloat("effects-volume", out var effectsValue);
            effectsVolumeSlider.value = effectsValue;

            audioMixer.GetFloat("music-volume", out var musicValue);
            musicVolumeSlider.value = musicValue;
        }
    }
}